using Microsoft.Teams.Assist.Domain.Common.Contracts;
using Microsoft.Teams.Assist.Domain.Enums;
using Microsoft.Teams.Assist.Infrastructure.Nexus.MultiTenant.DbModels;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Subscription.DbModels;
public class Subscriptions : AuditableEntity
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required SubscriptionTypes SubscriptionType { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    public required string Setting { get; set; }

    public virtual List<Tenants> Tenants { get; set; }
}