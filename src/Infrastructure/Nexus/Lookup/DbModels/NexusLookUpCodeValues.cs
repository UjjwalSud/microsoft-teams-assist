using Microsoft.Teams.Assist.Domain.Common.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.LookUp.DbModels;
public class NexusLookUpCodeValues : AuditableEntity
{
    public required string LookUpValue { get; set; }

    public required int DisplayOrder { get; set; }

    [ForeignKey(nameof(NexusLookUpCode))]
    public int FKNexusLookUpCodePKId { get; set; }

    public required bool IsActive { get; set; }

    public virtual NexusLookUpCodes NexusLookUpCode { get; set; }
}
