using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IIterationsPloter
    {
        void AddGeneration(double[] f1, double[] f2);
        uint[] ParetoFront { get; }
        Bitmap Plot(uint width, uint height);
    }
}
