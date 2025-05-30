namespace Microsoft.Teams.Assist.Application.LookUp.Models.Request;
public class CreateLookUpCodeValueRequest
{
    public required string LookUpValue { get; set; }

    public required int DisplayOrder { get; set; }

    public int LookUpCodeId { get; set; }

    public required bool IsActive { get; set; }
}
