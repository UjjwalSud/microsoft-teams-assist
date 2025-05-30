using Microsoft.EntityFrameworkCore;
using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Request;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Response;
using Microsoft.Teams.Assist.Infrastructure.Nexus.Identity.DbModels;
using Microsoft.Teams.Assist.Infrastructure.SystemConstants;
using Microsoft.Teams.Assist.Shared.Authorization;
using Microsoft.Teams.Assist.Shared.Nexus;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Identity;
internal partial class UserService
{
    public async Task AssignDefaultRoleToNewTenantAsync(int tenantId, Guid uniqueId, CancellationToken cancellationToken)
    {
        foreach (string roleName in SystemRoles.DefaultRoles)
        {
            string tenantRoleName = SystemRoles.FormatTenantRoleName(roleName, tenantId);

            if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == tenantRoleName && r.FKTenantPKId == tenantId)
                is not ApplicationRole role)
            {
                role = new ApplicationRole
                {
                    UserFriendlyRoleName = roleName,
                    Name = tenantRoleName,
                    FKTenantPKId = tenantId,
                    Description = $"{roleName} Role"
                };
                await _roleManager.CreateAsync(role);
            }

            // Assign permissions
            if (roleName == SystemRoles.Basic)
            {
                await AssignPermissionsToTenantRoleAsync(SystemPermissions.Basic, role, cancellationToken);
            }
            else if (roleName == SystemRoles.Admin)
            {
                await AssignPermissionsToTenantRoleAsync(SystemPermissions.Admin, role, cancellationToken);

                if (uniqueId == NexusConstants.Root.TenantUniqueId)
                {
                    await AssignPermissionsToTenantRoleAsync(SystemPermissions.Root, role, cancellationToken);
                }
            }
        }
    }

    private async Task AssignPermissionsToTenantRoleAsync(IReadOnlyList<SystemPermission> permissions, ApplicationRole role, CancellationToken cancellationToken)
    {
        var currentClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (!currentClaims.Any(c => c.Type == SystemClaims.Permission && c.Value == permission.Name))
            {
                _nexusDbContext.RoleClaims.Add(new ApplicationRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = SystemClaims.Permission,
                    ClaimValue = permission.Name,
                });
                await _nexusDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public async Task<List<UserRoleResponse>> GetRolesAsync(string userId, CancellationToken cancellationToken)
    {
        var userRoles = new List<UserRoleResponse>();

        var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId && x.FKTenantId == _currentUser.GetTenant());
        if (user is null) throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "User"));
        var roles = await _roleManager.Roles.Where(x => x.FKTenantPKId == user.FKTenantId).AsNoTracking().ToListAsync(cancellationToken);
        if (roles is null) throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "Roles"));
        foreach (var role in roles)
        {
            userRoles.Add(new UserRoleResponse
            {
                RoleId = role.Id,
                RoleName = role.UserFriendlyRoleName,
                Description = role.Description,
                Enabled = await _userManager.IsInRoleAsync(user, role.Name!)
            });
        }

        return userRoles;
    }

    public async Task<string> AssignRolesAsync(string userId, UserRolesRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Where(u => u.Id == userId && u.FKTenantId == _currentUser.GetTenant()).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "User"));

        // Check if the user is an admin for which the admin role is getting disabled
        var tenantDetails = await _tenantService.GetByIdAsync(_currentUser.GetTenant(), cancellationToken);
        if (await _userManager.IsInRoleAsync(user, SystemRoles.FormatTenantRoleName(SystemRoles.Admin, tenantDetails.Id))
            //&& request.UserRoles.Any(a => !a.Enabled && a.RoleName == WorkPowerRoles.Admin)
            )
        {
            // Get count of users in Admin Role
            int adminCount = (await _userManager.GetUsersInRoleAsync(SystemRoles.FormatTenantRoleName(SystemRoles.Admin, tenantDetails.Id))).Count;

            // Check if user is not Root Tenant Admin
            if (user.Id == NexusConstants.Root.UserId)
            {
                if (tenantDetails.UniqueId == NexusConstants.Root.TenantUniqueId)
                {
                    throw new ConflictException(ErrorMessages.RootTenantRoleCannotBeRemoved);
                }
            }
            else if (adminCount <= 2)
            {
                throw new ConflictException(ErrorMessages.MinimumAdminMessage);
            }
        }

        foreach (var userRole in request.UserRoles)
        {
            // Check if Role Exists
            string roleName = SystemRoles.FormatTenantRoleName(userRole.RoleName!, tenantDetails.Id);
            if (await _roleManager.FindByNameAsync(roleName) is not null)
            {
                if (userRole.Enabled)
                {
                    if (!await _userManager.IsInRoleAsync(user, roleName))
                    {
                        await _userManager.AddToRoleAsync(user, roleName);
                    }
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
        }

        return SuccessMessages.UserRoleAssigned;
    }
}
