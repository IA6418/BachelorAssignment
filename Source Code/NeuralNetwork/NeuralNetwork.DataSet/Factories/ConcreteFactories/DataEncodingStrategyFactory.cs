using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.NeuralNetwork.DataSet.Strategies;
using NeuralNetwork.DataSet.Strategies.ConcreteDataEncodingStrategies;

namespace NeuralNetwork.DataSet.Factories.ConcreteFactories
{
    public class DataEncodingStrategyFactory : IDataEncodingStrategyFactory
    {
        public IDataEncodingStrategy CreateEncodingStrategy(EncodingType encodingType)
        {
            switch (encodingType)
            {
                case EncodingType.None:
                    return new NoEncoding();
                case EncodingType.OneHot:
                    return new OneHot();
                default:
                    return new NoEncoding();
            }
        }
    }
}
