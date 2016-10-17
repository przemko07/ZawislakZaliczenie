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

        public static GraphNodeVM[] NodesPath(GraphVM graph)
        {
            GraphNodeVM[] nodes = new GraphNodeVM[graph.Edges.Count + 1];

            var edgeEnum = graph.Edges.GetEnumerator();
            edgeEnum.MoveNext();
            GraphEdgeVM edge = edgeEnum.Current;
            if (edge != null)
            {
                nodes[0] = edge.Begin;
                for (int i = 1; i < nodes.Length; i++)
                {
                    bool begExist = nodes[i - 1] == edge.Begin;
                    bool endExist = nodes[i - 1] == edge.End;
                    if (!begExist)
                    {
                        nodes[i] = edge.Begin;
                    }
                    else if (!endExist)
                    {
                        nodes[i] = edge.End;
                    }
                    edgeEnum.MoveNext();
                    edge = edgeEnum.Current;
                }
            }

            return nodes;
        }
    }
}
