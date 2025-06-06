﻿using Microsoft.Teams.Assist.Domain.Enums;

namespace Microsoft.Teams.Assist.Domain.Email;
public class EmailLog : AuditableEntity
{
    public required string To { get; set; }

    public required string Subject { get; set; }

    public required EmailTypes EmailType { get; set; }

    public required string Body { get; set; }

    public required string From { get; set; }

    public required string DisplayName { get; set; }

    public string? ReplyTo { get; set; }

    public string? ReplyToName { get; set; }

    public string? Bcc { get; set; }

    public string? Cc { get; set; }

    public string? Headers { get; set; }

    public string? EmailSmtpUsed { get; set; }

    public bool IsEmailSent { get; set; }

    public string? EmailSentMessage { get; set; }
}
