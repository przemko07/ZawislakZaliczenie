﻿using System;
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
        private int margin = 10;

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
            CircleGraphPositioner positioner = new CircleGraphPositioner(graphV.Graph);

            double width = graphV.ActualWidth - graphV.margin;
            double height = graphV.ActualHeight - graphV.margin;
            double rx = width / 2; // -10 cause the margin
            double ry = height / 2; // -10 cause the margin
            double cx = width / 2;
            double cy = height / 2;
            if (rx < 10) rx = 10; // I just want to see anything, uglynees is not important
            if (ry < 10) ry = 10; // I just want to see anything, uglynees is not important

            try
            {
                foreach (var node in graphV.Graph.Nodes)
                {
                    // I only need to calculate new positions
                    var newPos = positioner.Position(node, rx, ry, cx, ry);
                    newPos.X -= graphV.margin / 2;
                    newPos.Y += graphV.margin / 2;
                    graphV.Nodes[node] = newPos;
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
            CircleGraphPositioner positioner = new CircleGraphPositioner(Graph);

            double width = ActualWidth - margin; // -10 cause the margin
            double height = ActualHeight - margin; // -10 cause the margin
            double rx = width / 2;
            double ry = height / 2;
            double cx = width / 2;
            double cy = height / 2;
            if (rx < 10) rx = 10; // I just want to see anything, uglynees is not important
            if (ry < 10) ry = 10; // I just want to see anything, uglynees is not important

            try
            {
                foreach (var node in Nodes.Keys.ToArray())
                {
                    // I only need to calculate new positions
                    var newPos = positioner.Position(node, rx, ry, cx, ry);
                    newPos.X -= margin / 2;
                    newPos.Y += margin / 2;
                    Nodes[node] = newPos;
                }

                // doesnt need to generate new edges
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            ++ChangeCount;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
