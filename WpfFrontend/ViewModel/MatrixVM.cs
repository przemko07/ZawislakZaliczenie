using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfFrontend.Extensions;

namespace WpfFrontend.ViewModel
{
    public class MatrixValueVM
    : ObjectVM
    {
        private readonly MatrixVM matrix;

        private uint row;
        private uint col;

        private int _Value;
        public int Value
        {
            get { return _Value; }
            set
            {
                if (_Value == value) return;

                _Value = value;
                OnPropertyChanged(nameof(Value));
                
                if (matrix.CopyByDiagonal)
                {
                    matrix.Mask[col, row].OnPropertyChanged(nameof(Value));
                }

                matrix.OnMatrixChanged();
            }
        }

        public bool IsDiagonal
        {
            get { return row == col; }
        }

        public MatrixValueVM(MatrixVM matrix, uint row, uint col)
        {
            this.matrix = matrix;
            this.row = row;
            this.col = col;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class MatrixVM
    : ObjectVM
    {
        public event EventHandler MatrixChanged = null;
        public void OnMatrixChanged() => MatrixChanged?.Invoke(this, new EventArgs());
        public bool CopyByDiagonal;


        public MatrixValueVM[,] Mask;
        public ObservableCollection<MatrixValueVM> Items { get; } = new ObservableCollection<MatrixValueVM>();

        private uint _Rows;
        public uint Rows
        {
            get { return _Rows; }
            set
            {
                _Rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }

        private uint _Cols;
        public uint Cols
        {
            get { return _Cols; }
            set
            {
                _Cols = value;
                OnPropertyChanged(nameof(Cols));
            }
        }


        public MatrixVM(Matrix matrix)
        {
            Rows = matrix.Rows;
            Cols = matrix.Cols;
            Mask = new MatrixValueVM[Rows, Cols];
            for (uint row = 0; row < matrix.Rows; row++)
            {
                for (uint col = 0; col < matrix.Cols; col++)
                {
                    var vm = new MatrixValueVM(this, row, col);
                    Items.Add(vm);
                    Mask[row, col] = vm;
                }
            }
        }
    }
}
