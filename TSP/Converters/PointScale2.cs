using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TSP.Converters
{
    public class PointScale2
    : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 4) return null;
            if (values[0] is double && values[1] is double && values[2] is double && values[3] is double)
            {
                double x = (double)values[0];
                double y = (double)values[1];
                double scaleX = (double)values[2];
                double scaleY = (double)values[3];
                return new Point(
                    x * scaleX,
                    y * scaleY);
            }
            else
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
