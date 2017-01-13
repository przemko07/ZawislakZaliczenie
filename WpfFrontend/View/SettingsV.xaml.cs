using Model;
using System;
using System.Collections.Generic;
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
using WpfFrontend.Model;
using WpfFrontend.ViewModel;
using Matrix = Model.Matrix;

namespace WpfFrontend.View
{
    public partial class SettingsV
    : Window
    , INotifyPropertyChanged
    {
        public Engine EvoEngine
        {
            get { return (Engine)GetValue(EvoEngineProperty); }
            set { SetValue(EvoEngineProperty, value); }
        }
        public static readonly DependencyProperty EvoEngineProperty =
            DependencyProperty.Register("EvoEngine", typeof(Engine), typeof(SettingsV), new PropertyMetadata(null, EvoEngineChanged));
        private static void EvoEngineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SettingsV t = d as SettingsV;
            if (t == null) return;

            if (t.EvoEngine == null) return;

            t.Matrix1 = new MatrixVM(t.EvoEngine.Matrix1);
            t.Matrix2 = new MatrixVM(t.EvoEngine.Matrix2);
            t.PopSize = t.EvoEngine.IndividualsLength;
            t.NodesCount = t.EvoEngine.NodesCount;
            t.Optimalize = t.EvoEngine.Optimalize;
            t.wasChange = false;
        }

        bool wasChange = false;
        public SettingsV()
        {
            InitializeComponent();
            this.DataContext = this;
            Closing += SettingsV_Closing;
        }

        private MatrixVM _Matrix1 = null;
        public MatrixVM Matrix1
        {
            get { return _Matrix1; }
            set
            {
                if (_Matrix1 != null) _Matrix1.MatrixChanged -= _Matrix_MatrixChanged;
                _Matrix1 = value;
                if (_Matrix1 != null) _Matrix1.MatrixChanged += _Matrix_MatrixChanged;
                OnPropertyChanged(nameof(Matrix1));
                wasChange = true;
            }
        }

        private MatrixVM _Matrix2 = null;
        public MatrixVM Matrix2
        {
            get { return _Matrix2; }
            set
            {
                if (_Matrix2 != null) _Matrix2.MatrixChanged -= _Matrix_MatrixChanged;
                _Matrix2 = value;
                if (_Matrix2 != null) _Matrix2.MatrixChanged += _Matrix_MatrixChanged;
                OnPropertyChanged(nameof(Matrix2));
                wasChange = true;
            }
        }
        private void _Matrix_MatrixChanged(object sender, EventArgs e)
        {
            wasChange = true;
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

        private uint _PopSize;
        public uint PopSize
        {
            get { return _PopSize; }
            set
            {
                _PopSize = value;
                OnPropertyChanged(nameof(PopSize));
                wasChange = true;
            }
        }

        private uint _NodesCount;
        public uint NodesCount
        {
            get { return _NodesCount; }
            set
            {
                _NodesCount = value;
                OnPropertyChanged(nameof(NodesCount));
                wasChange = true;
            }
        }

        private bool _Optimalize;
        public bool Optimalize
        {
            get { return _Optimalize; }
            set
            {
                _Optimalize = value;
                OnPropertyChanged(nameof(Optimalize));
                EvoEngine.Optimalize = value;
            }
        }



        public ActionCommand Exit
        {
            get
            {
                return new ActionCommand(() =>
                {
                    Close();
                });
            }
        }

        public ActionCommand RandomMatrices
        {
            get
            {
                return new ActionCommand(() =>
                {
                    Matrix1 = new MatrixVM(MatrixFactory.CreateRandomDiagonal(NodesCount, 0, 100));
                    Matrix2 = new MatrixVM(MatrixFactory.CreateRandomDiagonal(NodesCount, 0, 100));
                });
            }
        }


        private void SettingsV_Closing(object sender, CancelEventArgs e)
        {
            if (wasChange)
            {
                var res = MessageBox.Show("Save changes?", "Changes was made", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Cancel) e.Cancel = true;
                else if (res == MessageBoxResult.Yes)
                {
                    if (EvoEngine.NodesCount != NodesCount)
                    {
                        EvoEngine.IndividualsLength = PopSize;
                        EvoEngine.NodesCount = NodesCount;
                        EvoEngine.Matrix1 = MatrixFactory.CreateRandomDiagonal(NodesCount, 0, 100);
                        EvoEngine.Matrix2 = MatrixFactory.CreateRandomDiagonal(NodesCount, 0, 100);
                    }
                    else
                    {
                        EvoEngine.IndividualsLength = PopSize;
                        EvoEngine.NodesCount = NodesCount;
                        CopyMatrices();
                    }

                    try
                    {
                        EvoEngine.ReCreateEvolutionary();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void CopyMatrices()
        {
            EvoEngine.Matrix1 = FromVM(Matrix1);
            EvoEngine.Matrix2 = FromVM(Matrix2);
        }

        private Matrix FromVM(MatrixVM matrix)
        {
            uint rows = (uint)matrix.Mask.GetLength(0);
            uint cols = (uint)matrix.Mask.GetLength(0);
            Matrix m = new Matrix(rows, cols);
            for (uint row = 0; row < rows; row++)
            {
                for (uint col = 0; col < cols; col++)
                {
                    m[row, col] = matrix.Mask[row, col].Value;
                }
            }
            return m;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
