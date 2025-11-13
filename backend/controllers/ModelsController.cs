using backend.models;
using backend.services;

namespace backend.controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("3dmodels")]
public sealed class ModelsController : ControllerBase
{
    private readonly IModelCatalog _catalog;
    public ModelsController(IModelCatalog catalog) => _catalog = catalog;

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
    public ActionResult<object> Get(
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
        var enumerable = _catalog.All.AsEnumerable();

        // 3) Filters
        if (categoryNorm is not null)
        {
            enumerable = enumerable.Where(m => !string.IsNullOrEmpty(m.Category) &&
                                                string.Equals(m.Category, categoryNorm, StringComparison.OrdinalIgnoreCase));
        }

        if (query is not null)
        {
            enumerable = enumerable.Where(m =>
                (!string.IsNullOrEmpty(m.Name) && m.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(m.Description) && m.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
            );
        }

        // 4) Total before paging
        var totalCount = enumerable.Count();

        // 5) Sorting
        enumerable = sort switch
        {
            "likes" => SortByLikes(enumerable, isDesc),
            "date"  => isDesc
                ? enumerable.OrderByDescending(m => m.DateAdded)
                : enumerable.OrderBy(m => m.DateAdded),
            "name"  => isDesc
                ? enumerable.OrderByDescending(m => m.Name, StringComparer.OrdinalIgnoreCase)
                : enumerable.OrderBy(m => m.Name, StringComparer.OrdinalIgnoreCase),
            _ => SortByLikes(enumerable, isDesc)
        };

        // 6) Pagination
        var page = enumerable.Skip(skip).Take(take).ToList();

        // 7) Response
        var result = new PagedResult<ModelItem>(totalCount, page);
        return Ok(result);
    }

    // Helper kept small and focused; extracts the likes-based ordering logic
    private static IEnumerable<ModelItem> SortByLikes(IEnumerable<ModelItem> items, bool desc)
        => desc ? items.OrderByDescending(m => m.Likes)
                : items.OrderBy(m => m.Likes);

    [HttpGet("{id:int}")]
    public ActionResult<ModelItem> GetById(int id)
    {
        var item = _catalog.All.FirstOrDefault(m => m.Id == id);
        return item is null ? NotFound() : Ok(item);
    }
    
    [HttpGet("{name}")]
    public ActionResult<ModelItem> GetById(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Name must be provided.");
        }

        var q = name.Trim();

        // Case-insensitive partial match: returns the first matching item
        var item = _catalog.All
            .FirstOrDefault(m => !string.IsNullOrEmpty(m.Name) &&
                                 m.Name.Contains(q, StringComparison.OrdinalIgnoreCase));

        return item is null ? NotFound() : Ok(item);
    }
    
    // Lists all category names (distinct, case-insensitive). Supports optional filtering by partial, case-insensitive search.
    // Example: GET /3dmodels/categories          -> ["art", "education", ...]
    //          GET /3dmodels/categories?query=ed -> ["education", ...]
    [HttpGet("categories")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<string>> GetCategories([FromQuery] string? query)
    {
        var categories = _catalog.All
            .Select(m => m.Category)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var q = query.Trim();
            categories = categories.Where(c => c.Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        var result = categories
            .OrderBy(c => c, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return Ok(result);
    }
}
