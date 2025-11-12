using backend.models;
using backend.services;

namespace backend.controllers;

using Microsoft.AspNetCore.Mvc;
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
    // TODO: Keep this route, but potentially add another endpoint for case-insensitive lookup in partial strings in the names category.
    public ActionResult<ModelItem> GetById(string name)
    {
        var item = _catalog.All.FirstOrDefault(m => m.Name == name);
        return item is null ? NotFound() : Ok(item);
    }

}