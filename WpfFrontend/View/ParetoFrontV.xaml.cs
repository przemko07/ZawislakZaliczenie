using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfFrontend.Extensions;

namespace WpfFrontend.View
{
    /// <summary>
    /// Interaction logic for ParetoFrontV.xaml
    /// </summary>
    public partial class ParetoFrontV : UserControl, INotifyPropertyChanged
    {
        public class ParetoPointGeneration
        : ObjectVM
        {
            public ObservableCollection<Point> Points { get; } = new ObservableCollection<Point>();
            private Brush _Color = Brushes.Black;
            public Brush Color
            {
                get { return _Color; }
                set
                {
                    _Color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

        int setF1 = 0;
        public double[] F1
        {
            get { return (double[])GetValue(F1Property); }
            set { SetValue(F1Property, value); }
        }
        public static readonly DependencyProperty F1Property =
            DependencyProperty.Register("F1", typeof(double[]), typeof(ParetoFrontV), new PropertyMetadata(new double[0], F1Changed));
        private static void F1Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ParetoFrontV pfv = d as ParetoFrontV;
            ++pfv.setF1;
            pfv.CheckF();
        }

        int setF2 = 0;
        public double[] F2
        {
            get { return (double[])GetValue(F2Property); }
            set { SetValue(F2Property, value); }
        }
        public static readonly DependencyProperty F2Property =
            DependencyProperty.Register("F2", typeof(double[]), typeof(ParetoFrontV), new PropertyMetadata(new double[0], F2Changed));
        private static void F2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ParetoFrontV pfv = d as ParetoFrontV;
            ++pfv.setF2;
            pfv.CheckF();
        }

        int maxGen = 10;
        public ObservableCollection<ParetoPointGeneration> Generations { get; } = new ObservableCollection<ParetoPointGeneration>();
        public ObservableCollection<Point> ParetoFront { get; } = new ObservableCollection<Point>();

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

        private void CheckF()
        {
            if (setF1 != setF2) return;

            ParetoPointGeneration gen = new ParetoPointGeneration();
            for (int i = 0; i < F1.Length; i++)
            {
                gen.Points.Add(new Point(F1[i], F2[i]));
            }

            // pareto
            uint[] indicies = ParetoFrontFinder.FindParetoFront(F1, F2);
            ParetoFront.Clear();
            for (int i = 0; i < indicies.Length; i++)
            {
                ParetoFront.Add(new Point(F1[indicies[i]], F2[indicies[i]]));
            }

            // calc colors
            Generations.Add(gen);
            if (Generations.Count > maxGen) Generations.RemoveAt(0);

            for (int i = 0; i < Generations.Count; i++)
            {
                byte c = (byte)(255 - ((double)i + 1) / Generations.Count * 255);
                Generations[i].Color = new SolidColorBrush(Color.FromArgb(255, c, c, c));
            }

            RecalculateScale();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            RecalculateScale();
            base.OnRenderSizeChanged(sizeInfo);
        }

        public ParetoFrontV()
        {
            InitializeComponent();
            this.DataContext = this;
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
                    plot1 = Generations.SelectMany(n => n.Points).ToArray();
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

                valueToSet.x1 = xMax * 1.15; // +15%
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
