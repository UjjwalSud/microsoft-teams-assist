using Microsoft.Teams.Assist.Domain.Enums.Nexus;

namespace Microsoft.Teams.Assist.Application.Nexus.Setting;
public interface INexusSettingService : ITransientService
{
    Task<T> GetByCodeAsync<T>(NexusSettingTypes settingType);
}
