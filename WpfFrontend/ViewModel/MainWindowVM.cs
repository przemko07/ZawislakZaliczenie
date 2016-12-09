using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WpfFrontend.Extensions;
using WpfFrontend.Model;

namespace WpfFrontend.ViewModel
{
    public class MainWindowVM : ObjectVM
    {
        public enum GraphPathType
        {
            F1Path,
            F2Path,
            ParetoPath
        }

        private Engine engine = new Engine();

        private double _NodeSize;
        public double NodeSize
        {
            get { return _NodeSize; }
            set
            {
                _NodeSize = value;
                OnPropertyChanged(nameof(NodeSize));
                //Debug.WriteLine("NodeSize:" + (value / NodeSize));
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
                //Debug.WriteLine("NodesMargin:" + (value / NodeSize));
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
                //Debug.WriteLine("NamesMargin:" + (value / NodeSize));
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
                NodesMargin = value * 3.0;
                NamesMargin = value * 1.65;
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

        private bool _ShowGraphEdges = true;
        public bool ShowGraphEdges
        {
            get { return _ShowGraphEdges; }
            set
            {
                _ShowGraphEdges = value;
                OnPropertyChanged(nameof(ShowGraphEdges));
            }
        }

        private bool _Editable = false;
        public bool Editable
        {
            get { return _Editable; }
            set
            {
                _Editable = value;
                OnPropertyChanged(nameof(Editable));
            }
        }


        public MatrixVM Matrix1
        {
            get
            {
                return new MatrixVM(engine.Matrix1);
            }
        }


        public MatrixVM Matrix2
        {
            get
            {
                return new MatrixVM(engine.Matrix1);
            }
        }

        public double[] Fitness1
        {
            get
            {
                return engine.Evolutionary.Evo1.FitnessCalc.Fitness;
            }
        }

        public double[] Fitness2
        {
            get
            {
                return engine.Evolutionary.Evo2.FitnessCalc.Fitness;
            }
        }

        public uint BestIndex1
        {
            get
            {
                double min = int.MaxValue;
                uint minIndex = 0;
                for (uint i = 0; i < Fitness1.Length; i++)
                {
                    if (Fitness1[i] < min)
                    {
                        min = Fitness1[i];
                        minIndex = i;
                    }
                }
                return minIndex;
            }
        }

        public uint BestIndex2
        {
            get
            {
                double min = int.MaxValue;
                uint minIndex = 0;
                for (uint i = 0; i < Fitness2.Length; i++)
                {
                    if (Fitness2[i] < min)
                    {
                        min = Fitness2[i];
                        minIndex = i;
                    }
                }
                return minIndex;
            }
        }


        public Individual BestIndividual1
        {
            get
            {
                return engine.Evolutionary.Evo1.individuals[BestIndex1];
            }
        }

        public Individual BestIndividual2
        {
            get
            {
                return engine.Evolutionary.Evo2.individuals[BestIndex2];
            }
        }

        private GraphVM _Graph;
        public GraphVM Graph
        {
            get
            {
                if (_Graph == null)
                {
                    _Graph = GraphFactory.GenerateClique(engine.NodesCount);
                }
                return _Graph;
            }
            set
            {
                _Graph = value;
                OnPropertyChanged(nameof(Graph));
            }
        }

        private GraphPathType _GraphPath;
        public GraphPathType GraphPath
        {
            get { return _GraphPath; }
            set
            {
                _GraphPath = value;
                OnPropertyChanged(nameof(GraphPath));
            }
        }

        public GraphPathType[] GraphPathOptions
        {
            get
            {
                return new GraphPathType[]
                    {
                        GraphPathType.F1Path,
                        GraphPathType.F2Path,
                        GraphPathType.ParetoPath,
                    };
            }
        }


        private GraphVM _Graph1Path;
        public GraphVM Graph1Path
        {
            get
            {
                if (_Graph1Path == null)
                {
                    _Graph1Path = GraphFactory.GeneratePath(Graph, BestIndividual1);
                }
                return _Graph1Path;
            }
            set
            {
                _Graph1Path = value;
                OnPropertyChanged(nameof(Graph1Path));
            }
        }

        private GraphVM _Graph2Path;
        public GraphVM Graph2Path
        {
            get
            {
                if (_Graph2Path == null)
                {
                    _Graph2Path = GraphFactory.GeneratePath(Graph, BestIndividual2);
                }
                return _Graph2Path;
            }
            set
            {
                _Graph2Path = value;
                OnPropertyChanged(nameof(Graph2Path));
            }
        }


        public ObservableCollection<Point> Plot1 { get; } = new ObservableCollection<Point>();
        public ObservableCollection<Point> Plot2 { get; } = new ObservableCollection<Point>();

        public ActionCommand EvoStep
        {
            get
            {
                return new ActionCommand(() =>
                {
                    engine.Evolutionary.Step();
                    Graph1Path = null;
                    Graph2Path = null;
                    Plot1.Add(new Point(Plot1.Count, Fitness1[BestIndex1]));
                    Plot2.Add(new Point(Plot2.Count, Fitness2[BestIndex1]));
                });
            }
        }


        public MainWindowVM()
        {
            BindingOperations.EnableCollectionSynchronization(Plot1, Plot1);
            BindingOperations.EnableCollectionSynchronization(Plot2, Plot2);

        }
    }
}
