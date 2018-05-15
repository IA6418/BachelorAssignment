using DataSet.DataSets;
using NeuralNetwork.CommonComponents.Interfaces;
using NeuralNetwork.DataSet.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.CrossValidationStrategies
{
    public class KFold : ICrossValidationStrategy
    {
        private INetwork _network;
        private INetworkEvaluator _evaluator;
        private ISubDataSet _trainingSet;
        private ISubDataSet _validationSet;

        private double[] _predictions;             
        private List<double[,]> _folds = new List<double[,]>();

        public KFold(INetwork network, INetworkEvaluator evaluator)
        {
            _network = network;
            _evaluator = evaluator;
            _trainingSet = new TrainingDataSet(network.DataSet.TrainingSet.DataSet, network.LayerStructure.numberOfInputNodes);
            _validationSet = new ValidationDataSet(network.DataSet.TrainingSet.DataSet, network.LayerStructure.numberOfInputNodes);

    }

        public double CrossValidate(double[,] dataSet, int KNumberOfFolds)
        {
            _predictions = new double[KNumberOfFolds];
            double modelEvaluationScore = 0;
            SplitDataSetIntoFolds(dataSet, KNumberOfFolds);

            int numberOfDataRows = dataSet.GetLength(0);
            int columnSize = dataSet.GetLength(1);

            for (int foldIndex = 0; foldIndex < KNumberOfFolds; foldIndex++)
            {
                GenerateTrainAndValidationSetsFromFolds(numberOfDataRows, columnSize, foldIndex);

               _predictions[foldIndex] = _evaluator.EvaluateClassificationNetwork(_validationSet);
            }

            modelEvaluationScore = CalculateAverageEvaluationScore();

            return modelEvaluationScore;
            
        }

        public double CrossValidate(double[,] dataSet, int KNumberOfFolds, double maximumAllowedDeviation)
        {
            _predictions = new double[KNumberOfFolds];
            double modelEvaluationScore = 0;
            SplitDataSetIntoFolds(dataSet, KNumberOfFolds);

            int numberOfDataRows = dataSet.GetLength(0);
            int columnSize = dataSet.GetLength(1);

            for (int foldIndex = 0; foldIndex < KNumberOfFolds; foldIndex++)
            {
                GenerateTrainAndValidationSetsFromFolds(numberOfDataRows, columnSize, foldIndex);

                _predictions[foldIndex] = _evaluator.EvaluateRegressionNetwork(maximumAllowedDeviation, _validationSet);
               
            }

            modelEvaluationScore = CalculateAverageEvaluationScore();

            return modelEvaluationScore;

        }

        private void SplitDataSetIntoFolds(double[,] dataSet, int KNumberOfFolds)
        {
            int numberOfDataRows = dataSet.Length / dataSet.GetLength(1);

            if (KNumberOfFolds <= dataSet.GetLength(0))
            {

                int dataRowCount = 0;
                

                for (int i = 0; i < KNumberOfFolds; i++)
                {
                    int sizeOfFolds = i < (KNumberOfFolds - 1) ? numberOfDataRows / KNumberOfFolds : numberOfDataRows - dataRowCount;

                    double[,] fold = new double[sizeOfFolds, dataSet.GetLength(1)];

                    for (int j = 0; j < fold.GetLength(0); j++)
                    {
                        for (int k = 0; k < dataSet.GetLength(1); k++)
                        {
                            fold[j, k] = dataSet[dataRowCount, k];

                        }

                        dataRowCount++;

                    }

                    _folds.Add(fold);
                }
            }
            else
            {
                throw new Exception("Number of data rows cannot exceed number of desired folds");
            }

        }

        private void GenerateTrainAndValidationSetsFromFolds(int totalNumberOfDatRows, int columnSize, int validationFoldIndex)
        {

            int sizeOfValidationSet = _folds[validationFoldIndex].GetLength(0);
            int sizeOfTrainingSet = totalNumberOfDatRows - sizeOfValidationSet;

            var trainingSet = new double[sizeOfTrainingSet, columnSize];
            var validationSet = new double[sizeOfValidationSet, columnSize];

            int rowIndex = 0;
            int numberOfFolds = _folds.Count;

            for (int i = 0; i < numberOfFolds; i++)
            {
                if (i != validationFoldIndex)
                {
                    int numberOfRows = _folds[i].GetLength(0);

                    for (int j = 0; j < numberOfRows; j++)
                    {
                        for (int k = 0; k < columnSize; k++)
                        {
                            trainingSet[rowIndex, k] = _folds[i][j, k];

                        }
                        rowIndex++;

                    }
                }

            }

            validationSet = _folds[validationFoldIndex];

            _trainingSet.DataSet = trainingSet;
            _trainingSet.CreateInputAndOutputMatrices();
            _validationSet.DataSet = validationSet;
            _validationSet.CreateInputAndOutputMatrices();

        }

        private double CalculateAverageEvaluationScore()
        {
            int numberOfEvaluations = _predictions.Length;
            double summedEvaluationScore = 0.0;

            for (int i = 0; i < numberOfEvaluations; i++)
            {
                summedEvaluationScore += _predictions[i];
            }

            var averagedEvaluationScore = summedEvaluationScore / numberOfEvaluations;

            return averagedEvaluationScore;
        }


    }

}
