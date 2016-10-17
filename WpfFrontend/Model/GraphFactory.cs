using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfFrontend.ViewModel;

namespace WpfFrontend.Model
{
    public static class GraphFactory
    {
        public static GraphVM Generate(Permutacja permutacja)
        {
            return null;
        }

        public static void FillEdges(GraphVM graph)
        {
            foreach (var edge in graph.Edges)
            {
                edge.Begin.Edges.Add(edge);
                edge.End.Edges.Add(edge);
            }
        }

        public static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
    }
}
