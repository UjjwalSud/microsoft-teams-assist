using Microsoft.Teams.Assist.Application.Common.Interfaces;

namespace Microsoft.Teams.Assist.Infrastructure.Common.Services;

public class DateTimeService : IDateTimeService
{

    public DateTime GetServerDateTime
    {
        get
        {
            return DateTime.Now.ToLocalTime();
        }
    }

    public DateTime UtcNow
    {
        get
        {
            return DateTime.UtcNow;
        }
    }
}

public static class DateTimeService2
{

    public static DateTime GetServerDateTime
    {
        get
        {
            return DateTime.Now.ToLocalTime();
        }
    }

    public static DateTime UtcNow
    {
        get
        {
            return DateTime.UtcNow;
        }
    }
}
