using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.DataSet.Strategies;
using NeuralNetwork.DataSet.Strategies.ConcreteDataNormalizationStrategies;

namespace NeuralNetwork.DataSet.Factories.ConcreteFactories
{
    public class DataNormalizationStrategyFactory : IDataNormalizationStrategyFactory
    {
        public IDataNormalizationStrategy CreateNormalizationStrategy(NormalizationType normalizationType)
        {
            switch (normalizationType)
            {
                case NormalizationType.None:
                    return new NoNormalization();
                case NormalizationType.Gaussian:
                    return new Gaussian();
                case NormalizationType.MinMax:
                    return new MinMax();
                case NormalizationType.NegativePositiveMinMax:
                    return new NegativePositiveMinMax();
                default:
                    return new NoNormalization();
            }
        }
    }
}
