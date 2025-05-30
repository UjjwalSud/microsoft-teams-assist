using Microsoft.Teams.Assist.Application.Common.Models;

namespace Microsoft.Teams.Assist.Application.Email.Model.Request;
public class SearchEmailLogRequest : SearchRequestBaseClass
{
    public string? To { get; set; }
    public string? Subject { get; set; }
    public bool? SentStatus { get; set; }
    public DateTime? SendDateTime { get; set; }
}
