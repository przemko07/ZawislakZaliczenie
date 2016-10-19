using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MatrixFitnessCalc
    : IFitnessCalc
    {
        public readonly Matrix matrix;


        public MatrixFitnessCalc(Matrix matrix)
        {
            this.matrix = matrix;
        }


        public double[] Fitness { get; set; }

        public void Calculate(Individual[] individuals)
        {
            Fitness = new double[individuals.Length];
            for (int i = 0; i < individuals.Length; i++)
            {
                Fitness[i] = CalculateForSingle(individuals[i]);
            }
        }

        public int CalculateForSingle(Individual individual)
        {
            int sum = 0;
            for (uint i = 1; i < individual.Length - 1; i++)
            {
                uint beg = individual[i];
                uint end = individual[i + 1];
                sum += matrix[beg, end];
            }
            return sum;
        }
    }
}
