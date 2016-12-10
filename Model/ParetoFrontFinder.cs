using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ParetoFrontFinder
    {
        public static uint[] FindParetoFront(double[] f1, double[] f2)
        {
            List<uint> pareto = new List<uint>();
            for (int i = 0; i < f1.Length; i++)
            {
                bool isPareto = true;
                for (int j = 0; j < f2.Length; j++)
                {
                    if (f1[i] > f1[j] && f2[i] > f2[j])
                    {
                        isPareto = false;
                        break;
                    }
                }
                if (isPareto) pareto.Add((uint)i);
            }
            return pareto.ToArray();
        }
    }
}
