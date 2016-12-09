using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Model
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrix1 = MatrixFactory.CreateRandomDiagonal(30, 0, 100);
            Evolutionary evo1 = new Model.Evolutionary(PermutationFactory.GenerateIndividuals(30, 10, true));
            evo1.FitnessCalc = new MatrixFitnessCalc(matrix1);
            evo1.Selection = new TournamentSelection(4);
            evo1.CrossOver = new CrossOverOX();
            evo1.Mutation = new SimpleMutation(0.3); // 5%

            var matrix2 = MatrixFactory.CreateRandomDiagonal(10, 0, 100);
            Evolutionary evo2 = new Model.Evolutionary(PermutationFactory.GenerateIndividuals(30, 10, true));
            evo2.FitnessCalc = new MatrixFitnessCalc(matrix2);
            evo2.Selection = new TournamentSelection(4);
            evo2.CrossOver = new CrossOverOX();
            evo2.Mutation = new SimpleMutation(0.3); // 5%

            DoubleEvolutionary evo = new DoubleEvolutionary();
            evo.Evo1 = evo1;
            evo.Evo2 = evo2;
            evo.Mixer = new SimpleMixer();

            for (int i = 0; i < 100; i++)
            {
                evo.Step();

                //ploter.AddGeneration(evo.Fitness1, evo.Fitness2);
                //Bitmap bmp = ploter.Plot(100, 100);
                //
                //bmp.SetPixel(i, 0, Color.Red);
                //try { bmp.Save(@"H:\plot.png"); }
                //catch { Thread.Sleep(1000); }
                //Thread.Sleep(200);
            }
        }
    }
}
