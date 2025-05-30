namespace Microsoft.Teams.Assist.Application.Common.Interfaces;

public interface IDateTimeService : ITransientService
{
    DateTime GetServerDateTime { get; }

    DateTime UtcNow { get; }
}
