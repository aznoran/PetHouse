﻿namespace PetHouse.Domain.Shared.Other;

public class PagedList<T>
{
    public IReadOnlyList<T> Items { get; init; }
    
    public int TotalCount { get; init; }
    
    public int PageSize { get; init; }
    
    public int Page { get; init; }

    public bool HasNextPage => Page * PageSize < TotalCount;
    
    public bool HasPreviousPage => Page > 1;
}