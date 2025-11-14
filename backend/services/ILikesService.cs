using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.services;

public interface ILikesService
{
    /// <summary>
    /// Atomically increments the Likes counter for the given model.
    /// Returns false if the model wasn't found.
    /// </summary>
    Task<bool> IncrementAsync(int modelId, CancellationToken ct = default);
}

public sealed class LikesService : ILikesService
{
    private readonly PrintforgeDbContext _db;

    public LikesService(PrintforgeDbContext db)
    {
        _db = db;
    }

    public async Task<bool> IncrementAsync(int modelId, CancellationToken ct = default)
    {
        // Atomic server-side increment to avoid race conditions
        var affected = await _db.Database.ExecuteSqlInterpolatedAsync($"UPDATE [Models] SET [Likes] = [Likes] + 1 WHERE [Id] = {modelId}", ct);
        return affected > 0;
    }
}

public interface ILikeRateLimiter
{
    /// <summary>
    /// Enforces: at most 1 request per 100ms and max 30 per minute for a client.
    /// Returns true if allowed; otherwise returns false and the suggested retryAfter.
    /// </summary>
    bool TryAcquire(HttpContext ctx, out TimeSpan retryAfter);
}

/// <summary>
/// Simple in-memory per-client rate limiter (per remote IP).
/// Not distributed; suitable for a single instance / development.
/// </summary>
public sealed class LikeRateLimiter : ILikeRateLimiter
{
    private sealed class State
    {
        public DateTime Last;
        public readonly Queue<DateTime> MinuteWindow = new();
        public readonly object Gate = new();
    }

    private readonly ConcurrentDictionary<string, State> _states = new();
    private static readonly TimeSpan MinSpacing = TimeSpan.FromMilliseconds(100);
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(1);
    private const int MaxPerWindow = 30;

    public bool TryAcquire(HttpContext ctx, out TimeSpan retryAfter)
    {
        retryAfter = TimeSpan.Zero;
        var key = GetClientKey(ctx);
        var state = _states.GetOrAdd(key, _ => new State { Last = DateTime.MinValue });

        lock (state.Gate)
        {
            var now = DateTime.UtcNow;

            // Enforce 1 every 100ms
            var sinceLast = now - state.Last;
            if (sinceLast < MinSpacing)
            {
                retryAfter = MinSpacing - sinceLast;
                return false;
            }

            // Enforce 30 per minute (sliding window)
            while (state.MinuteWindow.Count > 0 && (now - state.MinuteWindow.Peek()) > Window)
                state.MinuteWindow.Dequeue();

            if (state.MinuteWindow.Count >= MaxPerWindow)
            {
                var oldest = state.MinuteWindow.Peek();
                var until = (oldest + Window) - now;
                retryAfter = until <= TimeSpan.Zero ? TimeSpan.FromMilliseconds(50) : until;
                return false;
            }

            // Allow and record
            state.Last = now;
            state.MinuteWindow.Enqueue(now);
            return true;
        }
    }

    private static string GetClientKey(HttpContext ctx)
    {
        // Use RemoteIp as a simple key. In production behind proxies, honor X-Forwarded-For, etc.
        var ip = ctx.Connection.RemoteIpAddress?.ToString();
        return string.IsNullOrEmpty(ip) ? "unknown" : ip;
    }
}
