using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Permutacja
    {
        public int[,] permutacja;

        public Permutacja(uint population_size, uint single_length, bool unique_population = false)
        {
            permutacja = new int[population_size, single_length];

            Fill(unique_population);
        }
        
        private void Fill(bool unique_population)
        {
            Random random = new Random();
            for (int i = 0; i < permutacja.GetLength(0); ++i)
            {
                bool uniqe = true;
                do {
                    List<int> values = Enumerable.Range(0, permutacja.GetLength(1)).ToList();
                    for (int j = 0; j < permutacja.GetLength(1); ++j)
                    {
                        int index = random.Next(0, values.Count);
                        permutacja[i, j] = values[index];
                        values.RemoveAt(index);
                    }
                    for (int ik = 0; ik < i - 1; ik++)
                    {
                        if (IsEqual(ik, i)) uniqe = false;
                    }

                } while (!uniqe);
            }
        }

        bool IsEqual(int i1, int i2) {
            for (int i = 0; i < permutacja.Length; i++)
            {
                if (permutacja[i1, i] != permutacja[i2, i]) return false;
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < permutacja.GetLength(0); ++i)
            {
                sb.Append(permutacja[i, 0]);
                for (int j = 1; j < permutacja.GetLength(1); ++j)
                {
                    sb.AppendFormat(", {0}", permutacja[i, j]);
                }
                sb.AppendLine();
            }
            
            return sb.ToString();
        }
    }
}
