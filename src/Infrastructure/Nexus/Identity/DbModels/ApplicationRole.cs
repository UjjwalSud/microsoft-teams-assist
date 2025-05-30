using Microsoft.AspNetCore.Identity;
using Microsoft.Teams.Assist.Infrastructure.Nexus.MultiTenant.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Identity.DbModels;

public class ApplicationRole : IdentityRole
{
    [Required]
    public required string UserFriendlyRoleName { get; set; }

    public string RoleNameWithoutTenantId
    {
        get
        {
            return Name.Split('_')[1];
        }
    }
    public string? Description { get; set; }

    [Required]
    [ForeignKey(nameof(Tenant))]
    public required int FKTenantPKId { get; set; }

    public virtual Tenants Tenant { get; set; }
}