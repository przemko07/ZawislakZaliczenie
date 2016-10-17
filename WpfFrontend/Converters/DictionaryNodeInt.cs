using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfFrontend.Extensions;
using WpfFrontend.ViewModel;

namespace WpfFrontend.Converters
{
    class DictionaryNodeInt
    : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return null;
            if (values[0] == null) return null;

            ObservableDictionary<GraphNodeVM, int> nodes = values[0] as ObservableDictionary<GraphNodeVM, int>;
            if (nodes == null) return null;
            if (values[1] == null) return null;
            GraphNodeVM node = values[1] as GraphNodeVM;
            if (node == null) return null;

            try
            {
                var tmp = nodes[node];
                return tmp.ToString();
            }
            catch
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
