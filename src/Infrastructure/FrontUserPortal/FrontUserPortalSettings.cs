using System.ComponentModel.DataAnnotations;

namespace Microsoft.Teams.Assist.Infrastructure.FrontUserPortal;
public class FrontUserPortalSettings
{
    public required UrlSettings Urls { get; set; }
}

public class UrlSettings : IValidatableObject
{
    public required string RegistrationConfirmUrl { get; set; }
    public required string ForgotPasswordUrl { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(RegistrationConfirmUrl))
        {
            yield return new ValidationResult("No Key defined in FrontUserPortalSettings config", new[] { nameof(RegistrationConfirmUrl) });
        }

        if (string.IsNullOrEmpty(ForgotPasswordUrl))
        {
            yield return new ValidationResult("No Key defined in FrontUserPortalSettings config", new[] { nameof(ForgotPasswordUrl) });
        }
    }
}