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
        public static string[] Names = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z".Split(' ');

        static GraphFactory()
        {
            List<string> newNames = new List<string>();
            for (int i = 0; i < Names.Length; i++)
            {
                newNames.Add(Names[i]);
            }
            for (int i = 0; i < Names.Length; i++)
            {
                for (int j = 0; j < Names.Length; j++)
                {
                    if (i == j) continue;
                    newNames.Add(Names[i] + Names[j]);
                }
            }
            Names = newNames.ToArray();
        }

        public static GraphVM GenerateClique(uint nodesCount)
        {
            GraphVM graph = new GraphVM();

            // Create Nodes
            for (uint i = 0; i < nodesCount; i++)
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

        public static GraphPathVM GeneratePath(GraphVM graph, Individual individual)
        {
            GraphPathVM path = new GraphPathVM(graph);

            for (uint i = 1; i < individual.Length; i++)
            {
                var edge = new GraphEdgeVM()
                {
                    Begin = graph.Nodes[(int)individual[i - 1]],
                    End = graph.Nodes[(int)individual[i]],
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
