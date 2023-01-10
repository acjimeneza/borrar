namespace Deluxe.TokenRefresh.Domain.Utilities;

public static class StringExtensions
{
    public static T? ToEnum<T>(this string value)
    {
        if (Enum.TryParse(typeof(T), value, true, out var enumValue))
            return (T)enumValue;

        return default;
    }
}
