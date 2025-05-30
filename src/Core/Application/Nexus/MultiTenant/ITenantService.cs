using Microsoft.Teams.Assist.Application.Nexus.MultiTenant.Models;
using Microsoft.Teams.Assist.Application.Nexus.MultiTenant.Models.Request;
using Microsoft.Teams.Assist.Application.Nexus.MultiTenant.Models.Response;

namespace Microsoft.Teams.Assist.Application.Nexus.MultiTenant;
public interface ITenantService : ITransientService
{
    Task<CreateTenantResponse> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken);

    Task<TenantsDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<TenantsDto> GetByUserIdAsync(string userId, CancellationToken cancellationToken);

}
