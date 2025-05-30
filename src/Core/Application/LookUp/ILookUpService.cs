using Microsoft.Teams.Assist.Application.Common.Models;
using Microsoft.Teams.Assist.Application.Common.Models.Response;
using Microsoft.Teams.Assist.Application.LookUp.Models.Request;
using Microsoft.Teams.Assist.Application.LookUp.Models.Response;

namespace Microsoft.Teams.Assist.Application.LookUp;
public interface ILookUpService : ITransientService
{
    Task<List<ViewLookUpsResponse>> GetLookUpCodesAsync();

    Task<PaginationResponse<ViewLookUpCodeValuesResponse>> GetLookUpCodeValuesAsync(SearchLookUpCodeValuesRequest request);

    Task<List<DropDownItemResponse>> GetLookUpCodeValuesByTypeAsync(LookUpCodeTypes type);

    Task<string> CreateLookUpCodeValueAsync(CreateLookUpCodeValueRequest request);

    Task<string> UpdateLookUpCodeValueAsync(UpdateLookUpCodeValueRequest request);

    Task<ViewLookUpCodeValuesResponse> GetLookUpCodeValueByIdAsync(int id);
}
