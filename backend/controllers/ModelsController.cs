using backend.models;
using backend.database;

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
    public ModelsController(PrintforgeDbContext db) => _db = db;

    // Enhanced listing endpoint: supports category filter, text query, sorting, and pagination.
    // Returns a paged response with totalCount and items.
    private sealed record PagedResult<T>(int TotalCount, List<T> Items);
    
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
    public async Task<ActionResult<object>> Get(
        [FromQuery] string? category,
        [FromQuery] string? q,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        [FromQuery] string sort = "likes",
        [FromQuery] string order = "desc")
    {
        // 1) Validate and normalize inputs
        if (skip < 0) return BadRequest("Parameter 'skip' must be >= 0.");
        if (take < 0) return BadRequest("Parameter 'take' must be >= 0.");
        const int maxTake = 100;
        if (take > maxTake) take = maxTake;

        sort = (sort ?? "likes").Trim().ToLowerInvariant();
        order = (order ?? "desc").Trim().ToLowerInvariant();
        var isDesc = order == "desc";

        bool IsValidSort(string s) => s is "likes" or "date" or "name";
        bool IsValidOrder(string s) => s is "asc" or "desc";
        if (!IsValidSort(sort)) return BadRequest("Parameter 'sort' must be one of: likes, date, name.");
        if (!IsValidOrder(order)) return BadRequest("Parameter 'order' must be one of: asc, desc.");

        var categoryNorm = string.IsNullOrWhiteSpace(category) ? null : category!.Trim();
        var query = string.IsNullOrWhiteSpace(q) ? null : q!.Trim();

        // 2) Base set
        var queryable = _db.Models.AsQueryable();

        // 3) Filters
        if (categoryNorm is not null)
            queryable = queryable.Where(m => m.Category != null && m.Category.Equals(categoryNorm));

        if (query is not null)
            queryable = queryable.Where(m =>
                (m.Name != null && EF.Functions.Like(m.Name, $"%{query}%")) ||
                (m.Description != null && EF.Functions.Like(m.Description, $"%{query}%")));

        // 4) Total before paging
        var totalCount = await queryable.CountAsync();

        // 5) Sorting
        queryable = sort switch
        {
            "likes" => isDesc ? queryable.OrderByDescending(m => m.Likes)
                               : queryable.OrderBy(m => m.Likes),
            "date"  => isDesc ? queryable.OrderByDescending(m => m.DateAdded)
                               : queryable.OrderBy(m => m.DateAdded),
            "name"  => isDesc ? queryable.OrderByDescending(m => m.Name)
                               : queryable.OrderBy(m => m.Name),
            _ => isDesc ? queryable.OrderByDescending(m => m.Likes)
                        : queryable.OrderBy(m => m.Likes)
        };

        // 6) Pagination
        var page = await queryable.Skip(skip).Take(take)
                                  .Select(m => new ModelItem
                                  {
                                      Id = m.Id,
                                      Name = m.Name,
                                      Description = m.Description,
                                      Likes = m.Likes,
                                      Image = m.Image,
                                      Category = m.Category,
                                      DateAdded = m.DateAdded
                                  })
                                  .ToListAsync();

        // 7) Response
        var result = new PagedResult<ModelItem>(totalCount, page);
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

    // Upload a new model with STL file
    // multipart/form-data fields: name, description, image (optional URL), category, dateAdded (optional ISO), stl (file)
    [HttpPost]
    [RequestSizeLimit(100_000_000)]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ModelItem>> Create([FromForm] string name,
                                                      [FromForm] string description,
                                                      [FromForm] string? image,
                                                      [FromForm] string category,
                                                      [FromForm] DateTime? dateAdded,
                                                      [FromForm] IFormFile stl)
    {
        if (stl is null || stl.Length == 0)
            return BadRequest("STL file is required.");

        var record = new ModelRecord
        {
            Name = name,
            Description = description,
            Image = image ?? string.Empty,
            Category = category,
            DateAdded = dateAdded ?? DateTime.UtcNow,
            Likes = 0
        };

        await _db.Models.AddAsync(record);
        await _db.SaveChangesAsync();

        await using var ms = new MemoryStream();
        await stl.CopyToAsync(ms);
        var blob = new ModelStlBlob
        {
            ModelId = record.Id,
            FileName = string.IsNullOrWhiteSpace(stl.FileName) ? $"model-{record.Id}.stl" : stl.FileName,
            ContentType = string.IsNullOrWhiteSpace(stl.ContentType) ? "application/sla" : stl.ContentType,
            Data = ms.ToArray()
        };
        await _db.ModelStlBlobs.AddAsync(blob);
        await _db.SaveChangesAsync();

        var result = new ModelItem
        {
            Id = record.Id,
            Name = record.Name,
            Description = record.Description,
            Likes = record.Likes,
            Image = record.Image,
            Category = record.Category,
            DateAdded = record.DateAdded
        };

        return CreatedAtAction(nameof(GetById), new { id = record.Id }, result);
    }

    // Increment like count
    [HttpPost("{id:int}/like")]
    public async Task<IActionResult> Like(int id)
    {
        var model = await _db.Models.FirstOrDefaultAsync(m => m.Id == id);
        if (model is null) return NotFound();
        model.Likes += 1;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Download the first STL file for a model
    [HttpGet("{id:int}/stl")]
    public async Task<IActionResult> DownloadStl(int id)
    {
        var blob = await _db.ModelStlBlobs.Where(b => b.ModelId == id)
                                          .OrderBy(b => b.Id)
                                          .FirstOrDefaultAsync();
        if (blob is null) return NotFound();
        return File(blob.Data, blob.ContentType, blob.FileName);
    }
}
