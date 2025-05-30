namespace Microsoft.Teams.Assist.Application.Nexus.Identity.Tokens.Models.Response;

public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);