using Microsoft.AspNetCore.Identity;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Identity.DbModels;
public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public string? CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
}
