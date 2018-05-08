using NeuralNetwork.DataSet.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.Factories
{
    public interface IDataParseStrategyFactory
    {
        IDataParseStrategy CreateParsingStrategy(string fileExtension);
    }
}
