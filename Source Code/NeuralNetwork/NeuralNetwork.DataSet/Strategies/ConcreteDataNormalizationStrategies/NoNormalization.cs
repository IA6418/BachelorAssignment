namespace NeuralNetwork.DataSet.Strategies.ConcreteDataNormalizationStrategies
{
    public class NoNormalization : IDataNormalizationStrategy
    {
        public double[,] NormalizeData(double[,] dataSet, int numberOfInputs)
        {
            return dataSet;
        }
    }
}
