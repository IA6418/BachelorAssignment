using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.CommonComponents.Interfaces
{
    public interface IActivationFunction
    {
        double ActivationFunctionOutput(double sum);

        double DerivedActivationFunctionOutput(double sum);

        string GetActivationFunctionAsString(string input, double bias);
    }
}
