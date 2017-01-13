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

        public Individual[] Individuals
        {
            get
            {
                return Evo1.individuals.Concat(Evo2.individuals).ToArray();
            }
        }

        public double[] Fitness1
        {
            get
            {
                Evo1.FitnessCalc.Calculate(Evo1.individuals);
                double[] fitnessEvo1 = Evo1.FitnessCalc.Fitness;

                Evo1.FitnessCalc.Calculate(Evo2.individuals);
                double[] fitnessEvo2 = Evo1.FitnessCalc.Fitness;

                var result = fitnessEvo1.Concat(fitnessEvo2).ToArray();

                return result;
            }
        }

        public double[] Fitness2
        {
            get
            {
                Evo2.FitnessCalc.Calculate(Evo1.individuals);
                double[] fitnessEvo1 = Evo2.FitnessCalc.Fitness;

                Evo2.FitnessCalc.Calculate(Evo2.individuals);
                double[] fitnessEvo2 = Evo2.FitnessCalc.Fitness;

                var result = fitnessEvo1.Concat(fitnessEvo2).ToArray();

                return result;
            }
        }

        public void Step()
        {
            Evo1.Step();
            Evo2.Step();

            Mixer.Mix(Evo1.individuals, Evo2.individuals);

            Evo1.individuals = Mixer.Individuals1;
            Evo2.individuals = Mixer.Individuals2;
        }
    }
}
