using NeuralNetwork.DataSet.Strategies;
using NeuralNetwork.DataSet.Strategies.ConcreteStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.Factories.ConcreteFactories
{
    public class DataParseStrategyFactory : IDataParseStrategyFactory
    {
        public IDataParseStrategy CreateParsingStrategy(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".csv":
                    return new CSVParsingStrategy();
                default:
                    return new CSVParsingStrategy();
            }
        }
    }
}
