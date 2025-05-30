namespace Microsoft.Teams.Assist.Application.LookUp.Models.Response;
public class ViewLookUpsResponse
{
    public int Id { get; set; }
    public required LookUpCodeTypes LookUpCodeType { get; set; }
    public string? Description { get; set; }
}
