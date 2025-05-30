using Microsoft.Teams.Assist.Application.Common.Models;
using Microsoft.Teams.Assist.Application.LookUp;
using Microsoft.Teams.Assist.Application.LookUp.Models.Request;
using Microsoft.Teams.Assist.Application.LookUp.Models.Response;
using Microsoft.Teams.Assist.Host.Controllers.BaseControllers;

namespace Microsoft.Teams.Assist.Host.Controllers.LookUp;

public class LookUpController : VersionedApiController
{
    public readonly ILookUpService _lookUpService;

    public LookUpController(ILookUpService lookUpService)
    {
        _lookUpService = lookUpService;
    }

    [HttpGet("get-look-ups")]
    [MustHavePermission(SystemAction.View, SystemResource.ManageLookUps)]
    [OpenApiOperation("Retrieve all look-ups", "")]
    public async Task<List<ViewLookUpsResponse>> GetLookUpCodes()
    {
        return await _lookUpService.GetLookUpCodesAsync();
    }

    [HttpPost("get-look-up-values")]
    [MustHavePermission(SystemAction.View, SystemResource.ManageLookUps)]
    [OpenApiOperation("Retrieve look-up values", "")]
    public async Task<PaginationResponse<ViewLookUpCodeValuesResponse>> GetLookUpCodeValues(SearchLookUpCodeValuesRequest request)
    {
        return await _lookUpService.GetLookUpCodeValuesAsync(request);
    }

    [HttpPost("create-look-up-value")]
    [MustHavePermission(SystemAction.Create, SystemResource.ManageLookUps)]
    [OpenApiOperation("Create a new item for look-up", "")]
    public async Task<string> CreateLookUpCodeValue(CreateLookUpCodeValueRequest request)
    {
        return await _lookUpService.CreateLookUpCodeValueAsync(request);
    }

    [HttpGet("get-look-up-value/{id}")]
    [OpenApiOperation("Get look-up value by id", "")]
    [MustHavePermission(SystemAction.View, SystemResource.ManageLookUps)]
    public async Task<ViewLookUpCodeValuesResponse> GetLookUpCodeValueById(int id)
    {
        return await _lookUpService.GetLookUpCodeValueByIdAsync(id);
    }

    [HttpPut("update-look-up-value")]
    [OpenApiOperation("Update look-up value", "")]
    [MustHavePermission(SystemAction.Update, SystemResource.ManageLookUps)]
    public async Task<string> UpdateLookUpCodeValue(UpdateLookUpCodeValueRequest request)
    {
        return await _lookUpService.UpdateLookUpCodeValueAsync(request);
    }
}
