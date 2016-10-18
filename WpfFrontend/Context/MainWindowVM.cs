﻿using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private bool _Editable;
        public bool Editable
        {
            get { return _Editable; }
            set
            {
                _Editable = value;
                OnPropertyChanged(nameof(Editable));
            }
        }

        private bool _CopyByDiagonal;
        public bool CopyByDiagonal
        {
            get { return _CopyByDiagonal; }
            set
            {
                _CopyByDiagonal = value;
                OnPropertyChanged(nameof(CopyByDiagonal));
            }
        }
        

        Individual individual = PermutationFactory.GenerateIndividuals(1, 10, true)[0];

        private GraphVM _Graph;
        public GraphVM Graph
        {
            get
            {
                if (_Graph == null) _Graph = GraphFactory.GenerateClique(individual);

                return _Graph;
            }
        }

        private GraphVM _GraphPath;
        public GraphVM GraphPath
        {
            get
            {
                if (_GraphPath == null) _GraphPath = GraphFactory.GeneratePath(Graph, individual);

                return _GraphPath;

            }
        }

        public MatrixVM Matrix
        {
            get
            {
                return new MatrixVM(MatrixFactory.CreateRandomDiagonal(5, 0, 100));
            }
        }
    }
}
