namespace Microsoft.Teams.Assist.Application.Common.Mailing.Models;

public class MailDto
{
    //public MailDto(List<string> to, string subject, string? body = null, string? from = null, string? displayName = null, string? replyTo = null, string? replyToName = null, List<string>? bcc = null, List<string>? cc = null, IDictionary<string, byte[]>? attachmentData = null, IDictionary<string, string>? headers = null)
    //{
    //    To = to;
    //    Subject = subject;
    //    Body = body;
    //    From = from;
    //    DisplayName = displayName;
    //    ReplyTo = replyTo;
    //    ReplyToName = replyToName;
    //    Bcc = bcc ?? new List<string>();
    //    Cc = cc ?? new List<string>();
    //    AttachmentData = attachmentData ?? new Dictionary<string, byte[]>();
    //    Headers = headers ?? new Dictionary<string, string>();
    //}

    public List<string> To { get; set; }

    public required string Subject { get; set; }

    public required string Body { get; set; }

    public required EmailTypes EmailType { get; set; }

    public required string From { get; set; }

    public required string DisplayName { get; set; }

    public string? ReplyTo { get; set; }

    public string? ReplyToName { get; set; }

    public List<string>? Bcc { get; set; }

    public List<string>? Cc { get; set; }

    public IDictionary<string, byte[]>? AttachmentData { get; set; }

    public IDictionary<string, string>? Headers { get; set; }
}
