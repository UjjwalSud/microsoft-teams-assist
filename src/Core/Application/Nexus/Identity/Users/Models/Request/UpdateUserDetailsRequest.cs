namespace Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Request;
public class UpdateUserDetailsRequest
{
    public bool IsActive { get; set; }
    public string? UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    //public string? Email { get; set; }
    //public bool EmailConfirmed { get; set; }
}

