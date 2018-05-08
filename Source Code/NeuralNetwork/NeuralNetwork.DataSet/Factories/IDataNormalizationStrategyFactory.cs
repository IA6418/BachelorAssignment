using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.DataSet.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.Factories
{
    public interface IDataNormalizationStrategyFactory
    {
        IDataNormalizationStrategy CreateNormalizationStrategy(NormalizationType normalizationType);
    }
}
