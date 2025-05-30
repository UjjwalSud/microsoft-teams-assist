namespace Microsoft.Teams.Assist.Application.Setting.Models.Response;
public class ViewSettingResponse
{
    public int Id { get; set; }

    public required SettingTypes SettingType { get; set; }

    public string? Description { get; set; }
}
