using NeuralNetwork.NeuralNetwork.DataSet.Strategies;

namespace NeuralNetwork.DataSet.Strategies.ConcreteDataEncodingStrategies
{
    public class NoEncoding : IDataEncodingStrategy
    {
        public string[,] Encode(string[,] dataSet, int[] columnIndecesToEncode)
        {
            return dataSet;
        }
    }
}
