namespace Init.Application.Common.Extensions;

public static class DateTimeExtensions
{
    public static bool IsStartOfMonth(this DateTime date)
    {
        return date.Day == 1;
    }
    
    public static DateTime StartOfMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }
    
    public static DateOnly StartOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }
    
    public static DateTime EndOfMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    }

    public static DateOnly EndOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }

    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime);
    }

    public static DateTime ToDateTime(this DateOnly date)
    {
        return new DateTime(date.Year, date.Month, date.Day);
    }
}