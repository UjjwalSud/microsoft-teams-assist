namespace Microsoft.Teams.Assist.Application.Email.Model.Response;
public class ViewEmailLogResponse
{
    public required int Id { get; set; }
    public string? To { get; set; }
    public string? Subject { get; set; }
    public bool? IsEmailSent { get; set; }
    public DateTime CreatedOn { get; set; }
}
