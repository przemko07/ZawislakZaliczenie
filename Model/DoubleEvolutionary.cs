using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DoubleEvolutionary
    {
        public Evolutionary Evo1 { get; set; }
        public Evolutionary Evo2 { get; set; }
        public IMixer Mixer { get; set; }

        public void Step()
        {
            Evo1.Step();
            Evo2.Step();
            Mixer.Mix(Evo1.individuals, Evo2.individuals);
        }
    }
}
