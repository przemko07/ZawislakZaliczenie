using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TournamentSelection
    : ISelection
    {
        private Random random = RandomGenerator.GetRandom;
        public readonly uint IndividualPerTournament;


        public TournamentSelection(uint IndividualPerTournament)
        {
            this.IndividualPerTournament = IndividualPerTournament;
        }

        public uint[] Selected { get; set; }

        public void Select(double[] fitness)
        {
            int[] indicies = Enumerable
                .Range(0, fitness.Length)
                .OrderBy(n => random.Next())
                .ToArray();
            List<uint> selected = new List<uint>();
            for (int i = 0; i < fitness.Length;)
            {
                double best = double.NegativeInfinity;
                uint bestIndex = 0;
                for (
                    int j = 0;
                    j < IndividualPerTournament && i < fitness.Length;
                    j++, i++)
                {
                    uint index = (uint)indicies[i];
                    if (fitness[index] > best)
                    {
                        best = fitness[index];
                        bestIndex = index;
                    }
                }
                if (best != double.NegativeInfinity) selected.Add(bestIndex);
            }
            Selected = selected.ToArray();
        }
    }
}
