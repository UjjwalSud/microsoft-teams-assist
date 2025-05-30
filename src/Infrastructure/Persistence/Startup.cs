using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Microsoft.Teams.Assist.Infrastructure.Common;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Auditing;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Initialization;

namespace Microsoft.Teams.Assist.Infrastructure.Persistence;

internal static class Startup
{
    private static readonly ILogger _logger = Log.ForContext(typeof(Startup));

    internal static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddOptions<DatabaseSettings>()
            .BindConfiguration(nameof(DatabaseSettings))
            .PostConfigure(databaseSettings =>
            {
                _logger.Information("Current DB Provider For Nexus : {dbProvider}", databaseSettings.Nexus_DBProvider);
                _logger.Information("Current DB Provider For AuditTrail: {dbProvider}", databaseSettings.AuditTrail_DBProvider);
                _logger.Information("Current DB Provider For Application: {dbProvider}", databaseSettings.Application_DBProvider);
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services
            .AddDbContext<NexusDbContext>((p, m) =>
            {
                var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                m.UseDatabase(databaseSettings.Nexus_DBProvider, databaseSettings.Nexus_ConnectionString);
            })
            .AddDbContext<AuditingDbContext>((p, m) =>
            {
                var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                m.UseDatabase(databaseSettings.AuditTrail_DBProvider, databaseSettings.AuditTrail_ConnectionString);
            })
            .AddDbContext<ApplicationDbContext>((p, m) =>
            {
                var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                m.UseDatabase(databaseSettings.Application_DBProvider, databaseSettings.Application_ConnectionString);
            })
             .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
            .AddTransient<ApplicationDbInitializer>()
            .AddTransient<ApplicationDbSeeder>()
            //.AddServices(typeof(ICustomSeeder), ServiceLifetime.Transient)
            //.AddTransient<CustomSeederRunner>()

            //.AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>()
            //.AddTransient<IConnectionStringValidator, ConnectionStringValidator>()

            //.AddRepositories()
            ;
    }

    internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
    {
        return dbProvider.ToLowerInvariant() switch
        {
            DbProviderKeys.Npgsql => builder.UseNpgsql(connectionString, e =>
                                 e.MigrationsAssembly("Migrators.PostgreSQL")),
            DbProviderKeys.SqlServer => builder.UseSqlServer(connectionString, e =>
                                 e.MigrationsAssembly("Migrators.MSSQL")),
            //DbProviderKeys.MySql => builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), e =>
            //                     e.MigrationsAssembly("Migrators.MySQL")
            //                      .SchemaBehavior(MySqlSchemaBehavior.Ignore)),
            //DbProviderKeys.Oracle => builder.UseOracle(connectionString, e =>
            //                     e.MigrationsAssembly("Migrators.Oracle")),
            //DbProviderKeys.SqLite => builder.UseSqlite(connectionString, e =>
            //                     e.MigrationsAssembly("Migrators.SqLite")),
            _ => throw new InvalidOperationException($"DB Provider {dbProvider} is not supported."),
        };
    }

    /*
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Add Repositories
        services.AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>));

        foreach (var aggregateRootType in
            typeof(IAggregateRoot).Assembly.GetExportedTypes()
                .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
                .ToList())
        {
            // Add ReadRepositories.
            services.AddScoped(typeof(IReadRepository<>).MakeGenericType(aggregateRootType), sp =>
                sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)));

            // Decorate the repositories with EventAddingRepositoryDecorators and expose them as IRepositoryWithEvents.
            services.AddScoped(typeof(IRepositoryWithEvents<>).MakeGenericType(aggregateRootType), sp =>
                Activator.CreateInstance(
                    typeof(EventAddingRepositoryDecorator<>).MakeGenericType(aggregateRootType),
                    sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)))
                ?? throw new InvalidOperationException($"Couldn't create EventAddingRepositoryDecorator for aggregateRootType {aggregateRootType.Name}"));
        }

        return services;
    }
    */
}
