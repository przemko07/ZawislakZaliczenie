using System;
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
            List<uint> selected = new List<uint>();
            for (int i = 0; i < fitness.Length; i++)
            {
                double best = double.NegativeInfinity;
                uint bestIndex = 0;
                for (
                    int j = 0;
                    i < IndividualPerTournament && i < fitness.Length;
                    j++, i++)
                {
                    uint index = (uint)random.Next(fitness.Length);
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
