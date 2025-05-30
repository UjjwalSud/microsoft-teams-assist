using Microsoft.Teams.Assist.Application.Nexus.Identity.Tokens.Models.Request;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Tokens.Models.Response;

namespace Microsoft.Teams.Assist.Application.Nexus.Identity.Tokens;
public interface ITokenService : ITransientService
{
    Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken);

    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
}