﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Program
    {
        static void Main(string[] args)
        {
            var individuals = PermutationFactory.GenerateIndividuals(30, 7, true);
            Console.WriteLine(string.Join("\n", individuals));

            Matrix matrix = MatrixFactory.CreateRandomDiagonal(7, 0, 100);

            Evolutionary evo = new Model.Evolutionary(individuals);
            evo.FitnessCalc = new MatrixFitnessCalc(matrix);
            evo.Selection = new TournamentSelection(2);
            evo.CrossOver = new CrossOverOX();
            evo.Mutation = new SimpleMutation(0.5); // 5%

            evo.Step();

            //Macierz m = new Macierz(10, 0, 100);
        }
    }
}
