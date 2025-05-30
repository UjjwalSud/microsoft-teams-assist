using Microsoft.Teams.Assist.Application.Nexus.LookUp.Models.Response;
using Microsoft.Teams.Assist.Application.Nexus.LookUp;
using Microsoft.Teams.Assist.Domain.Enums.Nexus;
using Microsoft.Teams.Assist.Host.Controllers.BaseControllers;

namespace Microsoft.Teams.Assist.Host.Controllers.LookUp;

[Route("nexus-lookup")]
public class NexusLookUpController : VersionedApiController
{
    private readonly INexusLookUpService _nexusLookUpService;

    public NexusLookUpController(INexusLookUpService nexusLookUpService)
    {
        _nexusLookUpService = nexusLookUpService;
    }

    [HttpGet("lookup-values")]
    [OpenApiOperation("Retrieve look-up values by 'type'", "")]
    public async Task<List<NexusLookUpValueResponse>> LookUpValues(NexusLookUpCodeTypes type)
    {
        return await _nexusLookUpService.GetNexusLookUpValuesByCodeAsync(type);
    }

    [HttpGet("lookup-value/{id}")]
    [OpenApiOperation("Retrieve look-up value by Id", "")]
    public async Task<NexusLookUpValueResponse> LookUpValueById(int id)
    {
        return await _nexusLookUpService.GetNexusLookUpValueByIdAsync(id);
    }
}
