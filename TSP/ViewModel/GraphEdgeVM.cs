using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP.ViewModel
{
    public class GraphEdgeVM
    {
        public string Name { get; set; } = null;
        public GraphNodeVM Begin { get; set; } = null;
        public GraphNodeVM End { get; set; } = null;

        public override string ToString()
        {
            if (Name == null) return string.Format("{0}-{1}", Begin, End);
            return Name;
        }
    }
}
