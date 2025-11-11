using backend.models;

namespace backend.services;

using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public interface IModelCatalog
{
    IReadOnlyList<ModelItem> All { get; }
}

public sealed class FileModelCatalog : IModelCatalog
{
    public IReadOnlyList<ModelItem> All { get; }

    public FileModelCatalog(string jsonPath)
    {
        var json = File.ReadAllText(jsonPath);
        var items = JsonSerializer.Deserialize<List<ModelItem>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<ModelItem>();

        All = items;
    }
}