using NeuralNetwork.DataSet.Factories;
using NeuralNetwork.DataSet.Factories.ConcreteFactories;
using NeuralNetwork.DataSet.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.DataParsers.ConcreteParsers
{
    public class DataParser : IDataParser
    {
        private IDataParseStrategyFactory _parsingStrategyFactory;
        private IDataParseStrategy _parsingStrategy;

        public DataParser()
        {
            _parsingStrategyFactory = new DataParseStrategyFactory();
        }

        public string[,] ParseData(string filePath, string fileExtension, bool firstRowHeader)
        {
            _parsingStrategy = _parsingStrategyFactory.CreateParsingStrategy(fileExtension);

            string[,] rawDataSet = _parsingStrategy.ParseData(filePath, firstRowHeader);

            return rawDataSet;
        }
    }
}
