namespace backend.models;

public sealed record PagedResult<T>(int TotalCount, List<T> Items);
