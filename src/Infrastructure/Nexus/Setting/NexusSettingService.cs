using Microsoft.Teams.Assist.Application.Common.Interfaces;
using Microsoft.Teams.Assist.Application.Nexus.Setting;
using Microsoft.Teams.Assist.Application.Nexus.Setting.Models;
using Microsoft.Teams.Assist.Domain.Enums.Nexus;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Setting;
public class NexusSettingService : INexusSettingService
{
    private readonly ISerializerService _serializerService;
    public NexusSettingService(ISerializerService serializerService)
    {
        _serializerService = serializerService;
    }

    public async Task<T> GetByCodeAsync<T>(NexusSettingTypes settingType)
    {
        switch (settingType)
        {
            case NexusSettingTypes.UserSettings:
                string temp = _serializerService.Serialize(new NexusUserSettingModel());
                return _serializerService.Deserialize<T>(temp);
            default:
                throw new NotImplementedException($"{settingType.ToString()} not implemented");
        }
    }
}
