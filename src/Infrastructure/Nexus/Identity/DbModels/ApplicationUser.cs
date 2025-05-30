using Microsoft.AspNetCore.Identity;
using Microsoft.Teams.Assist.Infrastructure.Nexus.MultiTenant.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Identity.DbModels;

public class ApplicationUser : IdentityUser
{
    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }
    public string? ImageUrl { get; set; }

    [Required]
    public bool IsActive { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string? ObjectId { get; set; }

    [Required]
    [ForeignKey(nameof(Tenant))]
    public int FKTenantId { get; set; }
    public virtual Tenants Tenant { get; set; }
}
