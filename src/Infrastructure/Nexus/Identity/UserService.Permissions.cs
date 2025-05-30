using Microsoft.EntityFrameworkCore;
using Microsoft.Teams.Assist.Application.Common.Caching;
using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Infrastructure.Common;
using Microsoft.Teams.Assist.Infrastructure.SystemConstants;
using Microsoft.Teams.Assist.Shared.Authorization;
namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Identity;
internal partial class UserService
{
    public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new BadRequestException(ErrorMessages.AuthenticationFailed);

        var userRoles = await _userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        foreach (var role in await _roleManager.Roles
            .Where(r => userRoles.Contains(r.Name!))
            .ToListAsync(cancellationToken))
        {
            permissions.AddRange(await _nexusDbContext.RoleClaims
                .Where(rc => rc.RoleId == role.Id && rc.ClaimType == SystemClaims.Permission)
                .Select(rc => rc.ClaimValue!)
                .ToListAsync(cancellationToken));
        }

        return permissions.Distinct().ToList();
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken)
    {
        var permissions = await _cache.GetOrSetAsync(
            _cacheKeys.GetCacheKey(CacheKeys.Permission, userId),
            () => GetPermissionsAsync(userId, cancellationToken),
            cancellationToken: cancellationToken);

        if (permission.Contains(" OR "))
        {
            var reqPermissions = permission.Split(" OR ");
            return reqPermissions.Any(req => permissions?.Contains(req.Trim()) == true);
        }
        if (permission.Contains(" AND "))
        {
            var reqPermissions = permission.Split(" AND ");
            return reqPermissions.All(req => permissions?.Contains(req.Trim()) == true);
        }

        return permissions?.Contains(permission) ?? false;
    }
}
