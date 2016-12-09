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
            var col = ind1.Concat(ind2)
                .OrderBy(n => random.Next());

            Individuals1 = col.Take(ind1.Length).ToArray();
            Individuals2 = col.Skip(ind2.Length).ToArray();
        }
        
        public Individual[] Individuals1 { get; set; }
        public Individual[] Individuals2 { get; set; }
    }
}
