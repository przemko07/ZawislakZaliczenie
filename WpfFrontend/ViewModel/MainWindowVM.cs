using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WpfFrontend.Extensions;
using WpfFrontend.Model;
using WpfFrontend.View;

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
        private ParetoFrontFinder finder = new ParetoFrontFinder();

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

                NodeSize = value * 0.6;
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

        private bool _ShowGraphEdges = false;
        public bool ShowGraphEdges
        {
            get { return _ShowGraphEdges; }
            set
            {
                _ShowGraphEdges = value;
                OnPropertyChanged(nameof(ShowGraphEdges));
            }
        }

        public double[] Fitness1
        {
            get
            {
                return engine.Evolutionary.Fitness1;
                return engine.Evolutionary.Evo1.FitnessCalc.Fitness;
            }
        }

        public double[] Fitness2
        {
            get
            {
                return engine.Evolutionary.Fitness2;
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

        public ActionCommand ShowSettings
        {
            get
            {
                return new ActionCommand(() =>
                {
                    new SettingsV()
                    {
                        EvoEngine = engine,
                        Width = 800,
                        Height = 640,
                    }.ShowDialog();
                    _Graph = null;
                    EvoStep.Execute(null);
                });
            }
        }


        public Individual BestIndividual1
        {
            get
            {
                return engine.Evolutionary.Individuals[BestIndex1];
                return engine.Evolutionary.Evo1.individuals[BestIndex1];
            }
        }

        public Individual BestIndividual2
        {
            get
            {
                return engine.Evolutionary.Individuals[BestIndex2];
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
                _Graph.Name = "Graph";
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

                switch (GraphPath)
                {
                    case GraphPathType.F1Path:
                        SelectedIndex = BestIndex1;
                        break;
                    case GraphPathType.F2Path:
                        SelectedIndex = BestIndex2;
                        break;
                    case GraphPathType.ParetoPath:
                        SelectedIndex = ParetoIncidies[SelectedParetoIndex];
                        break;
                }
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

        private uint[] _ParetoIncidies = new uint[0];
        public uint[] ParetoIncidies
        {
            get { return _ParetoIncidies; }
            set
            {
                _ParetoIncidies = value;
                OnPropertyChanged(nameof(ParetoIncidies));

                MaxParetoIndex = (uint)(ParetoIncidies.Length - 1);
                _ParetoPath = null;
                _SelectedParetoPath = null;
                OnPropertyChanged(nameof(SelectedParetoPath));
            }
        }

        private uint _MaxParetoIndex = 0;
        public uint MaxParetoIndex
        {
            get { return _MaxParetoIndex; }
            set
            {
                _MaxParetoIndex = value;
                OnPropertyChanged(nameof(MaxParetoIndex));
            }
        }

        private ObservableCollection<GraphPathVM> _ParetoPath = null;
        public ObservableCollection<GraphPathVM> ParetoPath
        {
            get
            {
                if (_ParetoPath == null)
                {
                    _ParetoPath = new ObservableCollection<GraphPathVM>(ParetoIncidies
                        .Select(n => GraphFactory.GeneratePath(Graph, engine.Evolutionary.Individuals[n])));
                }
                return _ParetoPath;
            }
        }


        private uint _SelectedParetoIndex = 0;
        public uint SelectedParetoIndex
        {
            get { return _SelectedParetoIndex; }
            set
            {
                _SelectedParetoIndex = value;
                OnPropertyChanged(nameof(SelectedParetoIndex));
                if (value >= ParetoIncidies.Count()) value = (uint)(ParetoIncidies.Length - 1);
                SelectedParetoPath = ParetoPath[(int)value];
                SelectedIndex = ParetoIncidies[value];
            }
        }

        private GraphPathVM _SelectedParetoPath = null;
        public GraphPathVM SelectedParetoPath
        {
            get
            {
                if (_SelectedParetoPath == null && SelectedParetoIndex < ParetoPath.Count)
                {
                    _SelectedParetoPath = ParetoPath[(int)SelectedParetoIndex];
                    _SelectedParetoPath.Name = "_SelectedParetoPath";
                }
                return _SelectedParetoPath;
            }
            set
            {
                _SelectedParetoPath = value;
                OnPropertyChanged(nameof(SelectedParetoPath));
            }
        }

        private GraphPathVM _Graph1Path;
        public GraphPathVM Graph1Path
        {
            get
            {
                if (_Graph1Path == null)
                {
                    _Graph1Path = GraphFactory.GeneratePath(Graph, BestIndividual1);
                }
                _Graph1Path.Name = "_Graph1Path";
                return _Graph1Path;
            }
            set
            {
                _Graph1Path = value;
                OnPropertyChanged(nameof(Graph1Path));
            }
        }

        private GraphPathVM _Graph2Path;
        public GraphPathVM Graph2Path
        {
            get
            {
                if (_Graph2Path == null)
                {
                    _Graph2Path = GraphFactory.GeneratePath(Graph, BestIndividual2);
                }
                _Graph2Path.Name = "_Graph2Path";
                return _Graph2Path;
            }
            set
            {
                _Graph2Path = value;
                OnPropertyChanged(nameof(Graph2Path));
            }
        }

        private uint _SelectedIndex = 0;
        public uint SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
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
                    Plot1.Add(new Point(Plot1.Count, Fitness1[BestIndex1]));
                    Plot2.Add(new Point(Plot2.Count, Fitness2[BestIndex1]));
                    OnPropertyChanged(nameof(Fitness1));
                    OnPropertyChanged(nameof(Fitness2));
                    OnPropertyChanged(nameof(BestIndex1));
                    OnPropertyChanged(nameof(BestIndex2));
                    OnPropertyChanged(nameof(BestIndividual1));
                    OnPropertyChanged(nameof(BestIndividual2));
                    Graph1Path = null; // in a getter im getting the current best
                    Graph2Path = null; // in a getter im getting the current best


                    switch (GraphPath)
                    {
                        case GraphPathType.F1Path:
                            SelectedIndex = BestIndex1;
                            break;
                        case GraphPathType.F2Path:
                            SelectedIndex = BestIndex2;
                            break;
                        case GraphPathType.ParetoPath:
                            SelectedIndex = ParetoIncidies[SelectedParetoIndex];
                            break;
                    }
                });
            }
        }

        private int _MultiStepsCount = 10;
        public int MultiStepsCount
        {
            get { return _MultiStepsCount; }
            set
            {
                _MultiStepsCount = value;
                OnPropertyChanged(nameof(MultiStepsCount));
            }
        }


        public ActionCommand EvoMultiSteps
        {
            get
            {
                return new ActionCommand(() =>
                {
                    Thread.Sleep(100);
                    Task.Run(() =>
                    {
                        for (int i = 0; i < MultiStepsCount; i++)
                        {
                            Thread.Sleep(50);
                            EvoStep.Execute(null);
                        }
                    });
                });
            }
        }

        BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public MainWindowVM()
        {
            BindingOperations.EnableCollectionSynchronization(Plot1, Plot1);
            BindingOperations.EnableCollectionSynchronization(Plot2, Plot2);
            EvoStep.Execute(null);
        }
    }
}
