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
                    List<uint> values = Enumerable.Range(0, (int)singleLength).Select(n=>(uint)n).ToList();
                    for (uint j = 0; j < singleLength; ++j)
                    {
                        int index = random.Next(0, values.Count);
                        individuals[i][j] = values[index];
                        values.RemoveAt(index);
                    }
                    for (int ik = 0; ik < i - 1; ik++)
                    {
                        if (uniquePopulation && individuals[ik] == individuals[i]) uniqe = false;
                    }

                } while (!uniqe);
            }

            return individuals;
        }
    }
}
