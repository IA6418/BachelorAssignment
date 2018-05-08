using NeuralNetwork.CommonComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.CostFunctions
{
    public class MeanSquaredError : ICostFunction
    {
        public double CostFunction(double predicted, double target)
        {
            var error = predicted - target;

            error = (Math.Pow(error, 2)) * 0.5;

            return error;
        }

        public double DerivedCostFunction(double predicted, double target)
        {
            return (predicted - target);
        }
    }
}
