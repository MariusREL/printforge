using backend.models;
using backend.services;

namespace backend.controllers;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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
    
    [HttpGet("{name}")]
    public ActionResult<ModelItem> GetByName(string name)
    {
        var item = catalog.All.FirstOrDefault(m => m.Name == name);
        return item is null ? NotFound() : Ok(item);
    }

}