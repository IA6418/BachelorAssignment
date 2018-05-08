using DataSet.DataSets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.DataSets
{
    public class TrainingDataSet : ISubDataSet
    {
        public double[,] DataSet { get; set; }
        

        public List<double[]> InputMatrices { get; set; }

        public List<double[]> OutputMatrices { get; set; }

        private int _numberOfInputVariables;
        private int _numberOfOutputVariables;

        public int NumberOfDataRows
        {
            get { return DataSet.GetLength(0); }
        }

        public TrainingDataSet(double[,] dataSet, int numberOfInputVariables)
        {
            DataSet = dataSet;

            _numberOfInputVariables = numberOfInputVariables;
            _numberOfOutputVariables = DataSet.GetLength(1) - numberOfInputVariables;

            CreateInputAndOutputMatrices();
        }

        public void CreateInputAndOutputMatrices()
        {
            CreateInputMatrices();

            CreateOutputMatrices();

        }

        private void CreateInputMatrices()
        {
            InputMatrices = new List<double[]>();

            for (int i = 0; i < NumberOfDataRows; i++)
            {
                var inputMatrix = new double[_numberOfInputVariables];

                for (int j = 0; j < _numberOfInputVariables; j++)
                {
                    inputMatrix[j] = DataSet[i, j];
                }

                InputMatrices.Add(inputMatrix);

            }
        }

        private void CreateOutputMatrices()
        {
            OutputMatrices = new List<double[]>();

            for (int i = 0; i < NumberOfDataRows; i++)
            {
                var outputMatrix = new double[_numberOfOutputVariables];

                for (int j = 0; j < _numberOfOutputVariables; j++)
                {
                    outputMatrix[j] = DataSet[i, _numberOfInputVariables+j];
                }

                OutputMatrices.Add(outputMatrix);
            }
        }


    }
}
