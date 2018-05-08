using NeuralNetwork.CommonComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.CrossValidationStrategies
{
    public class HoldOut : ICrossValidationStrategy
    {
        public double CrossValidate()
        {
            throw new NotImplementedException();
        }

        public double CrossValidate(double[,] dataSet, int KNumberOfFolds, double maximumAllowedDeviation)
        {
            throw new NotImplementedException();
        }

        public double CrossValidate(double[,] dataSet, int KNumberOfFolds)
        {
            throw new NotImplementedException();
        }
    }
}
