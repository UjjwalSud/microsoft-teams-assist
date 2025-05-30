using System.Security.Claims;

namespace Microsoft.Teams.Assist.Application.Common.Interfaces;

public interface ICurrentUser
{
    string? Name { get; }

    Guid GetUserId();

    string? GetUserEmail();

    int GetTenant();

    Guid GetTenantUniqueId();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();
}
