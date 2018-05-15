using NeuralNetwork.CommonComponents.Structs;
using NeuralNetwork.CommonComponents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.CommonComponents.Interfaces
{
    public interface IOptimizationStrategy
    {
        double HiddenLayerActivate(double sum);

        double OutputLayerActivate(double sum);

        string GetHiddenActivationFunctionAsString(string input, double bias);

        string GetOutputActivationFunctionAsString(string input, double bias);

        void FetchInitialWeightsAndBiases(ref List<double[,]> weights, ref List<double[]> biases);

        void FetchPreviousDeltaWeightsAndBiases(ref List<double[,]> previousDeltaWeights, ref List<double[]> previousDeltaBiases);

        LayerStructure LayerStructure { get; set; }

        double Backpropagate(FeedForwardData feedForwardData, double[] targetOutputs, TrainingParameters trainingParams, int timeStep);

        double AccumulatedError { get; set; }

        IActivationFunction HiddenActivationFunction { get; set; }

        IActivationFunction OutputActivationFunction { get; set; }

        ICostFunction CostFunction { get; set; }

        IRegularizationStrategyFactory RegularizationStrategyFactory { get; set; }

        RegularizationType RegularizationType { get; set; }
    }
}
