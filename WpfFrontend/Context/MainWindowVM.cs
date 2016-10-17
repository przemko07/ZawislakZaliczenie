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

                Combine = Math.Min(WindowWidth, WindowHeight) / 6;
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

                Combine = Math.Min(WindowWidth, WindowHeight) / 6;
            }
        }


        private GraphVM _Graph;
        public GraphVM Graph
        {
            get
            {
                if (_Graph == null)
                {
                    _Graph = new GraphVM();

                    for (int i = 0; i < 10; i++)
                    {
                        _Graph.Nodes.Add(new GraphNodeVM() { Name = i.ToString() });
                    }

                    Random random = new Random();

                    for (int i = 0; i < 20; i++)
                    {
                        int i1 = random.Next(0, _Graph.Nodes.Count);
                        int i2 = random.Next(0, _Graph.Nodes.Count);
                        _Graph.Edges.Add(new GraphEdgeVM()
                        {
                            Name = i1 + "-" + i2,
                            Begin = _Graph.Nodes[i1],
                            End = _Graph.Nodes[i2],
                        });
                    }
                    GraphFactory.FillEdges(_Graph);
                }

                return _Graph;
            }
        }

        private GraphVM _GraphPath;
        public GraphVM GraphPath
        {
            get
            {
                if (_GraphPath == null)
                {
                    _GraphPath = new GraphVM();
                    Random random = new Random();
                    GraphNodeVM node = Graph.Nodes[random.Next(Graph.Nodes.Count)];
                    for (int i = 0; i < 4; i++)
                    {
                        GraphEdgeVM edge = node.Edges.Where(n=>n.Begin != node).OrderByDescending(n => n.Begin.Edges.Count).FirstOrDefault();
                        if (edge == null) break;
                        _GraphPath.Edges.Add(edge);
                        node = edge.Begin;
                    }
                }

                return _GraphPath;

            }
        }
    }
}
