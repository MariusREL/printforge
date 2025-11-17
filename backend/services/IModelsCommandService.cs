using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using backend.database;
using backend.models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.services;

public interface IModelsCommandService
{
    /// <summary>
    /// Creates a new model row with Likes = 0 and DateAdded set by DB default (UTC),
    /// stores the provided WebP thumbnail, and returns the created item.
    /// </summary>
    Task<ModelItem> CreateAsync(string name, string description, string category, IFormFile webp,
        CancellationToken ct = default);
}

public sealed class ModelsCommandService : IModelsCommandService
{
    private readonly PrintforgeDbContext _db;
    public ModelsCommandService(PrintforgeDbContext db) => _db = db;

    public async Task<ModelItem> CreateAsync(string name, string description, string category, IFormFile webp,
        CancellationToken ct = default)
    {
        // Basic validation (simple and readable)
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required", nameof(description));
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category is required", nameof(category));
        if (webp is null || webp.Length == 0) throw new ArgumentException("WebP image file is required", nameof(webp));

        // Content type check: prefer image/webp, but accept .webp by extension if ContentType missing
        var isWebp = string.Equals(webp.ContentType, "image/webp", StringComparison.OrdinalIgnoreCase)
                     || Path.GetExtension(webp.FileName).Equals(".webp", StringComparison.OrdinalIgnoreCase);
        if (!isWebp)
            throw new ArgumentException("Only WebP images are supported (image/webp)", nameof(webp));

        // Optional: size cap to avoid abuse in dev (2 MB)
        const long maxBytes = 2 * 1024 * 1024;
        if (webp.Length > maxBytes)
            throw new ArgumentException($"WebP too large. Max {maxBytes / 1024 / 1024} MB.", nameof(webp));

        // Create the model record. Likes left as 0 (default), DateAdded by DB default (GETUTCDATE()).
        var record = new ModelRecord
        {
            Name = name.Trim(),
            Description = description.Trim(),
            Category = category.Trim(),
            Image = string.Empty, // will set to thumbnail URL after Id is known
            Likes = 0
        };

        await _db.Models.AddAsync(record, ct);
        await _db.SaveChangesAsync(ct); // gets the generated Id

        // Save thumbnail bytes
        byte[] bytes;
        await using (var ms = new MemoryStream())
        {
            await webp.CopyToAsync(ms, ct);
            bytes = ms.ToArray();
        }

        var placeholder = new ModelWebpPlaceholder
        {
            ModelId = record.Id,
            FileName = string.IsNullOrWhiteSpace(webp.FileName) ? $"{record.Id}_thumbnail.webp" : webp.FileName,
            ContentType = "image/webp",
            Data = bytes
        };
        await _db.ModelWebpPlaceholders.AddAsync(placeholder, ct);

        // Set Image to the API thumbnail route so it aligns with the JSON schema (a string URL)
        record.Image = $"/3dmodels/{record.Id}/thumbnail";
        _db.Models.Update(record);

        await _db.SaveChangesAsync(ct);

        // Compose response DTO. DateAdded is filled by DB; fetch the value.
        // record.DateAdded should now be populated since we tracked the entity.
        return new ModelItem
        {
            Id = record.Id,
            Name = record.Name,
            Description = record.Description,
            Likes = record.Likes,
            Image = record.Image,
            Category = record.Category,
            DateAdded = record.DateAdded
        };
    }
}
