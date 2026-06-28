using System.Globalization;

namespace RougeTracker.Converters
{
    public class NumberToIncomeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resources = Application.Current!.Resources;
            if (value is decimal v)
            {
                if (v > 0) return (Color)resources["TextSuccess"];
                if (v < 0) return (Color)resources["TextDanger"];
            }

            return (Color)resources["TextInfo"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
