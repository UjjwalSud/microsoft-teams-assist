using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Infrastructure.SystemConstants;
using System.Text;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Identity;
internal partial class UserService
{
    public async Task<string> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == userId && !u.EmailConfirmed)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new BadRequestException(ErrorMessages.ConfirmEmail);

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);

        return result.Succeeded
            ? string.Format(SuccessMessages.ConfirmEmail, user.Email)
            : throw new BadRequestException(ErrorMessages.ConfirmEmail);
    }
}
