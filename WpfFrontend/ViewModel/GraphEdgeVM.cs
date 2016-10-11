﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfFrontend.ViewModel
{
    public class GraphEdgeVM
    {
        public string Name { get; set; } = string.Empty;
        public double Wage { get; set; } = 0.0;
        public GraphNodeVM Begin = null;
        public GraphNodeVM End = null;
    }
}
