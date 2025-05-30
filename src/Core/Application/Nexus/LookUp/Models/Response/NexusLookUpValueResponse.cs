namespace Microsoft.Teams.Assist.Application.Nexus.LookUp.Models.Response;
public class NexusLookUpValueResponse
{
    public required int Id { get; set; }
    public required string LookUpValue { get; set; }
    public required int DisplayOrder { get; set; }
    public required bool IsActive { get; set; }
}
