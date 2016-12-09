using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfFrontend.Extensions;
using WpfFrontend.Model;

namespace WpfFrontend.ViewModel
{
    public class MatrixValueNameVM
    {
        public string Name { get; set; }

        public MatrixValueNameVM(string name)
        {
            this.Name = name;
        }
    }

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
                    matrix.Mask[col, row]._Value = value;
                    matrix.Mask[col, row].OnPropertyChanged(nameof(Value));
                }

                matrix.OnMatrixChanged();
            }
        }

        public bool IsDiagonal
        {
            get { return row == col; }
        }

        public MatrixValueVM(MatrixVM matrix, int value, uint row, uint col)
        {
            this.matrix = matrix;
            this._Value = value;
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
        public ObservableCollection<object> Items { get; } = new ObservableCollection<object>();

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
            Rows = matrix.Rows + 1;
            Cols = matrix.Cols + 1;
            Mask = new MatrixValueVM[Rows, Cols];
            Items.Add(new MatrixValueNameVM(string.Empty));
            for (uint row = 0; row < matrix.Rows; row++)
            {
                Items.Add(new MatrixValueNameVM(GraphFactory.Names[row]));
            }

            for (uint row = 0; row < matrix.Rows; row++)
            {
                Items.Add(new MatrixValueNameVM(GraphFactory.Names[row]));
                for (uint col = 0; col < matrix.Cols; col++)
                {
                    var vm = new MatrixValueVM(this, matrix[row, col], row, col);
                    Items.Add(vm);
                    Mask[row, col] = vm;
                }
            }
        }
    }
}
