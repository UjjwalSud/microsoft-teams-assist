using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Teams.Assist.Infrastructure.Nexus.MultiTenant.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;

namespace Microsoft.Teams.Assist.Infrastructure.Persistence.Initialization;
internal class ApplicationDbInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly NexusDbContext _nexusDbContext;
    private readonly ApplicationDbSeeder _dbSeeder;
    private readonly ILogger<ApplicationDbInitializer> _logger;

    public ApplicationDbInitializer(ApplicationDbContext dbContext, NexusDbContext nexusDbContext, ApplicationDbSeeder dbSeeder, ILogger<ApplicationDbInitializer> logger)
    {
        _dbContext = dbContext;
        _dbSeeder = dbSeeder;
        _logger = logger;
        _nexusDbContext = nexusDbContext;
    }

    public async Task InitializeAsync(Tenants currentTenant, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(currentTenant.ConnectionString))
        {
            _dbContext.Database.SetConnectionString(currentTenant.ConnectionString);
        }

        if (_dbContext.Database.GetMigrations().Any())
        {
            if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                _logger.LogInformation("Applying Migrations for '{tenantId}' tenant.", currentTenant.Name);
                await _dbContext.Database.MigrateAsync(cancellationToken);
            }

            if (await _dbContext.Database.CanConnectAsync(cancellationToken))
            {
                _logger.LogInformation("Connection to {tenantId}'s Database Succeeded.", currentTenant.Name);

                await _dbSeeder.SeedDatabaseAsync(_dbContext, _nexusDbContext, currentTenant, cancellationToken);
            }
        }
    }
}
