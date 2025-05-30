namespace Microsoft.Teams.Assist.Application.Common.Models.Response;
public class DropDownItemResponse
{
    public required int Value { get; set; }
    public required string Text { get; set; }
    public bool IsSelected { get; set; } // Optional
}
