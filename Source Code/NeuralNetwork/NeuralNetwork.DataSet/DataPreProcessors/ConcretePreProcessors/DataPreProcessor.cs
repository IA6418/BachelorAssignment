using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.CommonComponents.Structs;
using NeuralNetwork.DataSet.Factories;
using NeuralNetwork.DataSet.Factories.ConcreteFactories;
using NeuralNetwork.DataSet.Strategies;
using NeuralNetwork.NeuralNetwork.DataSet.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.DataPreProcessors.ConcretePreProcessors
{
    public class DataPreProcessor : IDataPreProcessor
    {
        IDataEncodingStrategyFactory _encodingStrategyFactory;
        IDataNormalizationStrategyFactory _normalizationStrategyFactory;

        IDataEncodingStrategy _encodingStrategy;
        IDataNormalizationStrategy _normalizationStrategy;
       

        public DataPreProcessor()
        {
            //These are interfaces and therefore could be used for IOC later on
            _encodingStrategyFactory = new DataEncodingStrategyFactory(); 
            _normalizationStrategyFactory = new DataNormalizationStrategyFactory();
        }

        public double[,] PreProcessData(string[,] rawDataSet, NormalizationType normalizationType, int numberOfInputs, EncodingType encodingType, int[] columnIndexesToEncode)
        {
            var rawEncodedDataSet = EncodeData(rawDataSet, encodingType, columnIndexesToEncode);

            var encodedDataSet = ParseDataSetToDouble(rawEncodedDataSet);

            var normalizedAndEncodedDataSet = NormalizeData(encodedDataSet, normalizationType, numberOfInputs);

            return normalizedAndEncodedDataSet;
        }

        private string[,] EncodeData(string[,] rawDataSet, EncodingType encodingType, int[] columnIndexesToEncode)
        {
            _encodingStrategy = _encodingStrategyFactory.CreateEncodingStrategy(encodingType);

            var encodedDataSet = _encodingStrategy.Encode(rawDataSet, columnIndexesToEncode);

            return encodedDataSet;          
        }

        private double[,] NormalizeData(double[,] datSetToNormalize, NormalizationType normalizationType, int numberOfInputs)
        {
            _normalizationStrategy = _normalizationStrategyFactory.CreateNormalizationStrategy(normalizationType);

            var normalizedDataSet = _normalizationStrategy.NormalizeData(datSetToNormalize, numberOfInputs);

            return normalizedDataSet;
        }




        public double[,] ParseDataSetToDouble(string[,] dataSet)
        {
            
            int numberOfRows = dataSet.GetLength(0);
            int numberOfColumns = dataSet.GetLength(1);

            double[,] numericallyParsedDataSet = new double[numberOfRows, numberOfColumns];

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    double parsedValue;
                    var valueToParse = dataSet[i, j];

                    Double.TryParse(valueToParse, out parsedValue);

                    numericallyParsedDataSet[i, j] = parsedValue;

                }
            }

            return numericallyParsedDataSet;
        }
    }
}
