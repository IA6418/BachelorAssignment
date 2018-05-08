using NeuralNetwork.CommonComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.ActivationFunctions
{
    public class TanhFunction : IActivationFunction
    {
        public double ActivationFunctionOutput(double sum)
        {
            return 2 / (1 + Math.Pow(Math.E, -(2 * sum))) - 1;
        }

        public double DerivedActivationFunctionOutput(double sum)
        {
            var tanh = ActivationFunctionOutput(sum);

            return 1 - Math.Pow(tanh, 2);
        }

        public string GetActivationFunctionAsString(string input, double bias)
        {
            string functionString = string.Format("2 / (1 + (exp(-(2 * {0} + {1}))^2) - 1", input, bias);

            return functionString;
        }
    }
}
