using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CrossOverOX
    : ICrossOver
    {
        private Individual _offspring1;
        public Individual offspring1 { get { return _offspring1; }  }

        private Individual _offspring2;
        public Individual offspring2 { get { return _offspring2; } }

        public void Cross(Individual _individual1, Individual _individual2)
        {
            List<uint> individual1 = _individual1.ToList();
            List<uint> individual2 = _individual2.ToList();

            bool[] cycle = new bool[individual1.Count];

            int index1 = 0;

            while (true)
            {
                cycle[index1] = true;

                uint index2 = individual2[index1];
                if (index2 == individual1[index1]) index2 = individual2[(int)index2];
                uint value = index2;
                index1 = individual1.IndexOf(value);
                        
                if (cycle[index1]) break;
            }
            
            _offspring1 = Individual.IndividualOfLength((uint)individual1.Count);
            _offspring2 = Individual.IndividualOfLength((uint)individual2.Count);

            for (uint i = 0; i < cycle.Length; i++)
            {
                if (cycle[i])
                {
                    _offspring1[i] = _individual1[i];
                    _offspring2[i] = _individual2[i];
                }
                else
                {
                    _offspring1[i] = _individual2[i];
                    _offspring2[i] = _individual1[i];
                }
            }
            
        }
    }
}
