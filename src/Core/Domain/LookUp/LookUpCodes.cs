using Microsoft.Teams.Assist.Domain.Enums;

namespace Microsoft.Teams.Assist.Domain.LookUp;

public class LookUpCodes : AuditableEntity
{
    public required LookUpCodeTypes LookUpCodeType { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<LookUpCodeValues> LookUpCodeValues { get; set; }
}
