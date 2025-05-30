using Microsoft.Teams.Assist.Domain.Enums;

namespace Microsoft.Teams.Assist.Domain.Setting;
public class Settings : AuditableEntity
{
    public required SettingTypes SettingType { get; set; }

    public string? Description { get; set; }

    public required string OriginalValue { get; set; }

    public string? SettingValues { get; set; }
}
