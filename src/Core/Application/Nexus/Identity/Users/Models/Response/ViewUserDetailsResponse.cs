﻿namespace Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Response;
public class ViewUserDetailsResponse
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ImageUrl { get; set; }
}
