using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfFrontend.View
{
    public partial class XYPlotV
    : UserControl
    , INotifyPropertyChanged
    {
        public class LineModel
        {
            public Point Start { get; set; }
            public Point End { get; set; }
        }

        public ObservableCollection<Point> Plot1
        {
            get { return (ObservableCollection<Point>)GetValue(Plot1Property); }
            set { SetValue(Plot1Property, value); }
        }
        public static readonly DependencyProperty Plot1Property =
            DependencyProperty.Register("Plot1", typeof(ObservableCollection<Point>), typeof(XYPlotV), new PropertyMetadata(new ObservableCollection<Point>(), Plot1ChangedCallback));
        private static void Plot1ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d as XYPlotV) == null) return;
            (d as XYPlotV).RecalculateScale();
            (d as XYPlotV).Plot1_Changed();
            (d as XYPlotV).Plot1.CollectionChanged += (d as XYPlotV).Plot1_CollectionChanged;
        }

        public ObservableCollection<Point> Plot2
        {
            get { return (ObservableCollection<Point>)GetValue(Plot2Property); }
            set { SetValue(Plot2Property, value); }
        }
        public static readonly DependencyProperty Plot2Property =
            DependencyProperty.Register("Plot2", typeof(ObservableCollection<Point>), typeof(XYPlotV), new PropertyMetadata(new ObservableCollection<Point>(), Plot2ChangedCallback));
        private static void Plot2ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d as XYPlotV) == null) return;
            (d as XYPlotV).RecalculateScale();
            (d as XYPlotV).Plot2_Changed();
            (d as XYPlotV).Plot2.CollectionChanged += (d as XYPlotV).Plot2_CollectionChanged;
        }

        private Dictionary<ObservableCollection<Point>, ObservableCollection<LineModel>> Plots1Lines = new Dictionary<ObservableCollection<Point>, ObservableCollection<LineModel>>();
        private Dictionary<ObservableCollection<Point>, ObservableCollection<LineModel>> Plots2Lines = new Dictionary<ObservableCollection<Point>, ObservableCollection<LineModel>>();

        private ObservableCollection<LineModel> _Plot1Lines;
        public ObservableCollection<LineModel> Plot1Lines
        {
            get { return _Plot1Lines; }
            set
            {
                _Plot1Lines = value;
                OnPropertyChanged(nameof(Plot1Lines));
            }
        }

        private ObservableCollection<LineModel> _Plot2Lines;
        public ObservableCollection<LineModel> Plot2Lines
        {
            get { return _Plot2Lines; }
            set
            {
                _Plot2Lines = value;
                OnPropertyChanged(nameof(Plot2Lines));
            }
        }

        private double _X0;
        public double X0
        {
            get { return _X0; }
            set
            {
                _X0 = value;
                OnPropertyChanged(nameof(X0));
            }
        }

        private double _X1;
        public double X1
        {
            get { return _X1; }
            set
            {
                _X1 = value;
                OnPropertyChanged(nameof(X1));
            }
        }

        private double _Y0;
        public double Y0
        {
            get { return _Y0; }
            set
            {
                _Y0 = value;
                OnPropertyChanged(nameof(Y0));
            }
        }

        private double _Y1;
        public double Y1
        {
            get { return _Y1; }
            set
            {
                _Y1 = value;
                OnPropertyChanged(nameof(Y1));
            }
        }

        private double _ScaleX;
        public double ScaleX
        {
            get { return _ScaleX; }
            set
            {
                _ScaleX = value;
                OnPropertyChanged(nameof(ScaleX));
            }
        }

        private double _ScaleY;
        public double ScaleY
        {
            get { return _ScaleY; }
            set
            {
                _ScaleY = value;
                OnPropertyChanged(nameof(ScaleY));
            }
        }

        private double _RawMouseX = double.NaN;
        public double RawMouseX
        {
            get { return _RawMouseX; }
            set
            {
                _RawMouseX = value;
                OnPropertyChanged(nameof(RawMouseX));
            }
        }

        private double _RawMouxeY = double.NaN;
        public double RawMouseY
        {
            get { return _RawMouxeY; }
            set
            {
                _RawMouxeY = value;
                OnPropertyChanged(nameof(RawMouseY));
            }
        }

        private double _MouseX = double.NaN;
        public double MouseX
        {
            get { return _MouseX; }
            set
            {
                _MouseX = value;
                OnPropertyChanged(nameof(MouseX));
            }
        }

        private double _MouxeY = double.NaN;
        public double MouseY
        {
            get { return _MouxeY; }
            set
            {
                _MouxeY = value;
                OnPropertyChanged(nameof(MouseY));
            }
        }

        private double _MousePlotX = double.NaN;
        public double MousePlotX
        {
            get { return _MousePlotX; }
            set
            {
                _MousePlotX = value;
                OnPropertyChanged(nameof(MousePlotX));
            }
        }


        private double _MousePlot1 = double.NaN;
        public double MousePlot1
        {
            get { return _MousePlot1; }
            set
            {
                _MousePlot1 = value;
                OnPropertyChanged(nameof(MousePlot1));
            }
        }

        private double _MousePlot2 = double.NaN;
        public double MousePlot2
        {
            get { return _MousePlot2; }
            set
            {
                _MousePlot2 = value;
                OnPropertyChanged(nameof(MousePlot2));
            }
        }



        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            RecalculateScale();
        }


        public XYPlotV()
        {
            BindingOperations.EnableCollectionSynchronization(Plots1Lines, Plots1Lines);
            BindingOperations.EnableCollectionSynchronization(Plots2Lines, Plots2Lines);

            InitializeComponent();
            SizeChanged += XYPlotV_SizeChanged;
        }


        private void XYPlotV_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            X1 = X0;
            RecalculateScale();
        }

        private void Plots_MouseMove(object sender, MouseEventArgs e)
        {
            if (ScaleX == 0 || ScaleY == 0 || Plot1.Count == 0 || Plot2.Count == 0) return;

            Point pos = e.GetPosition(this);
            RawMouseX = pos.X;
            RawMouseY = Plots.Height - pos.Y; // I know its wrong, but at least i can see stuff

            MouseX = pos.X / ScaleX;
            MouseY = Y1 - pos.Y / ScaleY;
            int index = (int)(MouseX);
            if (index < Plot1.Count)
            {
                MousePlotX = index;
                MousePlot1 = Plot1[index].Y;
                MousePlot2 = Plot2[index].Y;
            }
            else
            {
                MousePlotX = double.NaN;
                MousePlot1 = double.NaN;
                MousePlot2 = double.NaN;
            }
        }

        private void Plot1_Changed()
        {
            ObservableCollection<Point> plot1 = null;
            Dictionary<ObservableCollection<Point>, ObservableCollection<LineModel>> plots1Lines = null;
            try
            {
                Dispatcher.Invoke(() =>
                {
                    plot1 = Plot1;
                    plots1Lines = Plots1Lines;
                });
            }
            catch { }

            if (!Plots1Lines.ContainsKey(plot1))
            {
                var lines = new ObservableCollection<LineModel>();
                BindingOperations.EnableCollectionSynchronization(lines, lines);
                plots1Lines.Add(plot1, lines);
            }
            Plot1Lines = Plots1Lines[plot1];
        }

        private void Plot2_Changed()
        {
            ObservableCollection<Point> plot2 = null;
            Dictionary<ObservableCollection<Point>, ObservableCollection<LineModel>> plots2Lines = null;
            try
            {
                Dispatcher.Invoke(() =>
                {
                    plot2 = Plot2;
                    plots2Lines = Plots2Lines;
                });
            }
            catch { }

            if (!Plots2Lines.ContainsKey(plot2))
            {
                var lines = new ObservableCollection<LineModel>();
                BindingOperations.EnableCollectionSynchronization(lines, lines);
                Plots2Lines.Add(plot2, lines);
            }
            Plot2Lines = Plots2Lines[plot2];
        }

        private void Plot1_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Point> col = sender as ObservableCollection<Point>;
            if (col == null) return;

            ObservableCollection<Point> plot1 = null;
            ObservableCollection<Point> plot2 = null;
            ObservableCollection<LineModel> plot1Lines = null;

            bool same = false;

            try
            {
                Dispatcher.Invoke(() =>
                {
                    plot1 = Plot1;
                    plot2 = Plot2;
                    plot1Lines = Plot1Lines;
                    same = plot1 == col;
                });
            }
            catch { }
            if (!same) return;

            for (int i = plot1Lines.Count; i < plot1.Count - 1; i++)
            {
                plot1Lines.Add(new LineModel() { Start = plot1[i], End = plot1[i + 1] });
            }


            RecalculateScale();
        }

        private void Plot2_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Point> col = sender as ObservableCollection<Point>;
            if (col == null) return;

            ObservableCollection<Point> plot1 = null;
            ObservableCollection<Point> plot2 = null;
            ObservableCollection<LineModel> plot2Lines = null;
            bool same = false;

            try
            {
                Dispatcher.Invoke(() =>
                {
                    plot1 = Plot1;
                    plot2 = Plot2;
                    plot2Lines = Plot2Lines;
                    same = plot2 == col;
                });
            }
            catch { }
            if (!same) return;

            for (int i = plot2Lines.Count; i < plot2.Count - 1; i++)
            {
                plot2Lines.Add(new LineModel() { Start = plot2[i], End = plot2[i + 1] });
            }

            RecalculateScale();
        }

        public void SetValues(double x0, double y0, double x1, double y1, double scaleX, double scaleY)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    X0 = x0;
                    Y0 = y0;
                    X1 = x1;
                    Y1 = y1;
                    ScaleX = scaleX;
                    ScaleY = scaleY;
                });
            }
            catch { }
        }

        private void RecalculateScale()
        {
            Point[] plot1 = new Point[0];
            Point[] plot2 = new Point[0];
            double actualWidth = 0;
            double actualHeight = 0;

            try
            {
                Dispatcher.Invoke(() =>
                {
                    plot1 = Plot1.ToArray();
                    plot2 = Plot2.ToArray();
                    actualWidth = Plots.ActualWidth;
                    actualHeight = Plots.ActualHeight;
                });
            }
            catch { }

            ValueToSet valueToSet = RecalculateScale(plot1, plot2, actualWidth, actualHeight, X1, Y1);

            try
            {
                if (!valueToSet.setXY && !valueToSet.setScale) return;

                Dispatcher.Invoke(() =>
                {
                    if (valueToSet.setXY)
                    {
                        X0 = valueToSet.x0;
                        Y0 = valueToSet.y0;
                        X1 = valueToSet.x1;
                        Y1 = valueToSet.y1;
                    }

                    if (valueToSet.setScale)
                    {
                        ScaleX = valueToSet.scaleX;
                        ScaleY = valueToSet.scaleY;
                    }
                });
            }
            catch { }
        }

        class ValueToSet
        {
            public bool setXY;
            public double x0, y0, x1, y1;
            public bool setScale;
            public double scaleX, scaleY;
        }

        private static ValueToSet RecalculateScale(Point[] plot1, Point[] plot2, double actualWidth, double actualHeight, double currentX1, double currentY1)
        {
            ValueToSet valueToSet = new ValueToSet();
            if (plot1.Length != 0 || plot2.Length != 0)
            {
                valueToSet.setXY = true;
                valueToSet.x0 = 0;
                valueToSet.y0 = 0;

                var xMax = double.NegativeInfinity;
                var yMax = double.NegativeInfinity;
                if (plot1.Any())
                {
                    xMax = plot1.Max(n => n.X);
                    yMax = plot1.Max(n => n.Y);
                }
                if (plot2.Any())
                {
                    xMax = Math.Max(xMax, plot2.Max(n => n.X));
                    yMax = Math.Max(yMax, plot2.Max(n => n.Y));
                }

                valueToSet.x1 = xMax * 2; // +100%
                valueToSet.y1 = yMax * 1.15; // +15%

                if (xMax < currentX1) valueToSet.setXY = false;
                if (valueToSet.y1 != currentY1) valueToSet.setXY = true;
            }

            if (valueToSet.setXY
                && (valueToSet.x1 != valueToSet.x0 && valueToSet.y1 != valueToSet.y0)
                && (!double.IsNaN(actualWidth) && !double.IsNaN(actualHeight))
                && (actualWidth != 0 && actualHeight != 0))
            {
                valueToSet.setScale = true;
                valueToSet.scaleX = actualWidth / (valueToSet.x1 - valueToSet.x0);
                valueToSet.scaleY = actualHeight / (valueToSet.y1 - valueToSet.y0);
            }

            return valueToSet;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
