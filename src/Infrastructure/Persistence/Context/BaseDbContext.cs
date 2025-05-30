using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Teams.Assist.Application.Common.Interfaces;
using Microsoft.Teams.Assist.Domain.Common.Contracts;
using Microsoft.Teams.Assist.Infrastructure.Auditing;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Auditing;
using System.Data;

namespace Microsoft.Teams.Assist.Infrastructure.Persistence.Context;
public class BaseDbContext : DbContext
{
    protected readonly ICurrentUser _currentUser;
    private readonly ISerializerService _serializer;
    private readonly AuditingDbContext _auditingDbContext;
    private readonly IDateTimeService _createdTimeService;
    public BaseDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, AuditingDbContext auditingDbContext, IDateTimeService createdTimeService)
   : base(options)
    {
        _currentUser = currentUser;
        _serializer = serializer;
        _auditingDbContext = auditingDbContext;
        _createdTimeService = createdTimeService;
    }

    // Used by Dapper
    public IDbConnection Connection => Database.GetDbConnection();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // QueryFilters need to be applied before base.OnModelCreating
        modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => !s.IsDeleted);
        modelBuilder.AppendGlobalQueryFilter<IAuditableEntity>(s => s.TenantId == _currentUser.GetTenant());

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var auditEntries = HandleAuditingBeforeSaveChanges(_currentUser.GetUserId(), _currentUser.GetTenant());

        int result = await base.SaveChangesAsync(cancellationToken);

        await HandleAuditingAfterSaveChangesAsync(auditEntries, cancellationToken);

        //await SendDomainEventsAsync();

        return result;
    }

    private List<AuditTrail> HandleAuditingBeforeSaveChanges(Guid userId, int tenantId)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.FKLastModifiedBy = userId;
                    entry.Entity.TenantId = tenantId;
                    entry.Entity.CreatedOn = _createdTimeService.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = _createdTimeService.UtcNow;
                    entry.Entity.FKLastModifiedBy = userId;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.IsDeleted = true;
                        softDelete.FKDeletedBy = userId;
                        softDelete.DeletedOn = _createdTimeService.UtcNow;
                        entry.State = EntityState.Modified;
                    }

                    break;
            }
        }

        ChangeTracker.DetectChanges();

        var trailEntries = new List<AuditTrail>();
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
            .ToList())
        {
            var trailEntry = new AuditTrail(entry, _serializer)
            {
                TableName = entry.Entity.GetType().Name,
                CreatedBy = userId,
                TenantId = tenantId,
                CreatedOn = _createdTimeService.UtcNow
            };
            trailEntries.Add(trailEntry);
            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    trailEntry.TemporaryProperties.Add(property);
                    continue;
                }

                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    //trailEntry.KeyValues[propertyName] = property.CurrentValue;
                    trailEntry.PrimaryKey = property.CurrentValue.ToString();
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        trailEntry.TrailType = TrailType.Create;
                        trailEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        trailEntry.TrailType = TrailType.Delete;
                        trailEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified && entry.Entity is ISoftDelete && property.OriginalValue == null && property.CurrentValue != null)
                        {
                            trailEntry.ChangedColumns.Add(propertyName);
                            trailEntry.TrailType = TrailType.Delete;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                        {
                            trailEntry.ChangedColumns.Add(propertyName);
                            trailEntry.TrailType = TrailType.Update;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                        }

                        break;
                }
            }
        }

        foreach (var auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
        {
            _auditingDbContext.AuditTrails.Add(auditEntry.ToAuditTrail());
        }

        return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
    }

    private Task HandleAuditingAfterSaveChangesAsync(List<AuditTrail> trailEntries, CancellationToken cancellationToken = new())
    {
        //if (trailEntries == null || trailEntries.Count == 0)
        //{
        //    return;
        //}

        foreach (var entry in trailEntries)
        {
            foreach (var prop in entry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    //entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    entry.PrimaryKey = prop.CurrentValue.ToString();
                }
                else
                {
                    entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }

            _auditingDbContext.AuditTrails.Add(entry.ToAuditTrail());
        }

        return _auditingDbContext.SaveChangesAsync(cancellationToken);
    }
}
