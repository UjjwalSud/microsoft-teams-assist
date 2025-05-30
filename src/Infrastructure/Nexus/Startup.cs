using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Teams.Assist.Infrastructure.Persistence;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus;
internal static class Startup
{
    internal static IServiceCollection AddNexus(this IServiceCollection services)
    {
        return services
            .AddDbContext<NexusDbContext>((p, m) =>
            {
                var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                m.UseDatabase(databaseSettings.Nexus_DBProvider, databaseSettings.Nexus_ConnectionString);
            })

            //.AddMultiTenant<FSHTenantInfo>()
                //.WithClaimStrategy(WorkPowerClaims.Tenant)
                //.WithHeaderStrategy(MultitenancyConstants.TenantIdName)
                //.WithQueryStringStrategy(MultitenancyConstants.TenantIdName)
                //.WithEFCoreStore<TenantDbContext, FSHTenantInfo>()
                //.Services
            //.AddScoped<ITenantService, TenantService>()
            ;
    }
}
