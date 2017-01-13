using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfFrontend.ViewModel
{
    public class GraphVM
    {
        public static int globalID = 0;
        public readonly int Id = globalID++;
        public string Name { get; set; } = string.Empty;
        public ObservableCollection<GraphNodeVM> Nodes { get; set; } = new ObservableCollection<GraphNodeVM>();
        public ObservableCollection<GraphEdgeVM> Edges { get; set; } = new ObservableCollection<GraphEdgeVM>();

        public override string ToString()
        {
            return Name;
        }
    }

    public class GraphPathVM
    {
        public readonly int Id = GraphVM.globalID++;
        public GraphVM Parent { get; }
        public string Name { get; set; } = string.Empty;
        public ObservableCollection<GraphNodeVM> Nodes { get; set; } = new ObservableCollection<GraphNodeVM>();
        public ObservableCollection<GraphEdgeVM> Edges { get; set; } = new ObservableCollection<GraphEdgeVM>();


        public GraphPathVM(GraphVM parent)
        {
            this.Parent = parent;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
