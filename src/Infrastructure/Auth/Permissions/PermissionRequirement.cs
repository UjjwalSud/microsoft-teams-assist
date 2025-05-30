using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Teams.Assist.Infrastructure.Auth.Permissions;
internal class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; private set; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}