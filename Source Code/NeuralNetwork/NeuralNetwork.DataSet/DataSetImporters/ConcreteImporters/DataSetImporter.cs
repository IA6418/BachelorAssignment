using NeuralNetwork.DataSet.DataParsers;
using System.IO;

namespace NeuralNetwork.DataSet.DataSetImporter.ConcreteImporters
{
    public class DataSetImporter : IDataSetImporter
    {
        private IDataParser _dataParser;

        public DataSetImporter()
        {
            _dataParser = new DataParsers.ConcreteParsers.DataParser();
        }

        public string[,] ImportDataFileIntoDataSet(string dataFilePath, bool firstRowHeader)
        {
            var _fileExtension = GetFileExtension(dataFilePath);
            var dataSet = _dataParser.ParseData(dataFilePath, _fileExtension, firstRowHeader);

            return dataSet;
        }

        public string GetFileExtension(string dataFilePath)
        {
            return Path.GetExtension(dataFilePath);
        }
    }
}
