using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Macierz
    {
        private Random random = new Random();
        public int[,] values;

        public Macierz(int n, int from, int to)
        {
            values = new int[n, n];
            
            FillTopRandom(from, to);
            Console.WriteLine(ToString());
            CopyTopToBottom();
            Console.WriteLine(ToString());
            FillDiagonalZero();
            Console.WriteLine(ToString());
        }
        
        private void FillTopRandom(int from, int to)
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = i; j < values.GetLength(1); j++)
                {
                    values[i, j] = random.Next(from, to);
                }
            }
        }

        private void FillDiagonalZero()
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                values[i, i] = 0;
            }
        }

        private void CopyTopToBottom()
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    values[i, j] = values[j, i];
                }
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < values.GetLength(0); i++)
            {
                sb.AppendFormat("{0:00}", values[i, 0]);
                for (int j = 1; j < values.GetLength(1); j++)
                {
                    sb.AppendFormat(", {0:00}", values[i, j]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
