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
            int[] indicies1 = Enumerable
                .Range(0, ind1.Length)
                .OrderBy(n => random.Next())
                .ToArray();
            int[] indicies2 = Enumerable
                .Range(0, ind1.Length)
                .OrderBy(n => random.Next())
                .ToArray();

            for (int i = 0; i < ind1.Length; i++)
            {
                var copy = ind1[indicies1[i]];
                ind2[indicies1[i]] = ind1[indicies2[i]];
                ind1[indicies2[i]] = copy;
            }

        }
    }
}
