using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Teams.Assist.Application.Common.Interfaces;
using Microsoft.Teams.Assist.Infrastructure.Nexus.LookUp.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Nexus.MultiTenant.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Nexus.Subscription.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Configuration;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Auditing;

namespace Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;

public class NexusDbContext : NexusBaseDbContext
{
    public NexusDbContext(DbContextOptions<NexusDbContext> options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, AuditingDbContext auditingDbContext, IDateTimeService createdTimeService)
   : base(options, currentUser, serializer, dbSettings, auditingDbContext, createdTimeService)
    {
    }

    public DbSet<Subscriptions> Subscriptions => Set<Subscriptions>();
    public DbSet<Tenants> Tenants => Set<Tenants>();
    public DbSet<NexusLookUpCodes> NexusLookUpCodes => Set<NexusLookUpCodes>();
    public DbSet<NexusLookUpCodeValues> NexusLookUpCodeValues => Set<NexusLookUpCodeValues>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NexusLookUpCodes>()
               .ToTable("LookUpCodes");

        modelBuilder.Entity<NexusLookUpCodeValues>()
             .ToTable("LookUpCodeValues");

        #region Foreign key
        var cascadeFKs = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys())
                        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
        {
            fk.DeleteBehavior = DeleteBehavior.NoAction;
        }
        #endregion

        modelBuilder.HasDefaultSchema(SchemaNames.dbo);

        base.OnModelCreating(modelBuilder);
    }
}