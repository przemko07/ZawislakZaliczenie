using Microsoft.Win32;
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
using TSP.Extensions;
using TSP.Model;
using TSP.View;

namespace TSP.ViewModel
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
            }
        }

        public double[] Fitness2
        {
            get
            {
                return engine.Evolutionary.Fitness2;
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
                    RefreshView();
                });
            }
        }


        public Individual BestIndividual1
        {
            get
            {
                return engine.Evolutionary.Individuals[BestIndex1];
            }
        }

        public Individual BestIndividual2
        {
            get
            {
                return engine.Evolutionary.Individuals[BestIndex2];
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
                    try
                    {
                        _ParetoPath = new ObservableCollection<GraphPathVM>(ParetoIncidies
                            .Select(n => GraphFactory.GeneratePath(Graph, engine.Evolutionary.Individuals[n])));
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e);
                    }
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
                if (ParetoPath != null && _SelectedParetoPath == null && SelectedParetoIndex < ParetoPath.Count)
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
                return new ActionCommand((o) =>
                {
                    bool refresh = true;
                    if (o is bool) refresh = !(bool)o;
                    engine.Evolutionary.Step();
                    Plot1.Add(new Point(Plot1.Count, Fitness1[BestIndex1]));
                    Plot2.Add(new Point(Plot2.Count, Fitness2[BestIndex1]));

                    if (!refresh) return;

                    RefreshView();
                });
            }
        }

        private void RefreshView()
        {
            _Graph = null;
            OnPropertyChanged(nameof(Fitness1));
            OnPropertyChanged(nameof(Fitness2));
            OnPropertyChanged(nameof(BestIndex1));
            OnPropertyChanged(nameof(BestIndex2));
            OnPropertyChanged(nameof(BestIndividual1));
            OnPropertyChanged(nameof(BestIndividual2));
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

        private int _CurrentStep = 0;
        public int CurrentStep
        {
            get { return _CurrentStep; }
            set
            {
                _CurrentStep = value;
                OnPropertyChanged(nameof(CurrentStep));
            }
        }


        bool stops = false;
        public ActionCommand EvoMultiSteps
        {
            get
            {
                return new ActionCommand(() =>
                {
                    if (MultiStepsCount > 20)
                    {
                        var opti = MessageBox.Show("This ammount of steps will slow down application. Do you want to optimalize view part?", "Optimalize", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        engine.Optimalize = opti == MessageBoxResult.Yes;
                    }
                    Thread.Sleep(100);
                    Task.Run(() =>
                    {
                        CurrentStep = 0;
                        for (int i = 0; i < MultiStepsCount - 1; i++)
                        {
                            ++CurrentStep;
                            if (stops) { stops = false; return; }
                            Thread.Sleep(50);
                            EvoStep.Execute(engine.Optimalize);
                        }
                        ++CurrentStep;
                        if (stops) { stops = false; return; }
                        EvoStep.Execute(null);
                    });
                });
            }
        }

        public ActionCommand StopMultiSteps
        {
            get
            {
                return new ActionCommand(() =>
                {
                    stops = true;
                });
            }
        }

        public ActionCommand SaveCommand
        {
            get
            {
                return new ActionCommand(() =>
                {

                    SaveFileDialog ofd = new SaveFileDialog();
                    ofd.Filter = "Graphs settings (*.xml)|*.xml|All Files (*.*)|*.*";
                    if (ofd.ShowDialog() == true && ofd.FileName != string.Empty)
                    {
                        try
                        {
                            SettingsXml s = new SettingsXml()
                            {
                                F1 = engine.Matrix1,
                                F2 = engine.Matrix2,
                                Individuals = engine.Evolutionary.Individuals,
                                PopSize = (uint)engine.Evolutionary.Individuals.Length,
                                Mutation = engine.Mutation,
                            };

                            SettingsXml.Write(s, ofd.FileName);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show($"Cant save file->{e}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                });
            }
        }

        public ActionCommand LoadCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    LoadSettingsV v = new LoadSettingsV();
                    v.engine = engine;
                    v.OnLoadCommand = new Action(() =>
                    {
                        RefreshView();
                    });
                    v.Show();
                });
            }
        }

        public ActionCommand InfoCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    InfoV info = new InfoV();
                    info.Show();
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
