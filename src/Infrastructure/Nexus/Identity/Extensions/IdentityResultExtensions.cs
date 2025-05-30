using Microsoft.AspNetCore.Identity;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Identity.Extensions;
internal static class IdentityResultExtensions
{
    public static List<string> GetErrors(this IdentityResult result) =>
        result.Errors.Select(e => e.Description.ToString()).ToList();
}