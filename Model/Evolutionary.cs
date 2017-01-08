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
        public Individual[] newIndividuals;

        public IFitnessCalc FitnessCalc { get; set; }
        public ISelection Selection { get; set; }
        public ICrossOver CrossOver { get; set; }
        public IMutation Mutation { get; set; }


        public Evolutionary(Individual[] individuals)
        {
            this.individuals = individuals;
            this.newIndividuals = new Individual[individuals.Length];
        }


        void CalculateFitness()
        {
            FitnessCalc?.Calculate(individuals);
        }

        void Select()
        {
            if (FitnessCalc == null) return;
            Selection?.Select(FitnessCalc.Fitness);
        }

        void Cross()
        {
            newIndividuals = new Individual[individuals.Length];
            if (CrossOver == null || Selection == null) return;

            int length = Selection.Selected.Length % 2 == 0 ? Selection.Selected.Length : Selection.Selected.Length - 1;

            int index = 0;
            // Offsprings
            for (int i = 0; i < length; i += 2)
            {
                CrossOver.Cross(
                    individuals[Selection.Selected[i]],
                    individuals[Selection.Selected[i + 1]]);
                newIndividuals[index++] = CrossOver.Offspring1.Clone();
                newIndividuals[index++] = CrossOver.Offspring2.Clone();
            }

            // Parents
            for (int i = 0; i < length; i += 2)
            {
                newIndividuals[index++] = individuals[Selection.Selected[i]].Clone();
                newIndividuals[index++] = individuals[Selection.Selected[i + 1]].Clone();
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
            Select();
            Cross();
            Mutate();

            individuals = newIndividuals;
        }
    }
}
