using Microsoft.Teams.Assist.Domain.Common.Contracts;
using Microsoft.Teams.Assist.Domain.Enums.Nexus;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.LookUp.DbModels;

public class NexusLookUpCodes : AuditableEntity
{
    public required NexusLookUpCodeTypes LookUpCodeType { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<NexusLookUpCodeValues> NexusLookUpCodeValues { get; set; }
}
