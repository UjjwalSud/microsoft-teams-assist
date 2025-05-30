using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Teams.Assist.Infrastructure.Auth;
using Microsoft.Teams.Assist.Infrastructure.BackgroundJobs;
using Microsoft.Teams.Assist.Infrastructure.Caching;
using Microsoft.Teams.Assist.Infrastructure.Common;
using Microsoft.Teams.Assist.Infrastructure.Cors;
using Microsoft.Teams.Assist.Infrastructure.FileStorage;
using Microsoft.Teams.Assist.Infrastructure.FrontUserPortal;
using Microsoft.Teams.Assist.Infrastructure.Mailing;
using Microsoft.Teams.Assist.Infrastructure.Middleware;
using Microsoft.Teams.Assist.Infrastructure.Nexus;
using Microsoft.Teams.Assist.Infrastructure.OpenApi;
using Microsoft.Teams.Assist.Infrastructure.Persistence;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Initialization;
using Microsoft.Teams.Assist.Infrastructure.SecurityHeaders;

namespace Microsoft.Teams.Assist.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        //var applicationAssembly = typeof(WorkPower.WebApi.Application.Startup).GetTypeInfo().Assembly;
        //MapsterSettings.Configure();
        return services
            .AddApiVersioning()
            .AddAuth(config)
            .AddBackgroundJobs(config)
            .AddCaching(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            //.AddBehaviours(applicationAssembly)
            //.AddHealthCheck()
            //.AddPOLocalization(config)
            .AddMailing(config)
            //.AddMediatR(Assembly.GetExecutingAssembly())
            /////////.AddMultitenancy() -- not required
            .AddNexus()
            //.AddNotifications(config)
            .AddOpenApiDocumentation(config)
            .AddPersistence()
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices()
            .AddFrontUserPortal(config)
            //.AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>))
            ; 
        ;
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

    //private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
    //    services.AddHealthChecks().AddCheck<TenantHealthCheck>("Tenant").Services;

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
        builder
            //.UseRequestLocalization()
            .UseStaticFiles()
            .UseSecurityHeaders(config)
            .UseFileStorage()
            .UseExceptionMiddleware()
            .UseRouting()
            .UseCorsPolicy()
            .UseAuthentication()
            .UseCurrentUser()
            /////////.UseMultiTenancy()-- not required
            .UseAuthorization()
            .UseRequestLogging(config)
            .UseHangfireDashboard(config)
            .UseOpenApiDocumentation(config)
        ;

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        //builder.MapHealthCheck();
        //builder.MapNotifications();
        return builder;
    }

    //private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
    //    endpoints.MapHealthChecks("/api/health");
}
