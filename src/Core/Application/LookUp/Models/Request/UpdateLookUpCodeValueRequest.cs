namespace Microsoft.Teams.Assist.Application.LookUp.Models.Request;
public class UpdateLookUpCodeValueRequest
{
    public int Id { get; set; }

    public required string LookUpValue { get; set; }

    public required int DisplayOrder { get; set; }

    public required bool IsActive { get; set; }
}
