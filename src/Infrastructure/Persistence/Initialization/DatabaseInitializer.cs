using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Teams.Assist.Application.Common.Interfaces;
using Microsoft.Teams.Assist.Application.Nexus.Subscription;
using Microsoft.Teams.Assist.Domain.Enums;
using Microsoft.Teams.Assist.Domain.Enums.Nexus;
using Microsoft.Teams.Assist.Application.Common.Extensions;
using Microsoft.Teams.Assist.Infrastructure.Nexus.MultiTenant.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Auditing;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;
using Microsoft.Teams.Assist.Shared.Nexus;
namespace Microsoft.Teams.Assist.Infrastructure.Persistence.Initialization;
internal class DatabaseInitializer : IDatabaseInitializer
{
    private readonly NexusDbContext _nexusDbContext;
    private readonly AuditingDbContext _auditingDbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializer> _logger;
    private readonly ISubscriptionService _subscriptionService;
    private readonly ISerializerService _serializer;
    public DatabaseInitializer(NexusDbContext nexusDbContext, AuditingDbContext auditingDbContext, IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger, ISubscriptionService subscriptionService, ISerializerService serializer)
    {
        _nexusDbContext = nexusDbContext;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _subscriptionService = subscriptionService;
        _serializer = serializer;
        _auditingDbContext = auditingDbContext;
    }

    public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
    {
        await InitializeAuditTrailDbAsync(cancellationToken);
        await InitializeNexusDbAsync(cancellationToken);

        foreach (var tenant in await _nexusDbContext.Tenants.ToListAsync(cancellationToken))
        {
            await InitializeApplicationDbForTenantAsync(tenant, cancellationToken);
        }
    }

    public async Task InitializeApplicationDbForTenantAsync(Tenants tenant, CancellationToken cancellationToken)
    {
        // First create a new scope
        using var scope = _serviceProvider.CreateScope();

        // Then run the initialization in the new scope
        await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
            .InitializeAsync(tenant, cancellationToken);
    }

    private async Task InitializeAuditTrailDbAsync(CancellationToken cancellationToken)
    {
        if (_auditingDbContext.Database.GetPendingMigrations().Any())
        {
            _logger.LogInformation("Applying Nexus Migrations.");
            await _auditingDbContext.Database.MigrateAsync(cancellationToken);
        }
    }

    private async Task InitializeNexusDbAsync(CancellationToken cancellationToken)
    {
        if (_nexusDbContext.Database.GetPendingMigrations().Any())
        {
            _logger.LogInformation("Applying Nexus Migrations.");
            await _nexusDbContext.Database.MigrateAsync(cancellationToken);
        }

        await SeedSubscriptions(cancellationToken);
        await SeedRootTenantAsync(cancellationToken);
        await SeedNexusLooks(cancellationToken);
    }

    private async Task SeedSubscriptions(CancellationToken cancellationToken)
    {
        foreach (SubscriptionTypes subscriptionType in Enum.GetValues(typeof(SubscriptionTypes)))
        {
            if (!await _nexusDbContext.Subscriptions.AnyAsync(x => x.SubscriptionType == subscriptionType))
            {
                _logger.LogInformation($"{subscriptionType.GetDescription()} added");
                string setting = _serializer.Serialize(_subscriptionService.GetSubscriptionDefaultSetting(subscriptionType));

                await _nexusDbContext.Subscriptions.AddAsync(new Nexus.Subscription.DbModels.Subscriptions
                {
                    Name = subscriptionType.GetDescription(),
                    SubscriptionType = subscriptionType,
                    Description = subscriptionType.GetDescription(),
                    Setting = setting,
                });
            }

            await _nexusDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task SeedRootTenantAsync(CancellationToken cancellationToken)
    {
        if (!await _nexusDbContext.Tenants.AnyAsync(x => x.Name == NexusConstants.Root.TenantName))
        {
            _logger.LogInformation("Root Tenant added");
            await _nexusDbContext.Tenants.AddAsync(new Tenants
            {
                UniqueId = NexusConstants.Root.TenantUniqueId,
                Name = NexusConstants.Root.TenantName,
                IsActive = true,
                FKSubscriptionPKId = (await _nexusDbContext.Subscriptions.SingleAsync(x => x.SubscriptionType == _subscriptionService.GetRootAdminSubscription())).Id,
                AdminEmail = NexusConstants.Root.TenantEmailAddress
            });

            await _nexusDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task SeedNexusLooks(CancellationToken cancellationToken)
    {
        foreach (NexusLookUpCodeTypes type in Enum.GetValues(typeof(NexusLookUpCodeTypes)))
        {
            if (!await _nexusDbContext.NexusLookUpCodes.AnyAsync(x => x.LookUpCodeType == type))
            {
                _logger.LogInformation($"{type.GetDescription()} added");
                await _nexusDbContext.NexusLookUpCodes.AddAsync(new Nexus.LookUp.DbModels.NexusLookUpCodes
                {
                    LookUpCodeType = type,
                    Description = type.GetDescription()
                });
            }
        }

        await _nexusDbContext.SaveChangesAsync(cancellationToken);

        #region Category Default Values
        var categoryData = await _nexusDbContext.NexusLookUpCodes.SingleAsync(x => x.LookUpCodeType == NexusLookUpCodeTypes.Category);
        var categories = new List<string>() { "Cat 1 Cat 2", "Cat 3" };

        var existingValues = _nexusDbContext.NexusLookUpCodeValues.Where(x => x.NexusLookUpCode.LookUpCodeType == NexusLookUpCodeTypes.Category).Select(x => x.LookUpValue).ToList();
        var entriesToAdd = categories.Except(existingValues).ToList();
        foreach (string entry in entriesToAdd)
        {
            _nexusDbContext.NexusLookUpCodeValues.Add(new Nexus.LookUp.DbModels.NexusLookUpCodeValues
            {
                IsActive = true,
                LookUpValue = entry,
                FKNexusLookUpCodePKId = categoryData.Id,
                DisplayOrder = 0
            });
        }

        await _nexusDbContext.SaveChangesAsync(cancellationToken);
        #endregion
    }
}