using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Teams.Assist.Application.Common.Models;

namespace Microsoft.Teams.Assist.Infrastructure.Persistence.Context;
public static class QueryableExtensions
{
    public static async Task<PaginationResponse<TDestination>> PaginatedListAsync<T, TDestination>(
        this IQueryable<T> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();

        // Ensure valid page size and page number
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        // Fetch paginated data
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Map the entities to the destination type
        var mappedItems = items.Adapt<List<TDestination>>();

        return new PaginationResponse<TDestination>(mappedItems, totalCount, pageNumber, pageSize);
    }
}
