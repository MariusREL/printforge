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

    [HttpGet]
    public ActionResult<IEnumerable<ModelItem>> Get([FromQuery] string? category)
    {
        var items = _catalog.All;
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