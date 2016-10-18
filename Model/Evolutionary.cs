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
        public Individual[] newIndividuals;
        public List<int> fitness = new List<int>();
        public Matrix matrix;
        
        public ICrossOver CrossOver { get; set; }
        public IMutation Mutation { get; set; }


        public Evolutionary(Individual[] individuals, Matrix matrix)
        {
            this.individuals = individuals;
            this.matrix = matrix;
            this.newIndividuals = new Individual[individuals.Length];
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

                best[i] = individuals[fitness[index1] > fitness[index2] ? index1 : index2];
            }
        }

        void Cross()
        {
            if (CrossOver == null) return;

            int length = best.Length % 2 == 0 ? best.Length : best.Length - 1;

            int index = 0;
            // Offsprings
            for (int i = 0; i < length; i += 2)
            {
                CrossOver.Cross(best[i], best[i + 1]);
                newIndividuals[i] = CrossOver.Offspring1;
                newIndividuals[i + 1] = CrossOver.Offspring2;
                index += 2;
            }

            // Parents
            for (int i = 0; i < length; i += 2)
            {
                newIndividuals[i + length] = best[i].Clone();
                newIndividuals[i + 1 + length] = best[i + 1].Clone();
                index += 2;
            }

            // XXX: for safety
            while (index < newIndividuals.Length)
            {
                int i = random.Next(individuals.Length);
                newIndividuals[index++] = individuals[i].Clone();
            }
        }

        void Mutate()
        {
            if (Mutation == null) return;

            for (int i = 0; i < newIndividuals.Length; i++)
            {
                Mutation.Mutate(newIndividuals[i]);
                if (Mutation.IsMutated)
                {
                    newIndividuals[i] = Mutation.MutatedIndividual;
                }
            }
        }

        public void Step()
        {
            CalculateFitness();
            Eliminate();
            Cross();
            Mutate();
        }
    }
}
