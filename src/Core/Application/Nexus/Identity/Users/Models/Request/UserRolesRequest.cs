using Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Response;

namespace Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Request;
public class UserRolesRequest
{
    public List<UserRoleResponse> UserRoles { get; set; } = new();
}
