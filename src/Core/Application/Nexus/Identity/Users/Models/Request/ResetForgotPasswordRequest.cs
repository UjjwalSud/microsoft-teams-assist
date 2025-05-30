using System.ComponentModel.DataAnnotations;

namespace Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Request;
public class ResetForgotPasswordRequest
{
    public string Email { get; set; }

    public string Password { get; set; } = default!;
    [Display(Name = "Confirm Password")]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = default!;

    public string Token { get; set; }

    //public string UserId { get; set; }
}
