namespace Microsoft.Teams.Assist.Application.Nexus.Identity.Roles.Models.Request;
public class CreateOrUpdateRoleRequest
{
    public string? Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
