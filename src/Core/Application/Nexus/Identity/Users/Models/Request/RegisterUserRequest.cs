using System.ComponentModel.DataAnnotations;

namespace Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Request;
public class RegisterUserRequest
{
    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }

    [Display(Name = "Confirm Password")]
    [Compare(nameof(Password))]
    [Required]
    public required string ConfirmPassword { get; set; }
    public string? PhoneNumber { get; set; }
}
