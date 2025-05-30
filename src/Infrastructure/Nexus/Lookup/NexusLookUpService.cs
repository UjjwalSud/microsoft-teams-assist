using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Teams.Assist.Application.Common.Caching;
using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Application.Common.Models.Response;
using Microsoft.Teams.Assist.Application.Nexus.LookUp;
using Microsoft.Teams.Assist.Application.Nexus.LookUp.Models.Response;
using Microsoft.Teams.Assist.Domain.Enums.Nexus;
using Microsoft.Teams.Assist.Infrastructure.Nexus.LookUp.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;
using Microsoft.Teams.Assist.Infrastructure.SystemConstants;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.LookUp;
public class NexusLookUpService : INexusLookUpService
{
    private readonly NexusDbContext _nexusDbContext;
    private readonly ICacheService _cache;
    private readonly ICacheKeyService _cacheKeys;
    private const string CacheObjectId = "NexusLookUpCodeValues";
    public NexusLookUpService(NexusDbContext nexusDbContext, ICacheService cache, ICacheKeyService cacheKeys)
    {
        _nexusDbContext = nexusDbContext;
        _cache = cache;
        _cacheKeys = cacheKeys;
    }

    private async Task<List<NexusLookUpCodeValues>> GetNexusLookUpCodeValuesAsync()
    {
        return await _nexusDbContext.NexusLookUpCodeValues
            .Include(x => x.NexusLookUpCode)
            .ToListAsync();
    }

    private async Task<List<NexusLookUpCodeValues>> GetCachedNexusLookUpCodeValuesAsync()
    {
        return await _cache.GetOrSetAsync(_cacheKeys.GetCacheKey(CacheKeys.NexusLookUpValuesByCode, CacheObjectId, false), GetNexusLookUpCodeValuesAsync);
    }

    public async Task<List<NexusLookUpValueResponse>> GetNexusLookUpValuesByCodeAsync(NexusLookUpCodeTypes lookUpCodeType)
    {
        var result = await GetCachedNexusLookUpCodeValuesAsync();

        return result
            .Where(x => x.NexusLookUpCode.LookUpCodeType == lookUpCodeType)
            .Select(x => x.Adapt<NexusLookUpValueResponse>())
            .ToList();
    }

    public async Task<List<DropDownItemResponse>> GetNexusLookUpValuesByCodeForDropDownAsync(NexusLookUpCodeTypes type)
    {
        var result = await GetCachedNexusLookUpCodeValuesAsync();

        return result
            .Where(x => x.NexusLookUpCode.LookUpCodeType == type)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.LookUpValue)
            .Select(x => new DropDownItemResponse { Text = x.LookUpValue, Value = x.Id })
            .ToList();
    }

    public async Task<NexusLookUpValueResponse> GetNexusLookUpValueByIdAsync(int id)
    {
        var items = await GetCachedNexusLookUpCodeValuesAsync();

        var result = items.SingleOrDefault(x => x.Id == id)
                     ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "item"));

        return result.Adapt<NexusLookUpValueResponse>();
    }

}
