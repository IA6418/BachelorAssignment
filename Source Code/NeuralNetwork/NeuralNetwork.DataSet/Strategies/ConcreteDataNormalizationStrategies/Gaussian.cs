using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.Strategies.ConcreteDataNormalizationStrategies
{
    public class Gaussian : IDataNormalizationStrategy
    {
        public double[,] NormalizeData(double[,] dataSet, int numberOfInputs)
        {
            int numberOfDataRows = dataSet.GetLength(0);
            int numberOfDataColumns = dataSet.GetLength(1);

            double mean = 0.0;
            double standardDeviation = 0.0;

            double[,] normalizedDataSet = dataSet;

            double[] columnValues = new double[numberOfDataRows];

            for (int i = 0; i < numberOfInputs; i++)
            {
                //get column values
                for (int j = 0; j < numberOfDataRows; j++)
                {
                    columnValues[j] = dataSet[j, i];
                }

                mean = CalculateMean(columnValues);
                standardDeviation = CalculateStandardDeviation(columnValues, mean);

                for (int k = 0; k < numberOfDataRows; k++)
                {
                    double currentValue = dataSet[k, i];
                    normalizedDataSet[k, i] = GaussianNormalizedDataValue(currentValue, mean, standardDeviation);
                }
            }

            return normalizedDataSet;
        }

        private double CalculateMean(double[] columnValues)
        {
            double sumOfDataRows = 0.0;
            double meanValue = 0.0;

            int numberOfDataRows = columnValues.GetLength(0);

            for (int i = 0; i < numberOfDataRows; i++)
            {
                sumOfDataRows += columnValues[i];
            }

            meanValue = sumOfDataRows / numberOfDataRows;

            return meanValue;
        }
        private double CalculateStandardDeviation(double[] columnValues, double mean)
        {
            double standardDeviation = 0.0;
            double varianceSum = 0.0;

            int numberOfDataRows = columnValues.GetLength(0);

            for (int i = 0; i < numberOfDataRows; i++)
            {
                double variance = Math.Pow(columnValues[i] - mean,2);
                varianceSum += variance;
            }

            standardDeviation = Math.Sqrt(varianceSum / numberOfDataRows);

            return standardDeviation;
        }

        private double GaussianNormalizedDataValue(double currentValue, double mean, double standardDeviation)
        {
            double normalizedDataValue = (currentValue - mean) / standardDeviation;

            if (double.IsNaN(normalizedDataValue))
            {
                normalizedDataValue = 0;
            }

            return normalizedDataValue;
        }
    }
}
