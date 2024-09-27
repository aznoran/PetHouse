using Microsoft.EntityFrameworkCore;
using PetHouse.Domain.Shared.Other;

namespace PetHouse.Application.Extensions;

public static class QueriesExtensions
{
    public static async Task<PagedList<TSource>> ToPagedList<TSource>(this IQueryable<TSource> source,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await source.CountAsync(cancellationToken: cancellationToken);
        
        var result = await source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return new PagedList<TSource>
        {
            Items = result,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}

