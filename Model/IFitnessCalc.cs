using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IFitnessCalc
    {
        void Calculate(Individual[] individuals);
        double[] Fitness { get; }
    }
}
