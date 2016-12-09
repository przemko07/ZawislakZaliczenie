using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public static class RandomGenerator
    {
        private static Random globalRandom = new Random(0);

        public static Random GetRandom
        {
            get
            {
                return new Random(globalRandom.Next());
            }
        }
    }
}
