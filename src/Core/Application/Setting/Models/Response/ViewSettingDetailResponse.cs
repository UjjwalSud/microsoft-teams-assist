namespace Microsoft.Teams.Assist.Application.Setting.Models.Response;
public class ViewSettingDetailResponse<T>
{
    public required int Id { get; set; }

    public required T SettingDetail { get; set; }

    public required T OriginalValue { get; set; }

    public required SettingTypes SettingType { get; set; }
}
