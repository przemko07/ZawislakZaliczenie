using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DoubleEvolutionary
    {
        public readonly Evolutionary evo1;
        public readonly Evolutionary evo2;


        DoubleEvolutionary(Evolutionary evo1, Evolutionary evo2)
        {
            this.evo1 = evo1;
            this.evo2 = evo2;
        }


        public IMixer Mixer { get; set; }

        public void Step()
        {
            evo1.Step();
            evo2.Step();
            Mixer.Mix(evo1.individuals, evo2.individuals);
        }
    }
}
