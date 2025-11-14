using System.Linq;
using backend.database;
using Microsoft.EntityFrameworkCore;

namespace backend.database;

public static class ModelsQueryExtensions
{
    public static IQueryable<ModelRecord> ApplyFilters(this IQueryable<ModelRecord> q, string? category, string? query)
    {
        if (!string.IsNullOrWhiteSpace(category))
        {
            var c = category.Trim();
            q = q.Where(m => m.Category != null && m.Category == c);
        }

        if (!string.IsNullOrWhiteSpace(query))
        {
            var t = query.Trim();
            q = q.Where(m =>
                (m.Name != null && EF.Functions.Like(m.Name, $"%{t}%")) ||
                (m.Description != null && EF.Functions.Like(m.Description, $"%{t}%"))
            );
        }

        return q;
    }

    public static IQueryable<ModelRecord> ApplySorting(this IQueryable<ModelRecord> q, string sort, bool desc)
        => sort switch
        {
            "likes" => desc ? q.OrderByDescending(m => m.Likes) : q.OrderBy(m => m.Likes),
            "date"  => desc ? q.OrderByDescending(m => m.DateAdded) : q.OrderBy(m => m.DateAdded),
            "name"  => desc ? q.OrderByDescending(m => m.Name) : q.OrderBy(m => m.Name),
            _        => desc ? q.OrderByDescending(m => m.Likes) : q.OrderBy(m => m.Likes)
        };
}
