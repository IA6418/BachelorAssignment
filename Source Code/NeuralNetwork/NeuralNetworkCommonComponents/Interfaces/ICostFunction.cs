using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.CommonComponents.Interfaces
{
    public interface ICostFunction
    {
        double CostFunction(double actual, double expected);

        double DerivedCostFunction(double actual, double expected);
    }
}
