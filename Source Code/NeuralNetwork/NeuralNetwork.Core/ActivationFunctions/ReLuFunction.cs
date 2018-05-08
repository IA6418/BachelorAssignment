using NeuralNetwork.CommonComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.ActivationFunctions
{
    public class ReLuFunction : IActivationFunction
    {
        public double ActivationFunctionOutput(double sum)
        {
            return Math.Max(0, sum * 0.1); //takes care of the dying relu problem
        }

        public double DerivedActivationFunctionOutput(double sum)
        {
            return sum < 0 ? 0 : 0.5;
        }

        public string GetActivationFunctionAsString(string input, double bias)
        {
            throw new NotImplementedException();
        }
    }
}
