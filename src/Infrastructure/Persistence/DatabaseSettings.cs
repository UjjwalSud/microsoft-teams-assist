using System.ComponentModel.DataAnnotations;

namespace Microsoft.Teams.Assist.Infrastructure.Persistence;

public class DatabaseSettings : IValidatableObject
{
    public string Nexus_DBProvider { get; set; } = string.Empty;
    public string Nexus_ConnectionString { get; set; } = string.Empty;
    public string Application_DBProvider { get; set; } = string.Empty;
    public string Application_ConnectionString { get; set; } = string.Empty;

    public string AuditTrail_DBProvider { get; set; } = string.Empty;
    public string AuditTrail_ConnectionString { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Nexus_DBProvider))
        {
            yield return new ValidationResult(
                $"{nameof(DatabaseSettings)}.{nameof(Nexus_DBProvider)} is not configured",
                new[] { nameof(Nexus_DBProvider) });
        }

        if (string.IsNullOrEmpty(Nexus_ConnectionString))
        {
            yield return new ValidationResult(
                $"{nameof(DatabaseSettings)}.{nameof(Nexus_ConnectionString)} is not configured",
                new[] { nameof(Nexus_ConnectionString) });
        }

        if (string.IsNullOrEmpty(Application_DBProvider))
        {
            yield return new ValidationResult(
                $"{nameof(DatabaseSettings)}.{nameof(Application_DBProvider)} is not configured",
                new[] { nameof(Application_DBProvider) });
        }

        if (string.IsNullOrEmpty(Application_ConnectionString))
        {
            yield return new ValidationResult(
                $"{nameof(DatabaseSettings)}.{nameof(Application_ConnectionString)} is not configured",
                new[] { nameof(Application_ConnectionString) });
        }


        if (string.IsNullOrEmpty(AuditTrail_DBProvider))
        {
            yield return new ValidationResult(
                $"{nameof(DatabaseSettings)}.{nameof(AuditTrail_DBProvider)} is not configured",
                new[] { nameof(AuditTrail_DBProvider) });
        }

        if (string.IsNullOrEmpty(AuditTrail_ConnectionString))
        {
            yield return new ValidationResult(
                $"{nameof(DatabaseSettings)}.{nameof(AuditTrail_ConnectionString)} is not configured",
                new[] { nameof(AuditTrail_ConnectionString) });
        }
    }
}
