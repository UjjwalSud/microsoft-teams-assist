using System.ComponentModel.DataAnnotations;

namespace Microsoft.Teams.Assist.Application.Nexus.MultiTenant.Models.Request;
public class CreateTenantRequest
{
    public required Guid UniqueId { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string AdminEmail { get; set; }
    public string? ConnectionString { get; set; }

    [Required]
    public required bool IsActive { get; set; }

    [Required]
    public required int FKSubscriptionPKId { get; set; }
}
