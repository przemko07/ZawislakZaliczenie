using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfFrontend.ViewModel
{
    public class GraphNodeVM
    {
        public string Name { get; set; } = string.Empty;
        public ObservableCollection<GraphEdgeVM> Edges { get; set; } = new ObservableCollection<GraphEdgeVM>();

        public override string ToString()
        {
            return Name;
        }
    }
}
