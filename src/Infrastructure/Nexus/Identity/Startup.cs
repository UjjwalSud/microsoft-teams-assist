using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Teams.Assist.Infrastructure.Nexus.Identity.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Identity;

internal static class Startup
{
    internal static IServiceCollection AddIdentity(this IServiceCollection services) =>
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<NexusDbContext>()
            .AddDefaultTokenProviders()
            .Services;
}
