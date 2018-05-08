using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.CommonComponents.Interfaces;
using NeuralNetwork.Core.RegularizationStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.Factories
{
    public class RegularizationStrategyFactory : IRegularizationStrategyFactory
    {
        public IRegularizationStrategy CreateRegularizationStrategy(RegularizationType regularizationType)
        {
            switch (regularizationType)
            {
                case RegularizationType.L1:
                    return new L1();
                case RegularizationType.L2:
                    return new L2();
                case RegularizationType.None:
                    return new NoRegularization();
                default:
                    return new L2();
            }
        }
    }
}
