using System.Globalization;

namespace RougeTracker.Converters
{
    public class IntToIncomeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int v)
            {
                if (v > 0) return Colors.Green;
                if (v < 0) return Colors.Red;
            }

            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
