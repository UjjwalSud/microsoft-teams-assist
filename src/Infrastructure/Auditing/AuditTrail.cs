using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Teams.Assist.Application.Common.Interfaces;

namespace Microsoft.Teams.Assist.Infrastructure.Auditing;
public class AuditTrail
{
    private readonly ISerializerService _serializer;

    public AuditTrail(EntityEntry entry, ISerializerService serializer)
    {
        Entry = entry;
        _serializer = serializer;
    }

    public EntityEntry Entry { get; }
    public Guid CreatedBy { get; set; }
    public int TenantId { get; set; }
    public DateTime CreatedOn { get; set; }

    public string? TableName { get; set; }
    //public Dictionary<string, object?> KeyValues { get; } = new();

    public string PrimaryKey { get; set; }

    public Dictionary<string, object?> OldValues { get; } = new();
    public Dictionary<string, object?> NewValues { get; } = new();
    public List<PropertyEntry> TemporaryProperties { get; } = new();
    public TrailType TrailType { get; set; }
    public List<string> ChangedColumns { get; } = new();
    public bool HasTemporaryProperties => TemporaryProperties.Count > 0;

    public Trail ToAuditTrail() =>
    new()
    {
        CreatedBy = CreatedBy,
        Type = TrailType.ToString(),
        TableName = TableName,
        CreatedOn = CreatedOn,
        TenantId = TenantId,
        PrimaryKey = PrimaryKey.ToString(), //_serializer.Serialize(KeyValues),
        OldValues = OldValues.Count == 0 ? null : _serializer.Serialize(OldValues),
        NewValues = NewValues.Count == 0 ? null : _serializer.Serialize(NewValues),
        AffectedColumns = ChangedColumns.Count == 0 ? null : _serializer.Serialize(ChangedColumns)
    };
}
