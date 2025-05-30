using Microsoft.AspNetCore.Authorization;
using Microsoft.Teams.Assist.Shared.Authorization;

namespace Microsoft.Teams.Assist.Infrastructure.Auth.Permissions;
public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = SystemPermission.NameFor(action, resource);
}

public class RequireAnyActionAttribute : AuthorizeAttribute
{
    public RequireAnyActionAttribute(string[] actions, string resource)
    {
        var policies = actions.Select(action => SystemPermission.NameFor(action, resource));

        // Join the policies using OR logic
        Policy = string.Join(" OR ", policies);
    }
}

public class RequireAllActionsAttribute : AuthorizeAttribute
{
    public RequireAllActionsAttribute(string[] actions, string resource)
    {
        var policies = actions.Select(action => SystemPermission.NameFor(action, resource));

        // Join the policies using AND logic
        Policy = string.Join(" AND ", policies);
    }
}

public class RequireAnyResourceAttribute : AuthorizeAttribute
{
    public RequireAnyResourceAttribute(string action, string[] resources)
    {
        var policies = resources.Select(resource => SystemPermission.NameFor(action, resource));

        // Join the policies using OR logic
        Policy = string.Join(" OR ", policies);
    }
}

public class RequireAllResourcesAttribute : AuthorizeAttribute
{
    public RequireAllResourcesAttribute(string action, string[] resources)
    {
        var policies = resources.Select(resource => SystemPermission.NameFor(action, resource));

        // Join the policies using AND logic
        Policy = string.Join(" AND ", policies);
    }
}