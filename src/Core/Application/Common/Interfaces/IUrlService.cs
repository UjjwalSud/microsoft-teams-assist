namespace Microsoft.Teams.Assist.Application.Common.Interfaces;
public interface IUrlService : ITransientService
{
    string GenerateUrlIdentifier(int maxLength);
}
