using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Microsoft.Teams.Assist.Application.Common.Interfaces;
using Microsoft.Teams.Assist.Application.Common.Mailing;
using Microsoft.Teams.Assist.Application.Common.Mailing.Models;
using Microsoft.Teams.Assist.Application.Email;
using Microsoft.Teams.Assist.Application.Nexus.MultiTenant;
using Microsoft.Teams.Assist.Application.Nexus.Setting;
using Microsoft.Teams.Assist.Domain.Email;
using Microsoft.Teams.Assist.Infrastructure.Auth;
using Microsoft.Teams.Assist.Infrastructure.FrontUserPortal;
using Microsoft.Teams.Assist.Infrastructure.Mailing.Models;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;
namespace Microsoft.Teams.Assist.Infrastructure.Mailing;

public partial class MailService : IMailService
{
    private readonly INexusSettingService _nexusSettingService;
    private readonly MailSettings _settings;
    private readonly ILogger<MailService> _logger;
    private readonly FrontUserPortalSettings _frontUserPortalSettings;
    private readonly IEmailLogService _emailLogService;
    private readonly ISerializerService _serializerService;
    private readonly ITenantService _tenantService;
    private readonly ICurrentUserInitializer _currentUserInitializer;
    public readonly NexusDbContext _nexusDbContext;

    public MailService(IOptions<MailSettings> settings, ILogger<MailService> logger, INexusSettingService nexusSettingService, IOptions<FrontUserPortalSettings> frontUserPortalSettings, IEmailLogService emailLogService, ISerializerService serializerService, ITenantService tenantService, ICurrentUserInitializer currentUserInitializer, NexusDbContext nexusDbContext)
    {
        _settings = settings.Value;
        _logger = logger;
        _nexusSettingService = nexusSettingService;
        _frontUserPortalSettings = frontUserPortalSettings.Value;
        _emailLogService = emailLogService;
        _serializerService = serializerService;
        _tenantService = tenantService;
        _currentUserInitializer = currentUserInitializer;
        _nexusDbContext = nexusDbContext;
    }

    private async Task SetCurrentUserAndTenantAsync(string userId, CancellationToken cancellationToken)
    {
        var tenantDto = await _tenantService.GetByUserIdAsync(userId, cancellationToken);
        _currentUserInitializer.SetCurrentUserId(userId);
        _currentUserInitializer.SetCurrentTenant(tenantDto.Id, tenantDto.UniqueId);
    }

    public async Task SendAsync(MailDto request, CancellationToken cancellationToken = default)
    {
        EmailLog log = new EmailLog
        {
            To = string.Join(",", request.To),
            Subject = request.Subject,
            Body = request.Body,
            EmailType = request.EmailType,
            From = request.From,
            DisplayName = request.DisplayName,
            ReplyTo = request.ReplyTo,
            ReplyToName = request.ReplyToName,
            Bcc = request.Bcc == null ? null : string.Join(",", request.Bcc),
            Cc = request.Cc == null ? null : string.Join(",", request.Cc),
            Headers = request.Headers == null ? null : string.Join(",", request.Headers),
            EmailSmtpUsed = _serializerService.Serialize(_settings)
        };
        try
        {
            var email = new MimeMessage();

            // From
            email.From.Add(new MailboxAddress(_settings.DisplayName, request.From ?? _settings.From));

            // To
            foreach (string address in request.To)
                email.To.Add(MailboxAddress.Parse(address));

            // Reply To
            if (!string.IsNullOrEmpty(request.ReplyTo))
                email.ReplyTo.Add(new MailboxAddress(request.ReplyToName, request.ReplyTo));

            // Bcc
            if (request.Bcc != null)
            {
                foreach (string address in request.Bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                    email.Bcc.Add(MailboxAddress.Parse(address.Trim()));
            }

            // Cc
            if (request.Cc != null)
            {
                foreach (string? address in request.Cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                    email.Cc.Add(MailboxAddress.Parse(address.Trim()));
            }

            // Headers
            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                    email.Headers.Add(header.Key, header.Value);
            }

            // Content
            var builder = new BodyBuilder();
            email.Sender = new MailboxAddress(request.DisplayName ?? _settings.DisplayName, request.From ?? _settings.From);
            email.Subject = request.Subject;
            builder.HtmlBody = request.Body;

            // Create the file attachments for this e-mail message
            if (request.AttachmentData != null)
            {
                foreach (var attachmentInfo in request.AttachmentData)
                    builder.Attachments.Add(attachmentInfo.Key, attachmentInfo.Value);
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
            log.IsEmailSent = true;
            log.EmailSentMessage = "E-mail sent successfully";
        }
        catch (Exception exception)
        {
            log.IsEmailSent = false;
            log.EmailSentMessage = exception.Message.Trim() + "\n" + (exception.InnerException != null ? exception.InnerException.Message.Trim() : string.Empty);
            _logger.LogError(exception, exception.Message);
        }

        await _emailLogService.AddEmailLogAsync(log);
    }
}
