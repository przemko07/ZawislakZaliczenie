using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Ewolucyjny
    {
        Random random = new Random();
        public int[,] osobniki;
        public int[,] najlepsi;
        public List<int> ocenaF1 = new List<int>();
        public List<int> ocenaF2 = new List<int>();
        public Macierz m1;
        public Macierz m2;

        public Ewolucyjny(Permutacja permutacja, Macierz m1, Macierz m2)
        {
            this.osobniki = permutacja.permutacja;
            this.m1 = m1;
            this.m2 = m2;
        }

        void Ocena()
        {
            ocenaF1.Clear();
            ocenaF2.Clear();
            for (int i = 0; i < osobniki.GetLength(0); i++)
            {
                ocenaF1.Add(function(i, m1));
                ocenaF2.Add(function(i, m2));
            }
        }

        int function(int osobnik, Macierz m)
        {
            int sum = 0;
            for (int i = 0; i < osobniki.GetLength(1) - 1; i++)
            {
                int beg = osobniki[osobnik, i];
                int end = osobniki[osobnik, i + 1];
                sum += m.values[beg, end];
            }
            return sum;
        }



        void Eliminacja()
        {
            najlepsi = new int [Math.Max(osobniki.GetLength(0) / 2, 1), osobniki.GetLength(1)];
            for (int i = 0; i < najlepsi.GetLength(0); i++)
            {
                int i1 = random.Next(0, osobniki.GetLength(0));
                int i2 = random.Next(0, osobniki.GetLength(0));
                for (int j = 0; j < osobniki.GetLength(1); j++)
                {
                    najlepsi[i, j] = osobniki[ocenaF1[i1] > ocenaF1[i2] ? i1 : i2, j];
                }
            }
        }

        void Mutacja()
        {

        }

        public void Step()
        {
            Ocena();
            Eliminacja();
            Mutacja();
        }
    }
}
