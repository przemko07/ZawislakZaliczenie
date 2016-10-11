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

                return graph;
            }
        }
    }
}
