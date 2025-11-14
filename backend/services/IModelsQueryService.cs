using backend.database;
using backend.models;
using backend.database; // for extensions namespace usage in implementation file
using Microsoft.EntityFrameworkCore;

namespace backend.services;

public interface IModelsQueryService
{
    Task<PagedResult<ModelItem>> BrowseAsync(BrowseQuery q, CancellationToken ct = default);
}

public sealed class BrowseQuery
{
    public string? Category { get; init; }
    public string? Q { get; init; }
    public int Skip { get; init; } = 0;
    public int Take { get; init; } = 20;
    public string Sort { get; init; } = "likes"; // likes|date|name
    public string Order { get; init; } = "desc"; // asc|desc
}

public sealed class ModelsQueryService : IModelsQueryService
{
    private readonly PrintforgeDbContext _db;
    public ModelsQueryService(PrintforgeDbContext db) => _db = db;

    public async Task<PagedResult<ModelItem>> BrowseAsync(BrowseQuery q, CancellationToken ct = default)
    {
        // Normalize & clamp
        var take = Math.Clamp(q.Take, 0, 100);
        var skip = Math.Max(0, q.Skip);
        var sort = (q.Sort ?? "likes").Trim().ToLowerInvariant();
        var order = (q.Order ?? "desc").Trim().ToLowerInvariant();
        var isDesc = order == "desc";
        if (sort is not ("likes" or "date" or "name")) sort = "likes";
        if (order is not ("asc" or "desc")) isDesc = true;

        var queryable = _db.Models.AsNoTracking()
            .ApplyFilters(q.Category, q.Q);

        var total = await queryable.CountAsync(ct);

        var items = await queryable
            .ApplySorting(sort, isDesc)
            .Skip(skip)
            .Take(take)
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
            .ToListAsync(ct);

        return new PagedResult<ModelItem>(total, items);
    }
}
