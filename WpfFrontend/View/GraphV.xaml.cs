using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using WpfFrontend.Model;
using WpfFrontend.ViewModel;

namespace WpfFrontend.View
{
    /// <summary>
    /// Interaction logic for GraphV.xaml
    /// </summary>
    public partial class GraphV : UserControl, INotifyPropertyChanged
    {
        private CircleGraphPositioner positioner = null;

        public GraphVM Graph
        {
            get { return (GraphVM)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }
        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register("Graph", typeof(GraphVM), typeof(GraphV), new PropertyMetadata(null, GraphChangedCallback));
        private static void GraphChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            GraphV graphV = d as GraphV;
            if (graphV == null) return;


            graphV.Nodes.Clear();
            graphV.Edges.Clear();

            if (graphV.Graph == null) return;

            graphV.positioner = new CircleGraphPositioner(graphV.Graph);


            try
            {
                foreach (var node in graphV.Graph.Nodes)
                {
                    graphV.Nodes[node] = graphV.CalculatePosition(node, graphV.NodesMargin);
                    graphV.NodesNames[node] = graphV.CalculatePosition(node, graphV.NamesMargin);
                }

                foreach (var edge in graphV.Graph.Edges)
                {
                    graphV.Edges.Add(edge);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            ++graphV.ChangeCount;
        }

        public double NodesMargin
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }
        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("NodesMargin", typeof(double), typeof(GraphV), new PropertyMetadata(100.0, NodesMarginChangedCallback));
        private static void NodesMarginChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            GraphV graphV = d as GraphV;
            if (graphV == null) return;

            graphV.GenerateNodePositions();
        }

        public double NamesMargin
        {
            get { return (double)GetValue(NamesMarginProperty); }
            set { SetValue(NamesMarginProperty, value); }
        }
        public static readonly DependencyProperty NamesMarginProperty =
            DependencyProperty.Register("NamesMargin", typeof(double), typeof(GraphV), new PropertyMetadata(50.0, NamesMarginChangedCallback));
        private static void NamesMarginChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            GraphV graphV = d as GraphV;
            if (graphV == null) return;

            graphV.GenerateNodePositions();
        }

        public double NodeSize
        {
            get { return (double)GetValue(NodeSizeProperty); }
            set { SetValue(NodeSizeProperty, value); }
        }
        public static readonly DependencyProperty NodeSizeProperty =
            DependencyProperty.Register("NodeSize", typeof(double), typeof(GraphV), new PropertyMetadata(20.0, NodeSizeChanged));
        private static void NodeSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            GraphV graphV = d as GraphV;
            if (graphV == null) return;

            graphV.GenerateNodePositions();
        }


        public ObservableDictionary<GraphNodeVM, Point> NodesNames { get; } = new ObservableDictionary<GraphNodeVM, Point>();
        public ObservableDictionary<GraphNodeVM, Point> Nodes { get; } = new ObservableDictionary<GraphNodeVM, Point>();
        public ObservableCollection<GraphEdgeVM> Edges { get; } = new ObservableCollection<GraphEdgeVM>();
        private int _ChangeCount = 0;
        public int ChangeCount
        {
            get { return _ChangeCount; }
            set
            {
                _ChangeCount = value;
                OnPropertyChanged(nameof(ChangeCount));
            }
        }
        

        public GraphV()
        {
            InitializeComponent();
            SizeChanged += GraphV_SizeChanged;
        }


        private void GraphV_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GenerateNodePositions();
        }

        private void GenerateNodePositions()
        {
            try
            {
                foreach (var node in Nodes.Keys.ToArray())
                {
                    Nodes[node] = CalculatePosition(node, NodesMargin);
                    NodesNames[node] = CalculatePosition(node, NamesMargin);
                }

                // doesnt need to generate new edges
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            ++ChangeCount;
        }

        private Point CalculatePosition(GraphNodeVM node, double margin)
        {
            double width = ActualWidth - margin;
            double height = ActualHeight - margin;
            double rx = width / 2;
            double ry = height / 2;
            double cx = width / 2;
            double cy = height / 2;
            if (rx < 10) rx = 10; // I just want to see anything, uglynees is not important
            if (ry < 10) ry = 10; // I just want to see anything, uglynees is not important

            var newPos = positioner.Position(node, rx, ry, cx, ry);
            newPos.X += margin / 2;
            newPos.Y += margin / 2;
            return newPos;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
