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
    public class PointScale
    : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3) return null;
            if (values[0] is Point && values[1] is double && values[2] is double)
            {
                Point point = (Point)values[0];
                double scaleX = (double)values[1];
                double scaleY = (double)values[2];
                return new Point(
                    point.X * scaleX,
                    point.Y * scaleY);
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
