using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public struct Matrix
    : IEquatable<Matrix>
    {
        private int[,] values;


        public Matrix(uint rows, uint cols)
        {
            values = new int[rows, cols];
        }


        public uint Rows
        {
            get { return (uint)values.GetLength(0); }
        }

        public uint Cols
        {
            get { return (uint)values.GetLength(1); }
        }

        public int this[uint row, uint col]
        {
            get { return values[row, col]; }
            set { values[row, col] = value; }
        }


        #region equality

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return Equals((Matrix)obj);
        }

        public override int GetHashCode()
        {
            return values.GetHashCode();
        }

        public bool Equals(Matrix other)
        {
            if (this.Rows != other.Rows) return false;
            if (this.Cols != other.Cols) return false;
            for (uint row = 0; row < Rows; row++)
            {
                for (uint col = 0; col < Cols; col++)
                {
                    if (this[row, col] != other[row, col]) return false;
                }
            }
            return false;
        }

        public static bool operator ==(Matrix i1, Matrix i2)
        {
            return i1.Equals(i2);
        }

        public static bool operator !=(Matrix i1, Matrix i2)
        {
            return !i1.Equals(i2);
        }

        #endregion
    }
}
