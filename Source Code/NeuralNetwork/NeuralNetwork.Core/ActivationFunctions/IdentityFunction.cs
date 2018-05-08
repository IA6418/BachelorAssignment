using NeuralNetwork.CommonComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.ActivationFunctions
{
    public class IdentityFunction : IActivationFunction
    {
        public double ActivationFunctionOutput(double sum)
        {
            return 1 * sum;
        }

        public double DerivedActivationFunctionOutput(double sum)
        {
            return 1;
        }

        public string GetActivationFunctionAsString(string input, double bias)
        {
            return string.Format("{0} + {1}", input, bias);
        }
    }
}
