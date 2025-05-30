using Microsoft.Teams.Assist.Application.Common.Caching;
using Microsoft.Teams.Assist.Application.Common.Interfaces;

namespace Microsoft.Teams.Assist.Infrastructure.Caching;
public class CacheKeyService : ICacheKeyService
{
    private readonly ICurrentUser _currentUser;
    public CacheKeyService(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string GetCacheKey(CacheKeys name, object id, bool includeTenantId = true)
    {
        string tenantUniqueId = _currentUser.GetTenantUniqueId().ToString();
        if (string.IsNullOrEmpty(tenantUniqueId) && includeTenantId)
        {
            throw new InvalidOperationException("GetCacheKey: includeTenantId set to true and no ITenantInfo available.");
        }
        else if (!includeTenantId)
        {
            tenantUniqueId = "GLOBAL";
        }

        return $"{tenantUniqueId}-{name.ToString()}-{id}";
    }
}