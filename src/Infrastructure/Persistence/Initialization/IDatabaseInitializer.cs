using Microsoft.Teams.Assist.Infrastructure.Nexus.MultiTenant.DbModels;

namespace Microsoft.Teams.Assist.Infrastructure.Persistence.Initialization;
internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(Tenants tenant, CancellationToken cancellationToken);
}