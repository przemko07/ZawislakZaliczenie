using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TSP.Converters
{
    class DivConverter
    : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return 1;
            try
            {
                double v1 = Double.Parse(value.ToString());
                double v2 = Double.Parse(parameter.ToString());
                double result = v1 / v2;
                if (result < 1) result = 1;
                return result;
            }
            catch
            {
                return 1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
