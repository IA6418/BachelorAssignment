using DataSet.DataSets;
using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.CommonComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.NetworkEvaluators
{
    public class NetworkEvaluator : INetworkEvaluator
    {
        private INetwork _network;

        public NetworkEvaluator(INetwork network)
        {
            _network = network;
        }

        /// <summary>
        /// Evaluates a trained classification networks ability to predict on unsees test data
        /// </summary>
        /// <returns></returns>
        public double EvaluateNetwork()
        {
            double evaluationScore = EvaluateClassificationNetwork(_network.DataSet.TestSet);

            Console.WriteLine("The trained network predicts the test set with " + (evaluationScore * 100) + "% accuracy");
            return evaluationScore;

        }

        /// <summary>
        /// Evaluates a trained regression networks ability to predict on unsees test data
        /// </summary>
        /// <param name="minimumAllowedDeviation">Minimum allowed deviation between predicted and target value</param>
        /// <returns></returns>
        public double EvaluateNetwork(double minimumAllowedDeviation)
        {
            double evaluationScore = EvaluateRegressionNetwork(minimumAllowedDeviation, _network.DataSet.TestSet);

            Console.WriteLine("The trained network predicts the test set with " + (evaluationScore * 100) + "% accuracy");
            return evaluationScore;
        }

        public double EvaluateRegressionNetwork(double maximumAllowedError, ISubDataSet subDataSet)
        {
            int numberOfDataRowsInTestSet = subDataSet.NumberOfDataRows;
            int numberOfCorrectPredictions = 0;
            double predictionRate = 0.0;

            //1. feed forward through test set
            for (int i = 0; i < numberOfDataRowsInTestSet; i++)
            {
                var inputMatrix = subDataSet.InputMatrices[i];
                var targetValues = subDataSet.OutputMatrices[i];

                var feedForwardData = _network.FeedForward(inputMatrix);

                int indexOfPredictedValues = feedForwardData.LayerOutputs.Count - 1;
                int numberOfPredictedValues = feedForwardData.LayerOutputs[indexOfPredictedValues].GetLength(0);
                var predictedValues = feedForwardData.LayerOutputs[indexOfPredictedValues];

                for (int j = 0; j < numberOfPredictedValues; j++)
                {
                    double predictedValue = predictedValues[j];
                    double targetValue = targetValues[j];
                    double differenceBetweenPredictedAndTargetValue = Math.Abs(predictedValue - targetValue);

                    //2. compare ||predicted - target|| <= maximum allowed deviance
                    if (differenceBetweenPredictedAndTargetValue <= maximumAllowedError)
                    {
                        numberOfCorrectPredictions++;
                    }
                }
            }
            //3. Get the successful prediction %
            predictionRate = (double)numberOfCorrectPredictions / (double)numberOfDataRowsInTestSet;

           // Console.WriteLine("The Regression Network predicts the test set with " + (predictionRate * 100) + "% accuracy");
            return predictionRate;

        }

        //for multiple-output classification network

        public double EvaluateClassificationNetwork(ISubDataSet subDataSet)
        {
            double predictionRate = 0.0;
            int numberOfDataRowsInTestSet = subDataSet.NumberOfDataRows;
            int numberOfCorrectPredictions = 0;

            //1. feed forward through test set
            for (int i = 0; i < numberOfDataRowsInTestSet; i++)
            {
                var inputMatrix = subDataSet.InputMatrices[i];
                var targetValues = subDataSet.OutputMatrices[i];

                var feedForwardData = _network.FeedForward(inputMatrix);

                int indexOfPredictedValues = feedForwardData.LayerOutputs.Count - 1;
                int numberOfPredictedValues = feedForwardData.LayerOutputs[indexOfPredictedValues].GetLength(0);
                var predictedValues = feedForwardData.LayerOutputs[indexOfPredictedValues];

                var indexOfCorrectTargetValue = 0;
                var indexOPredictedValue = 0;

                var largestTargetValue = targetValues.Max();
                indexOfCorrectTargetValue = targetValues.ToList().IndexOf(largestTargetValue);

                var largestPredictedValue = predictedValues.Max();
                indexOPredictedValue = predictedValues.ToList().IndexOf(largestPredictedValue);

                //2. compare the categorical index of the predicted and target values
                if (indexOPredictedValue == indexOfCorrectTargetValue)
                {
                    numberOfCorrectPredictions++;
                }

            }
            //3. Get the successful prediction %
            predictionRate = (double)numberOfCorrectPredictions / (double)numberOfDataRowsInTestSet;

            //Console.WriteLine("The Classification Network predicts the test set with " + (predictionRate * 100) + "% accuracy");
            return predictionRate;
        }



    }
}
