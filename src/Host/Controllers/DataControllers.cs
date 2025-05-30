using Microsoft.Teams.Assist.Application.Common.Models.Response;
using Microsoft.Teams.Assist.Application.LookUp;
using Microsoft.Teams.Assist.Application.Nexus.LookUp;
using Microsoft.Teams.Assist.Domain.Enums;
using Microsoft.Teams.Assist.Domain.Enums.Nexus;
using Microsoft.Teams.Assist.Host.Controllers.BaseControllers;

namespace Microsoft.Teams.Assist.Host.Controllers;

public class DataControllers : VersionNeutralApiController
{

    public readonly ILookUpService _lookUpService;
    public readonly INexusLookUpService _nexusLookUpService;


    public DataControllers(ILookUpService lookUpService, INexusLookUpService nexusLookUpService)
    {
        _lookUpService = lookUpService;
        _nexusLookUpService = nexusLookUpService;
    }

    [HttpPost("drp-get-look-up-values")]
    [OpenApiOperation("Retrieve look-up values for drop-down by type", "")]
    public async Task<List<DropDownItemResponse>> GetLookUpCodeValues(LookUpCodeTypes type)
    {
        return await _lookUpService.GetLookUpCodeValuesByTypeAsync(type);
    }

    [HttpPost("drp-nexus-get-look-up-values")]
    [OpenApiOperation("Retrieve look-up values for drop-down by type", "")]
    public async Task<List<DropDownItemResponse>> GetNexusLookUpCodeValues(NexusLookUpCodeTypes type)
    {
        return await _nexusLookUpService.GetNexusLookUpValuesByCodeForDropDownAsync(type);
    }

}
