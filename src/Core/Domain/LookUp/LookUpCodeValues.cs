using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Teams.Assist.Domain.LookUp;
public class LookUpCodeValues : AuditableEntity
{
    public required string LookUpValue { get; set; }

    public required int DisplayOrder { get; set; }

    [ForeignKey(nameof(LookUpCode))]
    public required int FKLookUpCodePKId { get; set; }

    public required bool IsActive { get; set; }

    public virtual LookUpCodes LookUpCode { get; set; }
}
