using Microsoft.Teams.Assist.Application.Setting.Models.Request;
using Microsoft.Teams.Assist.Application.Setting.Models.Response;

namespace Microsoft.Teams.Assist.Application.Setting;
public interface ISettingService : ITransientService
{
    Task<List<ViewSettingResponse>> GetSettingsAsync();

    Task<ViewSettingResponse> GetById(int id);

    Task<ViewSettingDetailResponse<T>> GetSettingByIdAsync<T>(int id);

    Task<string> UpdateSettings(UpdateSettingsRequest request);
}