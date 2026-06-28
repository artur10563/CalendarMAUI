using System.Globalization;
namespace RougeTracker.Converters;

public class SignedNumberConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not decimal number) return string.Empty;
        return number switch
        {
            > 0 => $"+{number:0.00}",
            < 0 => $"{number:0.00}",
            _ => "0.00"
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var str = value?.ToString()?.Trim(); if (string.IsNullOrWhiteSpace(str)) return null;
        if (str is "-" or "+" or "." or ",") return null;

        var sep = culture.NumberFormat.NumberDecimalSeparator[0];

        str = str.Replace(',', sep)
            .Replace('.', sep);

        if (decimal.TryParse(
            str, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
            culture, out var result))
        {
            return result;
        }

        return null;
    }
}