using System.Security.Claims;

namespace Microsoft.Teams.Assist.Infrastructure.Auth;

public interface ICurrentUserInitializer
{
    void SetCurrentUser(ClaimsPrincipal user);

    void SetCurrentUserId(string userId);

    void SetCurrentTenant(int tenantId, Guid tenantUniqueId);
}
