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
using System.Windows.Media.Animation;
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
        private uint animTimeSpan = 1500;
        Storyboard storyBoard = null;
        private CircleGraphPositioner positioner = null;


        #region dependency property

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
                    graphV.Nodes[node] = graphV.CalculatePosition(node, graphV.NodesMargin, graphV.ActualWidth, graphV.ActualHeight);
                    graphV.NodesNames[node] = graphV.CalculatePosition(node, graphV.NamesMargin, graphV.ActualWidth, graphV.ActualHeight);
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

        public GraphVM GraphPath
        {
            get { return (GraphVM)GetValue(GraphPathProperty); }
            set { SetValue(GraphPathProperty, value); }
        }
        public static readonly DependencyProperty GraphPathProperty =
            DependencyProperty.Register("GraphPath", typeof(GraphVM), typeof(GraphV), new PropertyMetadata(null, GraphPathChangedCallback));
        private static void GraphPathChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            GraphV graphV = d as GraphV;
            if (graphV == null) return;

            graphV.EdgesPath.Clear();
            graphV.NodesPathOrder.Clear();

            if (graphV.GraphPath == null) return;

            int index = 0;
            graphV.NodesPathOrder.Add(graphV.GraphPath.Edges.First().Begin, index++);
            foreach (var edge in graphV.GraphPath.Edges)
            {
                graphV.EdgesPath.Add(edge);
                graphV.NodesPathOrder.Add(edge.End, index++);
            }
            ++graphV.ChangeCount;

            graphV.SetAnimation();
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

            graphV.GenerateNodePositions(graphV.ActualWidth, graphV.ActualHeight);
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

            graphV.GenerateNodePositions(graphV.ActualWidth, graphV.ActualHeight);
        }

        public double NodeSize
        {
            get { return (double)GetValue(NodeSizeProperty); }
            set { SetValue(NodeSizeProperty, value); }
        }
        public static readonly DependencyProperty NodeSizeProperty =
            DependencyProperty.Register("NodeSize", typeof(double), typeof(GraphV), new PropertyMetadata(20.0, NodeSizeChangedCallback));
        private static void NodeSizeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            GraphV graphV = d as GraphV;
            if (graphV == null) return;

            graphV.GenerateNodePositions(graphV.ActualWidth, graphV.ActualHeight);
        }

        public bool ShowGraphEdges
        {
            get { return (bool)GetValue(ShowGraphEdgesProperty); }
            set { SetValue(ShowGraphEdgesProperty, value); }
        }
        public static readonly DependencyProperty ShowGraphEdgesProperty =
            DependencyProperty.Register("ShowGraphEdges", typeof(bool), typeof(GraphV), new PropertyMetadata(true, ShowGraphEdgesChangedCallback));
        private static void ShowGraphEdgesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            GraphV graphV = d as GraphV;
            if (graphV == null) return;

            graphV.OnPropertyChanged(nameof(ShowGraphEdges));
        }

        public bool ShowNodeNames
        {
            get { return (bool)GetValue(ShowNamesProperty); }
            set { SetValue(ShowNamesProperty, value); }
        }
        public static readonly DependencyProperty ShowNamesProperty =
            DependencyProperty.Register("ShowNodeNames", typeof(bool), typeof(GraphV), new PropertyMetadata(true, ShowNamesChangedCallback));
        private static void ShowNamesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            GraphV graphV = d as GraphV;
            if (graphV == null) return;

            graphV.OnPropertyChanged(nameof(ShowNodeNames));
        }

        #endregion

        public ObservableDictionary<GraphNodeVM, int> NodesPathOrder { get; } = new ObservableDictionary<GraphNodeVM, int>();
        public ObservableCollection<GraphEdgeVM> EdgesPath { get; } = new ObservableCollection<GraphEdgeVM>();
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


        private void SetAnimation()
        {
            storyBoard?.Stop(Selected);

            if ((EdgesPath?.Count ?? 0) == 0) return;

            storyBoard = new Storyboard();
            DoubleAnimationUsingKeyFrames leftAnimation = new DoubleAnimationUsingKeyFrames();
            DoubleAnimationUsingKeyFrames topAnimation = new DoubleAnimationUsingKeyFrames();
            leftAnimation.RepeatBehavior = RepeatBehavior.Forever;
            topAnimation.RepeatBehavior = RepeatBehavior.Forever;

            Storyboard.SetTargetName(leftAnimation, Selected.Name);
            Storyboard.SetTargetProperty(leftAnimation, new PropertyPath("(0)", Canvas.LeftProperty));

            Storyboard.SetTargetName(topAnimation, Selected.Name);
            Storyboard.SetTargetProperty(topAnimation, new PropertyPath("(0)", Canvas.TopProperty));


            double sumDist = EdgesPath.Sum(edge => Distance(edge));
            double sumTime = 0;

            foreach (var edge in EdgesPath)
            {
                leftAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(Nodes[edge.Begin].X, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, (int)sumTime))));
                topAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(Nodes[edge.Begin].Y, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, (int)sumTime))));

                sumTime += (Distance(edge) / sumDist) * (animTimeSpan * EdgesPath.Count);

                leftAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(Nodes[edge.End].X, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, (int)sumTime))));
                topAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(Nodes[edge.End].Y, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, (int)sumTime))));
            }

            leftAnimation.Freeze();
            topAnimation.Freeze();
            storyBoard.Children.Add(leftAnimation);
            storyBoard.Children.Add(topAnimation);
            storyBoard.Begin(Selected, true);
        }

        private double Distance(GraphEdgeVM edge)
        {
            return Distance(edge.Begin, edge.End);
        }

        private double Distance(GraphNodeVM n1, GraphNodeVM n2)
        {
            return GraphFactory.Distance(
                Nodes[n1].X,
                Nodes[n1].Y,
                Nodes[n2].X,
                Nodes[n2].Y);
        }

        private void GraphV_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GenerateNodePositions(e.NewSize.Width, e.NewSize.Height);
        }

        private void GenerateNodePositions(double actualWidth, double actualHeight)
        {
            try
            {
                SetAnimation();

                foreach (var node in Nodes.Keys.ToArray())
                {
                    Nodes[node] = CalculatePosition(node, actualWidth, actualHeight, NodesMargin);
                    NodesNames[node] = CalculatePosition(node, actualWidth, actualHeight, NamesMargin);
                }

                // doesnt need to generate new edges
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            ++ChangeCount;
        }

        private Point CalculatePosition(GraphNodeVM node, double actualWidth, double actualHeight, double margin)
        {
            double width = actualWidth - margin;
            double height = actualHeight - margin;
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
