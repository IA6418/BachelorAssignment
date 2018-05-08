using NeuralNetwork.CommonComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.ActivationFunctions
{
    public class SoftPlusFunction : IActivationFunction
    {
        public double ActivationFunctionOutput(double sum)
        {
            return Math.Log(Math.Exp(sum) + 1);
        }

        public double DerivedActivationFunctionOutput(double sum)
        {
            return 1 / (1 + Math.Exp(-sum));
        }

        public string GetActivationFunctionAsString(string input, double bias)
        {
            throw new NotImplementedException();
        }
    }
}
