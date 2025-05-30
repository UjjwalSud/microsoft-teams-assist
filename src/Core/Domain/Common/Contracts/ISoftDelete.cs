namespace Microsoft.Teams.Assist.Domain.Common.Contracts;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    DateTime? DeletedOn { get; set; }
    Guid? FKDeletedBy { get; set; }
}
