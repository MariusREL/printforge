using backend.models;
using backend.services;

namespace backend.controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

[ApiController]
[Route("3dmodels")]
public sealed class ModelsController(IModelCatalog catalog) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<ModelItem>> Get([FromQuery] string? category)
    {
        var items = catalog.All;
        if (!string.IsNullOrWhiteSpace(category))
        {
            items = items.Where(m => string.Equals(m.Category, category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public ActionResult<ModelItem> GetById(int id)
    {
        var item = catalog.All.FirstOrDefault(m => m.Id == id);
        return item is null ? NotFound() : Ok(item);
    }
    
    /// <summary>
    /// Search models by partial name (case-insensitive). Returns a list.
    /// Extendable to also search Description/Category and add pagination.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<ModelItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<ModelItem>> Search(
        [FromQuery] string? query,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        [FromQuery] bool includeDescription = false, // TODO: set default to true if you want to search description by default
        [FromQuery] bool includeCategory = false     // TODO: set default to true if you want to search category by default
    )
    {
        // 1) Validate input
        if (string.IsNullOrWhiteSpace(query))
        {
            // TODO: If you prefer returning all items when query is empty, replace BadRequest with Ok(catalog.All)
            return BadRequest("Query parameter 'query' is required.");
        }

        // 2) Normalize the query
        var q = query.Trim();

        // TODO: If you want to be more permissive, normalize hyphens/underscores/spaces here
        // Example:
        // q = q.Replace('-', ' ').Replace('_', ' ');

        // 3) Minimum query length to avoid super broad scans
        const int minLength = 2; // TODO: tune this
        if (q.Length < minLength)
        {
            return BadRequest($"Query must be at least {minLength} characters long.");
        }

        // 4) Base set
        var items = catalog.All.AsEnumerable();

        // 5) Case-insensitive partial matching
        // Note: string.Contains(string, StringComparison) is available on modern .NET.
        // If you need culture-aware behavior, you can use CompareInfo.IndexOf instead.

        bool NameMatches(ModelItem m) =>
            (!string.IsNullOrEmpty(m.Name)) &&
            m.Name.Contains(q, StringComparison.OrdinalIgnoreCase);

        bool DescriptionMatches(ModelItem m) =>
            includeDescription && !string.IsNullOrEmpty(m.Description) &&
            m.Description.Contains(q, StringComparison.OrdinalIgnoreCase);

        bool CategoryMatches(ModelItem m) =>
            includeCategory && !string.IsNullOrEmpty(m.Category) &&
            m.Category.Contains(q, StringComparison.OrdinalIgnoreCase);

        var filtered = items.Where(m => NameMatches(m) || DescriptionMatches(m) || CategoryMatches(m));

        // 6) Ordering
        // TODO: Choose how to sort results. A few options below—uncomment one and delete the others.

        // Option A: Most popular first
        filtered = filtered.OrderByDescending(m => m.Likes);

        // Option B: Alphabetical by name
        // filtered = filtered.OrderBy(m => m.Name);

        // Option C: Newest first (if you parse DateTime from your model)
        // filtered = filtered.OrderByDescending(m => m.DateAdded);

        // 7) Pagination guards
        const int maxTake = 50; // TODO: tune this cap
        if (take > maxTake) take = maxTake;
        if (skip < 0) skip = 0;
        if (take < 0) take = 0;

        var results = filtered.Skip(skip).Take(take).ToList();

        // 8) Response behavior when no matches
        // TODO: Keep 200 OK with [] (recommended for search), or switch to NotFound() if you prefer.
        return Ok(results);
    }
    
    [HttpGet("{name}")]
    public ActionResult<ModelItem> GetByName(string name)
    {
        var item = catalog.All.FirstOrDefault(m => m.Name == name);
        return item is null ? NotFound() : Ok(item);
    }

}