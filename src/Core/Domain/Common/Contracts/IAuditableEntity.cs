namespace Microsoft.Teams.Assist.Domain.Common.Contracts;

public interface IAuditableEntity
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? FKLastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public int TenantId { get; set; }
}
