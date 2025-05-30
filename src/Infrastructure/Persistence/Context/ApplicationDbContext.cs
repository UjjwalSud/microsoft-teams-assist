using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Teams.Assist.Application.Common.Interfaces;
using Microsoft.Teams.Assist.Domain.Email;
using Microsoft.Teams.Assist.Domain.LookUp;
using Microsoft.Teams.Assist.Domain.Setting;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Configuration;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Auditing;

namespace Microsoft.Teams.Assist.Infrastructure.Persistence.Context;
public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, AuditingDbContext auditingDbContext, IDateTimeService createdTimeService)
: base(options, currentUser, serializer, dbSettings, auditingDbContext, createdTimeService)
    {
    }

    public DbSet<EmailLog> EmailLog => Set<EmailLog>();

    public DbSet<LookUpCodes> LookUpCodes => Set<LookUpCodes>();

    public DbSet<LookUpCodeValues> LookUpCodeValues => Set<LookUpCodeValues>();

    public DbSet<Settings> Settings => Set<Settings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
