namespace Microsoft.Teams.Assist.Application.Nexus.Identity.Roles.Models.Request;
public class UpdateRolePermissionsRequest
{
    public string RoleId { get; set; } = default!;
    public List<string> Permissions { get; set; } = default!;
}