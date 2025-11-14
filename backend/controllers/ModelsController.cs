using backend.models;
using backend.database;
using backend.services;

namespace backend.controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("3dmodels")]
public sealed class ModelsController : ControllerBase
{
    private readonly PrintforgeDbContext _db;
    private readonly backend.services.ILikesService _likes;
    private readonly backend.services.ILikeRateLimiter _limiter;
    private readonly IModelsQueryService _modelsQuery;
    public ModelsController(PrintforgeDbContext db,
                            backend.services.ILikesService likes,
                            backend.services.ILikeRateLimiter limiter,
                            IModelsQueryService modelsQuery)
    {
        _db = db;
        _likes = likes;
        _limiter = limiter;
        _modelsQuery = modelsQuery;
    }

    // Enhanced listing endpoint: supports category filter, text query, sorting, and pagination.
    // Returns a paged response with totalCount and items.
    // PagedResult<T> moved to backend/models/PagedResult.cs
    
    // ## Usage examples
    /*- First page (default sort):
    - `GET https://localhost:5001/3dmodels?skip=0&take=24`
    - Filtered by category, newest first:
    - `GET https://localhost:5001/3dmodels?category=household&sort=date&order=desc&skip=0&take=12`
    - Search by text, alphabetical:
    - `GET https://localhost:5001/3dmodels?q=planter&sort=name&order=asc&skip=0&take=24`*/
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ModelItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<ModelItem>>> Get(
        [FromQuery] string? category,
        [FromQuery] string? q,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        [FromQuery] string sort = "likes",
        [FromQuery] string order = "desc")
    {
        var result = await _modelsQuery.BrowseAsync(new BrowseQuery
        {
            Category = category,
            Q = q,
            Skip = skip,
            Take = take,
            Sort = sort,
            Order = order
        }, HttpContext.RequestAborted);

        return Ok(result);
    }

    // Helper kept small and focused; extracts the likes-based ordering logic
    private static IEnumerable<ModelItem> SortByLikes(IEnumerable<ModelItem> items, bool desc)
        => desc ? items.OrderByDescending(m => m.Likes)
                : items.OrderBy(m => m.Likes);

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ModelItem>> GetById(int id)
    {
        var m = await _db.Models.FirstOrDefaultAsync(x => x.Id == id);
        var item = m is null ? null : new ModelItem
        {
            Id = m.Id,
            Name = m.Name,
            Description = m.Description,
            Likes = m.Likes,
            Image = m.Image,
            Category = m.Category,
            DateAdded = m.DateAdded
        };
        return item is null ? NotFound() : Ok(item);
    }
    
    [HttpGet("{name}")]
    public async Task<ActionResult<ModelItem>> GetById(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Name must be provided.");
        }

        var q = name.Trim();

        // Case-insensitive partial match: returns the first matching item
        var m = await _db.Models
            .Where(m => m.Name != null && m.Name.Contains(q))
            .OrderBy(m => m.Name)
            .FirstOrDefaultAsync();

        var item = m is null ? null : new ModelItem
        {
            Id = m.Id,
            Name = m.Name,
            Description = m.Description,
            Likes = m.Likes,
            Image = m.Image,
            Category = m.Category,
            DateAdded = m.DateAdded
        };

        return item is null ? NotFound() : Ok(item);
    }
    
    // Lists all category names (distinct, case-insensitive). Supports optional filtering by partial, case-insensitive search.
    // Example: GET /3dmodels/categories          -> ["art", "education", ...]
    //          GET /3dmodels/categories?query=ed -> ["education", ...]
    [HttpGet("categories")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories([FromQuery] string? query)
    {
        var q = string.IsNullOrWhiteSpace(query) ? null : query.Trim();
        var categoriesQuery = _db.Models
            .Where(m => m.Category != null)
            .Select(m => m.Category!)
            .Distinct();

        if (q is not null)
            categoriesQuery = categoriesQuery.Where(c => c.Contains(q));

        var result = await categoriesQuery.OrderBy(c => c).ToListAsync();
        return Ok(result);
    }

    // Uploads are disabled; return 410 Gone to signal deprecation
    [HttpPost]
    public IActionResult Create()
    {
        return StatusCode(StatusCodes.Status410Gone, "Uploads are disabled in this environment.");
    }

    // Increment like count (with per-client rate limiting)
    [HttpPost("{id:int}/like")]
    public async Task<IActionResult> Like(int id)
    {
        if (!_limiter.TryAcquire(HttpContext, out var retryAfter))
        {
            // 429 Too Many Requests with Retry-After header (in seconds)
            Response.Headers["Retry-After"] = Math.Ceiling(retryAfter.TotalSeconds).ToString();
            return StatusCode(StatusCodes.Status429TooManyRequests, $"Rate limit exceeded. Retry after {retryAfter.TotalMilliseconds:F0} ms");
        }

        var ok = await _likes.IncrementAsync(id, HttpContext.RequestAborted);
        if (!ok) return NotFound();
        return NoContent();
    }

    // Return the first WebP thumbnail for a model
    [HttpGet("{id:int}/thumbnail")]
    public async Task<IActionResult> GetThumbnail(int id)
    {
        var webp = await _db.ModelWebpPlaceholders.Where(b => b.ModelId == id)
                                                  .OrderBy(b => b.Id)
                                                  .FirstOrDefaultAsync();
        if (webp is null) return NotFound();
        return File(webp.Data, webp.ContentType, webp.FileName);
    }
}
