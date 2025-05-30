using Microsoft.Teams.Assist.Application.Common.Mailing.Models;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Request;

namespace Microsoft.Teams.Assist.Application.Common.Mailing;

public interface IMailService : ITransientService
{
    Task SendAsync(MailDto request, CancellationToken ct);

    Task EmailRegistrationVerificationEmailAsync(string userId, string code,CancellationToken cancellationToken);

    Task EmailForgotPasswordAsync(string userId, string code, ForgotPasswordRequest request, CancellationToken cancellationToken);
}
