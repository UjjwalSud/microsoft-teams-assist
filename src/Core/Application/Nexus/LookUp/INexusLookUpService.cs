using Microsoft.Teams.Assist.Application.Common.Models.Response;
using Microsoft.Teams.Assist.Application.Nexus.LookUp.Models.Response;
using Microsoft.Teams.Assist.Domain.Enums.Nexus;

namespace Microsoft.Teams.Assist.Application.Nexus.LookUp;
public interface INexusLookUpService : ITransientService
{
    Task<List<NexusLookUpValueResponse>> GetNexusLookUpValuesByCodeAsync(NexusLookUpCodeTypes lookUpCodeType);

    Task<NexusLookUpValueResponse> GetNexusLookUpValueByIdAsync(int id);

    Task<List<DropDownItemResponse>> GetNexusLookUpValuesByCodeForDropDownAsync(NexusLookUpCodeTypes type);
}
