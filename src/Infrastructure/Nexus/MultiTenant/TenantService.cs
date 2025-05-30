using Microsoft.EntityFrameworkCore;
using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Application.Nexus.MultiTenant;
using Microsoft.Teams.Assist.Application.Nexus.MultiTenant.Models;
using Microsoft.Teams.Assist.Application.Nexus.MultiTenant.Models.Request;
using Microsoft.Teams.Assist.Application.Nexus.MultiTenant.Models.Response;
using Microsoft.Teams.Assist.Application.Nexus.Subscription;
using Microsoft.Teams.Assist.Infrastructure.Nexus.MultiTenant.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;
using Microsoft.Teams.Assist.Infrastructure.SystemConstants;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.MultiTenant;
public class TenantService : ITenantService
{
    public readonly NexusDbContext _nexusDbContext;
    public readonly ISubscriptionService _subscriptionService;
    public TenantService(NexusDbContext nexusDbContext, ISubscriptionService subscriptionService)
    {
        _nexusDbContext = nexusDbContext;
        _subscriptionService = subscriptionService;
    }

    public async Task<CreateTenantResponse> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken)
    {
        var tenant = new Tenants
        {
            UniqueId = request.UniqueId,
            Name = request.Name,
            AdminEmail = request.AdminEmail,
            ConnectionString = request.ConnectionString,
            IsActive = request.IsActive,
            FKSubscriptionPKId = await _subscriptionService.GetRegisteredUserDefaultSubscriptionAsync(),
        };
        await _nexusDbContext.Tenants.AddAsync(tenant, cancellationToken);
        await _nexusDbContext.SaveChangesAsync(cancellationToken);
        return new CreateTenantResponse { TenantId = tenant.Id, UniqueId = tenant.UniqueId };
    }

    public async Task<TenantsDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var item = await _nexusDbContext.Tenants.SingleAsync(x => x.Id == id, cancellationToken);

        _ = item ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "Tenant"));

        return new TenantsDto
        {
            Id = item.Id,
            UniqueId = item.UniqueId,
            Name = item.Name,
            AdminEmail = item.AdminEmail,
            ConnectionString = item.ConnectionString,
            IsActive = item.IsActive,
            FKSubscriptionPKId = item.FKSubscriptionPKId
        };
    }

    public async Task<TenantsDto> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _nexusDbContext.Users.Include(x => x.Tenant).SingleAsync(x => x.Id == userId, cancellationToken);
        return new TenantsDto
        {
            Id = user.Tenant.Id,
            UniqueId = user.Tenant.UniqueId,
            Name = user.Tenant.Name,
            AdminEmail = user.Tenant.AdminEmail,
            ConnectionString = user.Tenant.ConnectionString,
            IsActive = user.Tenant.IsActive,
            FKSubscriptionPKId = user.Tenant.FKSubscriptionPKId
        };
    }
}
