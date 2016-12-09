using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public static class PermutationFactory
    {
        static Random random = RandomGenerator.GetRandom;


        public static Individual[] GenerateIndividuals(uint populationSize, uint singleLength, bool uniquePopulation = false)
        {
            Individual[] individuals = new Individual[populationSize];

            for (int i = 0; i < individuals.Length; ++i)
            {
                individuals[i] = Individual.IndividualOfLength(singleLength);
                bool uniqe = true;
                do
                {
                    RandomSingleIndividual(individuals, i);
                    for (int ik = 0; ik < i - 1; ik++)
                    {
                        if (uniquePopulation && individuals[ik] == individuals[i]) uniqe = false;
                    }
                } while (!uniqe);
            }

            return individuals;
        }

        private static void RandomSingleIndividual(Individual[] individuals, int i)
        {
            bool[] was = new bool[individuals[i].Length];
            for (int j = 0; j < was.Length; j++) was[j] = false;

            for (uint j = 0; j < was.Length; j++)
            {
                uint index = 1;
                do
                {
                    index = (uint)random.Next(was.Length);
                } while (was[index]);

                individuals[i][index] = j;
                was[index] = true;
            }
        }
    }
}
