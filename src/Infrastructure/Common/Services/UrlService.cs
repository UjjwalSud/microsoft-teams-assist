using Microsoft.Teams.Assist.Application.Common.Interfaces;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace Microsoft.Teams.Assist.Infrastructure.Common.Services;
public class UrlService : IUrlService
{
    public string GenerateUrlIdentifier(int maxLength)
    {
        // Generate a Guid and convert it to a string without dashes
        string input = Guid.NewGuid().ToString("N");

        // Normalize the input to remove diacritical marks (accents) and convert to ASCII equivalent
        var normalizedString = input.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();
        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        // Remove non-alphanumeric characters, replace spaces with hyphens, and convert to lowercase
        var baseSlug = Regex.Replace(stringBuilder.ToString(), @"[^a-zA-Z0-9\s-]", "")
                             .ToLower()
                             .Trim();
        baseSlug = Regex.Replace(baseSlug, @"\s+", "-").Trim('-');

        // Generate a random number for additional uniqueness
        var randomNumber = new Random().Next(1000, int.MaxValue);

        // Generate a short Guid to append
        var shortGuid = Guid.NewGuid().ToString().Substring(0, 8);

        // Combine the slug, random number, and short GUID
        var combinedSlug = $"{baseSlug}-{randomNumber}-{shortGuid}";

        // Truncate the final slug if it exceeds maxLength
        if (combinedSlug.Length > maxLength)
        {
            combinedSlug = combinedSlug.Substring(0, maxLength).TrimEnd('-');
        }

        return combinedSlug;
    }
}
