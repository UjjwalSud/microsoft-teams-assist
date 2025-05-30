using Microsoft.Teams.Assist.Domain.Common.Contracts;
using Microsoft.Teams.Assist.Infrastructure.Nexus.Identity.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Nexus.Subscription.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.MultiTenant.DbModels;
public class Tenants : AuditableEntity
{
    [Required]
    public required Guid UniqueId { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string AdminEmail { get; set; }

    public string? ConnectionString { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [ForeignKey(nameof(Subscription))]
    [Required]
    public required int FKSubscriptionPKId { get; set; }

    public virtual Subscriptions Subscription { get; set; }

    public virtual List<ApplicationUser> ApplicationUsers { get; set; }
}