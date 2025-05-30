namespace Microsoft.Teams.Assist.Application.Nexus.MultiTenant.Models;
public class TenantsDto
{
    public int Id { get; set; }
    public required Guid UniqueId { get; set; }

    public required string Name { get; set; }

    public required string AdminEmail { get; set; }

    public string? ConnectionString { get; set; }

    public bool IsActive { get; set; }

    public required int FKSubscriptionPKId { get; set; }
}
