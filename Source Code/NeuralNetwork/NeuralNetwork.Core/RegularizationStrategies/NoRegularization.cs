using NeuralNetwork.CommonComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.RegularizationStrategies
{
    class NoRegularization : IRegularizationStrategy
    {
        public double Regularize(List<double[,]> weights)
        {
            return 0.0;
        }
    }
}
