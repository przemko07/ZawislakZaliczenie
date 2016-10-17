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
        public static string[] Names = "A,B,C,D,E,F,G,H,J,K,L,M,P,K,L".Split(',');

        public static GraphVM GenerateClique(Permutacja permutacja)
        {
            GraphVM graph = new GraphVM();

            // Create Nodes
            for (int i = 0; i < permutacja.permutacja.GetLength(1); i++)
            {
                graph.Nodes.Add(new GraphNodeVM()
                {
                    Name = Names[i],
                });
            }

            // Connect each node with every node (without itself)
            foreach (var node in graph.Nodes)
            {
                foreach (var neighbour in graph.Nodes.Where(n => n != node))
                {
                    var edge = new GraphEdgeVM()
                    {
                        Begin = node,
                        End = neighbour,
                    };
                    node.Edges.Add(edge);
                    graph.Edges.Add(edge);
                }
            }

            return graph;
        }

        public static GraphVM GeneratePath(GraphVM graph, Permutacja permutacja)
        {
            GraphVM path = new GraphVM();

            for (int i = 1; i < permutacja.permutacja.GetLength(1); i++)
            {
                var edge = new GraphEdgeVM()
                {
                    Begin = graph.Nodes[permutacja.permutacja[0, i - 1]],
                    End = graph.Nodes[permutacja.permutacja[0, i]],
                };
                path.Edges.Add(edge);
            }

            // add last - first
            path.Edges.Add(new GraphEdgeVM()
            {
                Begin = path.Edges.Last().End,
                End = path.Edges.First().Begin,
            });

            foreach (var edge in path.Edges)
            {
                if (!path.Nodes.Contains(edge.Begin)) path.Nodes.Add(edge.Begin);
                if (!path.Nodes.Contains(edge.End)) path.Nodes.Add(edge.End);
            }

            return path;
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
