using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TSP.Extensions;
using TSP.ViewModel;

namespace TSP.Converters
{
    class Dictionaryitem
    : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return null;
            if (values[0] == null) return null;

            ObservableDictionary<GraphNodeVM, Point> nodes = values[0] as ObservableDictionary<GraphNodeVM, Point>;
            if (nodes == null) return null;
            if (values[1] == null) return null;
            GraphNodeVM node = values[1] as GraphNodeVM;
            if (node == null) return null;
            if (!nodes.ContainsKey(node)) return null;

            return nodes[node];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
