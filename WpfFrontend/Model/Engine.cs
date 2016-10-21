using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfFrontend.Model
{
    public class Engine
    {
        private DoubleEvolutionary evolutionary;
        private IIterationsPloter iterationsPloter;

        public enum StepNotifierStatus
        {
            NoStarted,
            Started,
            Finished,
        }

        public event EventHandler Started = null;
        public event EventHandler Finished = null;
        public StepNotifierStatus Status = StepNotifierStatus.NoStarted;
        public uint StepsFinished = 0;


        private Matrix _Matrix1;
        public Matrix Matrix1
        {
            get
            {
                return _Matrix1;
            }
            set
            {
                _Matrix1 = value;
                ReCreateEvolutionary();
            }
        }

        private Matrix _Matrix2;
        public Matrix Matrix2
        {
            get
            {
                return _Matrix2;
            }
            set
            {
                _Matrix2 = value;
                ReCreateEvolutionary();
            }
        }

        public Individual[] Individuals
        {
            get
            {
                return Enumerable
                    .Concat(
                        evolutionary.Evo1.individuals,
                        evolutionary.Evo2.individuals)
                    .ToArray();
            }
        }

        private uint _IndividualsCount = 0;
        public uint IndividualsCount
        {
            get
            {
                return _IndividualsCount;
            }
            set
            {
                _IndividualsCount = value;
                ReCreateEvolutionary();
            }
        }

        public double[] Fitness1
        {
            get
            {
                return evolutionary.Evo1.FitnessCalc.Fitness;
            }
        }
        public double[] Fitness2
        {
            get
            {
                return evolutionary.Evo2.FitnessCalc.Fitness;
            }
        }

        Bitmap ParetoFront
        {
            get
            {
                return iterationsPloter.Plot(0, 0);
            }
        }


        public Task Step()
        {
            return Task.Run(() =>
            {
                Status = StepNotifierStatus.Started;
                Started?.Invoke(this, new EventArgs());

                evolutionary.Step();
                iterationsPloter.AddGeneration(Fitness1, Fitness2);

                Status = StepNotifierStatus.Finished;
                Finished?.Invoke(this, new EventArgs());
            });
        }

        private void ReCreateEvolutionary()
        {
            if (BuildDoubleEvo())
            {
                evolutionary = new DoubleEvolutionary();
                evolutionary.Mixer = new SimpleMixer();
            }

            if (BuildEvo1())
            {
                evolutionary.Evo1 = new Evolutionary(
                    PermutationFactory.GenerateIndividuals(
                        IndividualsCount / 2, Matrix1.Cols, true));
                evolutionary.Evo1.Selection = new TournamentSelection(2);
                evolutionary.Evo1.CrossOver = new CrossOverOX();
                evolutionary.Evo1.Mutation = new SimpleMutation(0.05); // 5%
            }

            if (BuildEvo2())
            {
                evolutionary.Evo2 = new Evolutionary(
                    PermutationFactory.GenerateIndividuals(
                        IndividualsCount % 2, Matrix2.Cols, true));
                evolutionary.Evo2.Selection = new TournamentSelection(2);
                evolutionary.Evo2.CrossOver = new CrossOverOX();
                evolutionary.Evo2.Mutation = new SimpleMutation(0.05); // 5%
            }

            if (BuildEvo1Fitness())
            {
                evolutionary.Evo1.FitnessCalc = new MatrixFitnessCalc(Matrix1);
            }

            if (BuildEvo2Fitness())
            {
                evolutionary.Evo2.FitnessCalc = new MatrixFitnessCalc(Matrix2);
            }

        }

        private bool BuildDoubleEvo()
        {
            return evolutionary == null;
        }

        private bool BuildEvo1()
        {
            return evolutionary != null
                && (IndividualsCount > 0 && !Matrix1.Empty)
                || (evolutionary.Evo1.individuals.Length != IndividualsCount || evolutionary.Evo1.individuals[0].Length != Matrix2.Cols);
        }

        private bool BuildEvo2()
        {
            return evolutionary != null
                && (IndividualsCount > 0 && !Matrix2.Empty)
                || (evolutionary.Evo2.individuals.Length != IndividualsCount || evolutionary.Evo2.individuals[0].Length != Matrix2.Cols);
        }

        private bool BuildEvo1Fitness()
        {
            return evolutionary != null && evolutionary.Evo1 != null
                && (evolutionary.Evo1.FitnessCalc != null || evolutionary.Evo1.FitnessCalc is MatrixFitnessCalc)
                && ((evolutionary.Evo1.FitnessCalc as MatrixFitnessCalc).matrix != Matrix1);
        }

        private bool BuildEvo2Fitness()
        {
            return evolutionary != null && evolutionary.Evo2 != null
                && (evolutionary.Evo2.FitnessCalc != null || evolutionary.Evo2.FitnessCalc is MatrixFitnessCalc)
                && ((evolutionary.Evo2.FitnessCalc as MatrixFitnessCalc).matrix != Matrix2);
        }
    }
}
