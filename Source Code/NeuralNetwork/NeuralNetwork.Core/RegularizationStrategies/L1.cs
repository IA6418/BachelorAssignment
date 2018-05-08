using NeuralNetwork.CommonComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.RegularizationStrategies
{
    public class L1 : IRegularizationStrategy
    {
        public double Regularize(List<double[,]> weights)
        {
            double absoluteSumOfWeights = 0.0;

            int numberOfWeightLayers = weights.Count - 1;

            for (int i = 0; i < numberOfWeightLayers; i++)
            {
                int numberOfWeightRows = weights[i].GetLength(0);
                int numberOfWeightColumns = weights[i].GetLength(1);

                for (int j = 0; j < numberOfWeightRows; j++)
                {
                    for (int k = 0; k < numberOfWeightColumns; k++)
                    {
                        absoluteSumOfWeights += Math.Abs(weights[i][j, k]);
                    }
                }
            }

            return absoluteSumOfWeights;
        }
    }
}
