namespace Microsoft.Teams.Assist.Application.Common.Caching;
public interface ICacheKeyService : IScopedService
{
    public string GetCacheKey(CacheKeys name, object id, bool includeTenantId = true);
}
