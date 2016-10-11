using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfFrontend.Extensions;
using WpfFrontend.ViewModel;

namespace WpfFrontend.Context
{
    public class MainWindowVM : ObjectVM
    {
        public string text { get; } = "Ala ma kota";

        public GraphVM Graph
        {
            get
            {
                GraphVM graph = new GraphVM();

                for (int i = 0; i < 10; i++)
                {
                    graph.Nodes.Add(new GraphNodeVM() {  Name = i.ToString() });
                }

                Random random = new Random();

                for (int i = 0; i < graph.Nodes.Count; i++)
                {
                    int i1 = random.Next(0, graph.Nodes.Count);
                    int i2 = random.Next(0, graph.Nodes.Count);
                    graph.Edges.Add(new GraphEdgeVM()
                    {
                        Name = i1 + "-" + i2,
                        Begin = graph.Nodes[i1],
                        End = graph.Nodes[i2],
                    });
                }

                return graph;
            }
        }
    }
}
