using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SimpleMixer
    : IMixer
    {
        private Random random = RandomGenerator.GetRandom;

        public void Mix(Individual[] ind1, Individual[] ind2)
        {
            //var col = ind1.Concat(ind2)
            //    .OrderBy(n => random.Next());
            //
            //Individuals1 = col.Take(ind1.Length).ToArray();
            //Individuals2 = col.Skip(ind2.Length).ToArray();
            Individuals1 = new Individual[ind1.Length];
            Individuals2 = new Individual[ind2.Length];
            int i1 = 0;
            int i2 = 0;
            for (int i = 0; i < ind1.Length; i++)
            {
                if (i1 == Individuals1.Length) Individuals2[i2++] = ind1[i].Clone();
                else if (i2 == Individuals2.Length) Individuals1[i1++] = ind1[i].Clone();
                else if (random.NextDouble() < 0.5) Individuals2[i2++] = ind1[i].Clone();
                else Individuals1[i1++] = ind1[i].Clone();
            }

            for (int i = 0; i < ind2.Length; i++)
            {
                if (i1 == Individuals1.Length) Individuals2[i2++] = ind2[i].Clone();
                else if (i2 == Individuals2.Length) Individuals1[i1++] = ind2[i].Clone();
                else if (random.NextDouble() < 0.5) Individuals2[i2++] = ind2[i].Clone();
                else Individuals1[i1++] = ind2[i].Clone();
            }
        }

        public Individual[] Individuals1 { get; set; }
        public Individual[] Individuals2 { get; set; }
    }
}
