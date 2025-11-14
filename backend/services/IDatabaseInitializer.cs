using System.Text.Json;
using backend.database;
using backend.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.services;

public interface IDatabaseInitializer
{
    Task InitializeAsync(CancellationToken ct = default);
}

/// <summary>
/// Creates or resets the database schema and seeds it from IModelCatalog and local WebP placeholders.
/// </summary>
public sealed class DatabaseInitializer : IDatabaseInitializer
{
    private readonly PrintforgeDbContext _db;
    private readonly ILogger<DatabaseInitializer> _logger;
    private readonly IHostEnvironment _env;
    private readonly IModelCatalog _catalog;

    public DatabaseInitializer(PrintforgeDbContext db,
                               ILogger<DatabaseInitializer> logger,
                               IHostEnvironment env,
                               IModelCatalog catalog)
    {
        _db = db;
        _logger = logger;
        _env = env;
        _catalog = catalog;
    }

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        // 1) Ensure database exists; if it already exists, we'll drop all tables next
        var creator = _db.Database.GetService<IRelationalDatabaseCreator>();
        if (!await creator.ExistsAsync(ct))
        {
            _logger.LogInformation("Database does not exist. Creating database and tables...");
            await creator.CreateAsync(ct);
            await creator.CreateTablesAsync(ct);
        }
        else
        {
            _logger.LogInformation("Database exists. Dropping all tables (reset to seed)...");
            await DropAllTablesAsync(ct);
            _logger.LogInformation("Recreating tables...");
            await creator.CreateTablesAsync(ct);
        }

        // 2) Seed models from catalog (IDs are fixed; use IDENTITY_INSERT)
        if (_catalog.All.Count > 0)
        {
            var records = _catalog.All.Select(m => new ModelRecord
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Likes = m.Likes,
                Image = m.Image,
                Category = m.Category,
                DateAdded = m.DateAdded
            }).ToList();

            await _db.Database.OpenConnectionAsync(ct);
            try
            {
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Models] ON", ct);
                _db.Models.AddRange(records);
                await _db.SaveChangesAsync(ct);
            }
            finally
            {
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Models] OFF", ct);
                await _db.Database.CloseConnectionAsync();
            }

            // 3) Seed WebP placeholders
            var seedDir = Path.Combine(_env.ContentRootPath, "data", "database_seeding");
            var webpDir = Path.Combine(seedDir, "webp_placeholders");
            if (Directory.Exists(webpDir))
            {
                var files = Directory.EnumerateFiles(webpDir, "*.webp", SearchOption.TopDirectoryOnly).ToList();
                foreach (var model in records)
                {
                    var matches = files.Where(f => Path.GetFileName(f)
                        .StartsWith(model.Id + "_", StringComparison.OrdinalIgnoreCase)).ToList();
                    foreach (var path in matches)
                    {
                        var bytes = await File.ReadAllBytesAsync(path, ct);
                        _db.ModelWebpPlaceholders.Add(new ModelWebpPlaceholder
                        {
                            ModelId = model.Id,
                            FileName = Path.GetFileName(path),
                            ContentType = "image/webp",
                            Data = bytes
                        });
                    }
                }
                if (_db.ChangeTracker.HasChanges())
                    await _db.SaveChangesAsync(ct);
            }
        }

        _logger.LogInformation("Database reset and seeding complete.");
    }

    /// <summary>
    /// Drops all foreign keys and tables in the current database schema.
    /// </summary>
    private async Task DropAllTablesAsync(CancellationToken ct)
    {
        // Disable all FKs, then drop them, then drop tables.
        // Build and execute dynamic SQL that is safe for current DB.
        const string dropFksSql = @"
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name)
    + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';' + CHAR(13)
FROM sys.foreign_keys AS fk
JOIN sys.tables t ON fk.parent_object_id = t.object_id
JOIN sys.schemas s ON t.schema_id = s.schema_id;
EXEC sp_executesql @sql;";

        const string dropTablesSql = @"
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql = @sql + N'DROP TABLE ' + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name) + N';' + CHAR(13)
FROM sys.tables t
JOIN sys.schemas s ON t.schema_id = s.schema_id;
IF @sql <> N'' EXEC sp_executesql @sql;";

        await _db.Database.ExecuteSqlRawAsync(dropFksSql, ct);
        await _db.Database.ExecuteSqlRawAsync(dropTablesSql, ct);
    }
}
