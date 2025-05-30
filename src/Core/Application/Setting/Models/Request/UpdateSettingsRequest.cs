namespace Microsoft.Teams.Assist.Application.Setting.Models.Request;
public class UpdateSettingsRequest
{
    public required int Id { get; set; }

    public required string SettingJson { get; set; }
}
