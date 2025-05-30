using Microsoft.AspNetCore.Authorization;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users;
using System.Security.Claims;

namespace Microsoft.Teams.Assist.Infrastructure.Auth.Permissions;
internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserService _userService;

    public PermissionAuthorizationHandler(IUserService userService) =>
        _userService = userService;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User?.GetUserId() is { } userId &&
            await _userService.HasPermissionAsync(userId, requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}