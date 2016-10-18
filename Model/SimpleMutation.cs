using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SimpleMutation
    : IMutation
    {
        private Random random = RandomGenerator.GetRandom;
        private double percentage;

        public SimpleMutation(double percentage)
        {
            this.percentage = percentage;
        }

        public bool IsMutated { get; set; }
        private Individual _MutatedIndividual;
        public Individual MutatedIndividual { get { return _MutatedIndividual; } }
        public void Mutate(Individual individual)
        {
            var tmp = random.NextDouble();
            if (tmp < percentage)
            {
                uint i1 = (uint)random.Next((int)individual.Length);
                uint i2 = (uint)random.Next((int)individual.Length);

                IsMutated = true;
                _MutatedIndividual = individual.Clone();
                _MutatedIndividual[i1] = individual[i2];
                _MutatedIndividual[i2] = individual[i1];
            }
            else
            {
                IsMutated = false;
            }
        }
    }
}
