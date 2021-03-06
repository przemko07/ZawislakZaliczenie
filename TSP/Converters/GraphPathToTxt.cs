﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TSP.ViewModel;

namespace TSP.Converters
{
    class GraphPathToTxt
    : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GraphPathVM g = value as GraphPathVM;
            if (g == null) return string.Empty;
            if (!g.Edges.Any()) return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(g.Edges.First().Begin.Name);
            foreach (var edge in g.Edges)
            {
                sb.Append(" -> ");
                sb.Append(edge.End.Name);
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
