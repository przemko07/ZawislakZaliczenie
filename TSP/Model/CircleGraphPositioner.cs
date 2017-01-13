using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TSP.ViewModel;

namespace TSP.Model
{
    class CircleGraphPositioner
    {
        private readonly GraphVM graph;

        public CircleGraphPositioner(GraphVM graph)
        {
            this.graph = graph;
        }

        public Point Position(GraphNodeVM node, double rx, double ry, double cx, double cy)
        {
            int index = graph.Nodes.IndexOf(node);
            double angle = ((double)graph.Nodes.IndexOf(node) / graph.Nodes.Count) * (Math.PI * 2);
            return new Point(
                cx + Math.Cos(angle) * rx,
                cy + Math.Sin(angle) * ry);
        }
    }
}
