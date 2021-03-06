﻿using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TSP.Model
{
    public class Engine
    {
        private static uint _popSize = 60;
        private static uint _nodeCount = 7;
        private static int _m1ValuesFrom = 0;
        private static int _m1ValuesTo = 100;
        private static int _m2ValuesFrom = 0;
        private static int _m2ValuesTo = 100;
        private static double _mutation = 0.15;

        private Matrix? _Matrix1;
        public Matrix Matrix1
        {
            get
            {
                if (_Matrix1 == null)
                {
                    _Matrix1 = MatrixFactory.CreateRandomDiagonal(_nodeCount, _m1ValuesFrom, _m1ValuesTo);
                }
                return _Matrix1.Value;
            }
            set
            {
                _Matrix1 = value;
            }
        }

        private Matrix? _Matrix2;
        public Matrix Matrix2
        {
            get
            {
                if (_Matrix2 == null)
                {
                    _Matrix2 = MatrixFactory.CreateRandomDiagonal(_nodeCount, _m2ValuesFrom, _m2ValuesTo);
                }
                return _Matrix2.Value;
            }
            set
            {
                _Matrix2 = value;
            }
        }

        private uint? _IndividualsLength;
        public uint IndividualsLength
        {
            get
            {
                if (_IndividualsLength == null)
                {
                    _IndividualsLength = _popSize;
                }
                return _IndividualsLength.Value;
            }
            set
            {
                _IndividualsLength = value;
            }
        }

        private uint? _NodesCount;
        public uint NodesCount
        {
            get
            {
                if (_NodesCount == null)
                {
                    _NodesCount = _nodeCount;
                }
                return _NodesCount.Value;
            }
            set
            {
                _NodesCount = value;
            }
        }

        private double? _Mutation;
        public double Mutation
        {
            get
            {
                if (_Mutation == null)
                {
                    _Mutation = _mutation;
                }
                return _Mutation.Value;
            }
            set { _Mutation = value; }
        }
        

        private DoubleEvolutionary _Evolutionary;
        public DoubleEvolutionary Evolutionary
        {
            get
            {
                if (_Evolutionary == null)
                {
                    ReCreateEvolutionary();
                }
                return _Evolutionary;
            }
            private set
            {
                _Evolutionary = value;
            }
        }

        public bool Optimalize = false;

        public Engine()
        {
            ReCreateEvolutionary();
        }

        public void ReCreateEvolutionary()
        {
            var individuals = PermutationFactory.GenerateIndividuals(IndividualsLength, NodesCount, true);

            _Evolutionary = new DoubleEvolutionary()
            {
                Evo1 = new Evolutionary(individuals.Take((int)individuals.Length / 2).ToArray())
                {
                    FitnessCalc = new MatrixFitnessCalc(Matrix1),
                    Selection = new TournamentSelection(2),
                    CrossOver = new CrossOverOX(),
                    Mutation = new SimpleMutation(Mutation)
                },
                Evo2 = new Evolutionary(individuals.Skip((int)individuals.Length / 2).ToArray())
                {
                    FitnessCalc = new MatrixFitnessCalc(Matrix2),
                    Selection = new TournamentSelection(2),
                    CrossOver = new CrossOverOX(),
                    Mutation = new SimpleMutation(Mutation)
                },
                Mixer = new SimpleMixer(),
            };
        }
    }
}
