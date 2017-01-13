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

        public bool Empty
        {
            get { return values == null || Rows == 0 || Cols == 0; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            string firstdigitFormat = "{0:000}";
            string digitFormat = ", {0:000}";
            for (uint row = 0; row < this.Rows; row++)
            {
                sb.AppendFormat(firstdigitFormat, this[row, 0]);
                for (uint col = 1; col < this.Cols; col++)
                {
                    sb.AppendFormat(digitFormat, this[row, col]);
                }
                sb.AppendLine();
            }
            sb.AppendLine();

            return sb.ToString();
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
            return true;
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

        public Matrix Clone()
        {
            Matrix clone = new Model.Matrix(this.Rows, this.Cols);
            for (uint row = 0; row < this.Rows; row++)
            {
                for (uint col = 0; col < this.Cols; col++)
                {
                    clone[row, col] = this[row, col];
                }
            }
            return clone;
        }
    }
}
