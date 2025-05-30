using Microsoft.Teams.Assist.Application.Common.Interfaces;
using System.Security.Claims;

namespace Microsoft.Teams.Assist.Infrastructure.Auth;

public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    private ClaimsPrincipal? _user;

    public string? Name => _user?.Identity?.Name;

    private Guid _userId = Guid.Empty;
    private Guid _tenantUniqueId = Guid.Empty;
    private int _tenantId = -1;

    public Guid GetUserId() =>
    IsAuthenticated()
        ? Guid.Parse(_user?.GetUserId() ?? Guid.Empty.ToString())
        : _userId;

    public string? GetUserEmail() =>
        IsAuthenticated()
            ? _user!.GetEmail()
            : string.Empty;

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated is true;

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) is true;

    public IEnumerable<Claim>? GetUserClaims() =>
        _user?.Claims;

    public int GetTenant() =>
   IsAuthenticated()
       ? int.Parse(_user?.GetTenant() ?? "-1")
       : _tenantId;

    public Guid GetTenantUniqueId() =>
 IsAuthenticated()
     ? Guid.Parse(_user?.GetTenantUniqueId() ?? Guid.Empty.ToString())
     : _tenantUniqueId;

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(string userId)
    {
        if (_userId != Guid.Empty)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(userId))
        {
            _userId = Guid.Parse(userId);
        }
    }

    public void SetCurrentTenant(int tenantId, Guid tenantUniqueId)
    {
        if (_tenantId != -1)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _tenantUniqueId = tenantUniqueId;
        _tenantId = tenantId;
    }
}
