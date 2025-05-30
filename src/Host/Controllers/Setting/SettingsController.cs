using Microsoft.Teams.Assist.Application.Setting;
using Microsoft.Teams.Assist.Application.Setting.Models;
using Microsoft.Teams.Assist.Application.Setting.Models.Request;
using Microsoft.Teams.Assist.Application.Setting.Models.Response;
using Microsoft.Teams.Assist.Host.Controllers.BaseControllers;

namespace Microsoft.Teams.Assist.Host.Controllers.Setting;

public class SettingsController : VersionNeutralApiController
{
    public readonly ISettingService _settingService;

    public SettingsController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    [HttpGet("get-settings")]
    [MustHavePermission(SystemAction.View, SystemResource.ManageSettings)]
    [OpenApiOperation("Retrieve all settings", "")]
    public async Task<List<ViewSettingResponse>> GetSettings()
    {
        return await _settingService.GetSettingsAsync();
    }

    [HttpPut("update-setting")]
    [MustHavePermission(SystemAction.Update, SystemResource.ManageSettings)]
    [OpenApiOperation("Update setting details", "")]
    public async Task<string> UpdateSettings(UpdateSettingsRequest request)
    {
        return await _settingService.UpdateSettings(request);
    }
}
