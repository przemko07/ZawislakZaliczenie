using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfFrontend.Extensions;
using WpfFrontend.Model;
using WpfFrontend.ViewModel;

namespace WpfFrontend.Context
{
    public class MainWindowVM : ObjectVM
    {
        public string text { get; } = "Ala ma kota";

        private double _NodeSize;
        public double NodeSize
        {
            get { return _NodeSize; }
            set
            {
                _NodeSize = value;
                OnPropertyChanged(nameof(NodeSize));
            }
        }

        private double _NodesMargin;
        public double NodesMargin
        {
            get { return _NodesMargin; }
            set
            {
                _NodesMargin = value;
                OnPropertyChanged(nameof(NodesMargin));
            }
        }

        private double _NamesMargin;
        public double NamesMargin
        {
            get { return _NamesMargin; }
            set
            {
                _NamesMargin = value;
                OnPropertyChanged(nameof(NamesMargin));
            }
        }

        private double _Combine;
        public double Combine
        {
            get { return _Combine; }
            set
            {
                _Combine = value;
                OnPropertyChanged(nameof(Combine));

                NodeSize = value;
                NodesMargin = value * 2;
                NamesMargin = value * 0.45;
            }
        }

        private double _WindowWidth;
        public double WindowWidth
        {
            get { return _WindowWidth; }
            set
            {
                _WindowWidth = value;
                OnPropertyChanged(nameof(WindowWidth));

                Combine = Math.Min(WindowWidth, WindowHeight) / (Graph.Nodes.Count);
            }
        }

        private double _WindowHeight;
        public double WindowHeight
        {
            get { return _WindowHeight; }
            set
            {
                _WindowHeight = value;
                OnPropertyChanged(nameof(WindowHeight));

                Combine = Math.Min(WindowWidth, WindowHeight) / (Graph.Nodes.Count);
            }
        }
        
        Permutacja p = new Permutacja(1, 10, true);

        private GraphVM _Graph;
        public GraphVM Graph
        {
            get
            {
                if (_Graph == null) _Graph = GraphFactory.GenerateClique(p);

                return _Graph;
            }
        }

        private GraphVM _GraphPath;
        public GraphVM GraphPath
        {
            get
            {
                if (_GraphPath == null) _GraphPath = GraphFactory.GeneratePath(Graph, p);

                return _GraphPath;

            }
        }
    }
}
