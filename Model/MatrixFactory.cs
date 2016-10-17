using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public static class MatrixFactory
    {
        private static Random random = RandomGenerator.GetRandom;


        public static Matrix CreateRandomDiagonal(uint n, int from, int to)
        {
            Matrix matrix = new Matrix(n, n);

            FillTopRandom(matrix, from, to);

            CopyTopToBottom(matrix);

            FillDiagonalZero(matrix);

            return matrix;
        }

        private static void FillTopRandom(Matrix matrix, int from, int to)
        {
            for (uint row = 0; row < matrix.Rows; row++)
            {
                for (uint col = row; col < matrix.Cols; col++)
                {
                    matrix[row, col] = random.Next(from, to);
                }
            }
        }


        private static void CopyTopToBottom(Matrix matrix)
        {
            for (uint row = 1; row < matrix.Rows; row++)
            {
                for (uint col = 0; col < row; col++)
                {
                    matrix[row, col] = matrix[col, row];
                }
            }
        }

        private static void FillDiagonalZero(Matrix matrix)
        {
            for (uint index = 0; index < matrix.Rows; index++)
            {
                matrix[index, index] = 0;
            }
        }
    }
}
