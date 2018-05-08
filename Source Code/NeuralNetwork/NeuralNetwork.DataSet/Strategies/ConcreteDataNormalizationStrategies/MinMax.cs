using System.Linq;

namespace NeuralNetwork.DataSet.Strategies.ConcreteDataNormalizationStrategies
{
    /// <summary>
    /// Min-Max normalization with interval 0 - 1
    /// </summary>
    public class MinMax : IDataNormalizationStrategy
    {
        public double[,] NormalizeData(double[,] dataSet, int numberOfInputs)
        {
            int numberOfDataRows = dataSet.GetLength(0);
            int numberOfDataColumns = dataSet.GetLength(1);

            double[,] normalizedDataSet = dataSet;

            double[] columnValues = new double[numberOfDataRows];

            for (int i = 0; i < numberOfInputs; i++)
            {               
                for (int j = 0; j < numberOfDataRows; j++)
                {
                    columnValues[j] = dataSet[j, i];
                }

                double minValue = columnValues.Min();
                double maxValue = columnValues.Max();

                for (int k = 0; k < numberOfDataRows; k++)
                {
                    double currentValue = dataSet[k, i];
                    normalizedDataSet[k, i] = MinMaxNormalizedDataValue(minValue, maxValue, currentValue);
                }
            }

            return normalizedDataSet;
            

        }

        private double MinMaxNormalizedDataValue(double minValue, double maxValue, double currentValue)
        {
            double normalizedDataValue = (currentValue - minValue) / (maxValue - minValue);

            if (double.IsNaN(normalizedDataValue))
            {
                normalizedDataValue = 0;
            }

            return normalizedDataValue;
        }
    }
}
