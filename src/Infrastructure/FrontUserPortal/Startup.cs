using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Teams.Assist.Infrastructure.FrontUserPortal;
internal static class Startup
{
    internal static IServiceCollection AddFrontUserPortal(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<FrontUserPortalSettings>(config.GetSection(nameof(FrontUserPortalSettings)));

        return services;
    }
}
