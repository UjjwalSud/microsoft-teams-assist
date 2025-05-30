using System.ComponentModel.DataAnnotations;

namespace Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Request;
public class ChangePasswordRequest
{
    public string Password { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    [Display(Name = "Confirm Password")]
    [Compare(nameof(NewPassword))]
    public string ConfirmNewPassword { get; set; } = default!;
}