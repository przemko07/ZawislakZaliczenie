using System;
using System.Globalization;
using System.Windows.Data;

namespace TSP.Converters
{
    public class DoubleIsNanConverter
    : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is double))
            {
                return false;
            }
            return double.IsNaN((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
