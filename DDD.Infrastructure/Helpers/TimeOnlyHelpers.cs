namespace DDD.Infrastructure.Helpers;

public static class TimeOnlyHelpers
{
    public static TimeOnly ToUtc(this TimeOnly timeOnly)
    {
        var dateTime = new DateTime(2000, 01, 01, hour: timeOnly.Hour, minute: timeOnly.Minute, second: 00);
        return TimeOnly.FromDateTime(dateTime.ToUniversalTime());
    }
}
