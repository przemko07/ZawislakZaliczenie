using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Evolutionary
    {
        Random random = RandomGenerator.GetRandom;

        public Individual[] individuals;
        public Individual[] best;
        public List<int> fitness = new List<int>();
        public Matrix matrix;

        public Evolutionary(Individual[] individuals, Matrix matrix)
        {
            this.individuals = individuals;
            this.matrix = matrix;
        }

        void CalculateFitness()
        {
            fitness.Clear();
            for (int i = 0; i < individuals.Length; i++)
            {
                fitness.Add(function(individuals[i]));
            }
        }

        int function(Individual individual)
        {
            int sum = 0;
            for (uint i = 1; i < individual.Length - 1; i++)
            {
                uint beg = individual[i];
                uint end = individual[i + 1];
                sum += matrix[beg, end];
            }
            return sum;
        }



        void Eliminate()
        {
            best = new Individual[(individuals.Length + 1) / 2];
            for (int i = 0; i < best.Length; i++)
            {
                int index1 = random.Next(individuals.Length);
                int index2 = random.Next(individuals.Length);
                Individual i1 = individuals[index1];
                Individual i2 = individuals[index2];
                for (int j = 0; j < i1.Length; j++)
                {
                    best[i] = individuals[fitness[index1] > fitness[index2] ? index1 : index2];
                }
            }
        }

        void Crossing(ICrossOver crossOver)
        {
            for (int i = 0; i < best.Length; i += 2)
            {
                //cross(best[i], best[i + 1]);
            }
        }

        void Mutation()
        {

        }

        public void Step()
        {
            CalculateFitness();
            Eliminate();
            //Crossing();
            Mutation();
        }
    }
}
