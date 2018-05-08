using NeuralNetwork.CommonComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.ActivationFunctions
{
    public class SigmoidFunction : IActivationFunction
    {
        public double ActivationFunctionOutput(double sum)
        {
            var result = 1 / (1 + Math.Exp(-sum));

            return result;
        }

        public double DerivedActivationFunctionOutput(double sum)
        {
            var sigmoid = this.ActivationFunctionOutput(sum);

            return  (1 - sigmoid) * sigmoid;

        }

        public string GetActivationFunctionAsString(string input, double bias)
        {
            string functionString = string.Format("1 / (1 + exp(-(({0}) + ({1}))))", input, bias);

            return functionString;
        }
    }
}
