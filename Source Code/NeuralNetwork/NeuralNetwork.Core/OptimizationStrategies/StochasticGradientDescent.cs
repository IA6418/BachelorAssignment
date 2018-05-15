using LinearAlgebra;
using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.CommonComponents.Interfaces;
using NeuralNetwork.CommonComponents.Structs;
using NeuralNetwork.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.OptimizationStrategies
{    

    public class StochasticGradientDescent : IOptimizationStrategy
    {
        private IActivationFunction _hiddenActivationFunction;
        private IActivationFunction _outputActivationFunction;
        private ICostFunction _costFunction;
        private IRegularizationStrategyFactory _regularizationStrategyFactory;
        private IRegularizationStrategy _regularizationStrategy;
        private RegularizationType _regularizationType;
  
        private double[] _outputGradientMatrix;
        private List<double[]> _gradientMatrices = new List<double[]>();

        private List<double[,]> _weights = new List<double[,]>();
        private List<double[,]> _deltaWeightMatrices = new List<double[,]>();
        private List<double[,]> _previousDeltaWeights = new List<double[,]>();

        private List<double[]> _biases = new List<double[]>();
        private List<double[]> _deltaBiasMatrices = new List<double[]>();
        private List<double[]> _previousDeltaBiases = new List<double[]>();


        //adam parameters with recommended default values-----------8::::::::::::>---------------------

        private double _Beta1 = 0.9;
        private double _Beta2 = 0.999;
        private double _epsilon = Math.Pow(10, -8);
        private double _m;
        private double _v;

        public LayerStructure LayerStructure { get; set; }
        private double[] DerivedErrorMatrix { get; set; }
        public double AccumulatedError { get; set; }
        public double LearningRate { get; set; }
        public double Momentum { get; set; }
        public int Epochs { get; set; }

       

        public StochasticGradientDescent(IActivationFunction hiddenActivationFunction, IActivationFunction outputActivationFunction, ICostFunction costFunction,
                                        RegularizationType regularizationType, IRegularizationStrategyFactory regularizationStrategyFactory)
        {
            _hiddenActivationFunction = hiddenActivationFunction;
            _outputActivationFunction = outputActivationFunction;
            _costFunction = costFunction;
            _regularizationType = regularizationType;
            _regularizationStrategyFactory = regularizationStrategyFactory; //would normally solve this with IOC
        }

        public IActivationFunction HiddenActivationFunction 
        { 
            get { return _hiddenActivationFunction; } 
            set { _hiddenActivationFunction = value; } 
        }
        public IActivationFunction OutputActivationFunction 
        { 
            get { return _outputActivationFunction; } 
            set { _outputActivationFunction = value; }
        }
        public ICostFunction CostFunction 
        {
            get { return _costFunction; }
            set { _costFunction = value; }
        }
        public IRegularizationStrategyFactory RegularizationStrategyFactory
        {
            get { return _regularizationStrategyFactory; }
            set { _regularizationStrategyFactory = value; }
        }
        public RegularizationType RegularizationType
        {
            get { return _regularizationType; }
            set { _regularizationType = value; }
        }


        public double Backpropagate(FeedForwardData feedForwardData, double[] targetOutputs, TrainingParameters trainingParams, int timeStep) //public double Backpropagate(FeedForwardData feedForwardData, double[] targetOutputs, TrainingParameters trainingParams)
        {

            //1. compute error
            int outputLayerIndex = LayerStructure.HiddenLayerList.Count + 1;
            var predictedOutputs = feedForwardData.LayerOutputs[outputLayerIndex];

            GenerateErrorMatrix(predictedOutputs, targetOutputs, trainingParams);
            ComputeAccumulatedError(predictedOutputs, targetOutputs);

            //2. compute gradients
            ComputeGradients(feedForwardData);

            //3. compute delta weights and biases
            ComputeDeltaWeights(feedForwardData);
            ComputeDeltaBiases();

            //4. update weights and biases
            UpdateWeights(trainingParams);
            UpdateBiases(trainingParams);


            //adam update-----------------8::::::::::::>---------------------------

            //UpdateWeightsAdam(trainingParams, timeStep);
            //UpdateBiasesAdam(trainingParams, timeStep);

            //reset lists
            ClearLists();

            return AccumulatedError;

        }

        private void GenerateErrorMatrix(double[] predictedOutputs, double[] targetOutputs, TrainingParameters trainingParams)
        {
            int numberOfOutputNeurons = LayerStructure.numberOfOutputNodes;
            DerivedErrorMatrix = new double[numberOfOutputNeurons];

            var regularizationPenaltyTerm = CalculateRegularizationPenalty(_weights, trainingParams.RegularizationLambda);

            for (int i = 0; i < numberOfOutputNeurons; i++)
            {
                DerivedErrorMatrix[i] = _costFunction.DerivedCostFunction(predictedOutputs[i], targetOutputs[i]) + regularizationPenaltyTerm;
            }
        }

        private double CalculateRegularizationPenalty(List<double[,]> weights, double lambda)
        {
            var penalityTerm = 0.0;

            _regularizationStrategy = _regularizationStrategyFactory.CreateRegularizationStrategy(_regularizationType);

            penalityTerm = _regularizationStrategy.Regularize(weights);

            return penalityTerm * lambda;

        }

        private void ComputeAccumulatedError(double[] predictedOutputs, double[] targetOutputs)
        {
            int numberOfOutputNeurons = LayerStructure.numberOfOutputNodes;

            AccumulatedError = 0.0;

            for (int i = 0; i < numberOfOutputNeurons; i++)
            {                               
                AccumulatedError += _costFunction.CostFunction(predictedOutputs[i], targetOutputs[i]);
            }
        }

        private void ComputeGradients(FeedForwardData feedForwardData)
        {
            //output gradients
            int numberOfOutputNeurons = LayerStructure.numberOfOutputNodes;
            _outputGradientMatrix = new double[numberOfOutputNeurons];

            int outputLayerIndex = feedForwardData.LayerInputs.Count - 1;
            var outputLayerInputs = feedForwardData.LayerOutputs[outputLayerIndex];

            for (int i = 0; i < numberOfOutputNeurons; i++)
            {
                _outputGradientMatrix[i] = DerivedErrorMatrix[i] * _outputActivationFunction.DerivedActivationFunctionOutput(outputLayerInputs[i]);
            }

            _gradientMatrices.Add(_outputGradientMatrix);

            //hidden layer gradients
            int numberOfHiddenLayers  = LayerStructure.HiddenLayerList.Count;

            var forwardLayerGradients = _outputGradientMatrix;

            for (int i = numberOfHiddenLayers; i > 0;  i--)
            {
                                            
                var currentWeightMatrix = Matrix.Transpose(_weights[i]); //this was transposed in the original... why?!
                
                var currentHiddenLayerInputs = feedForwardData.LayerInputs[i];          //this is for regression
                //var currentHiddenLayerInputs = feedForwardData.LayerOutputs[i];       //this is for classification

                int numberOfNeuronsInCurrentLayer = feedForwardData.LayerOutputs[i].GetLength(0);
                int numberOfNeuronsInForwardLayer = feedForwardData.LayerOutputs[i+1].GetLength(0);

                var hiddenGradientMatrix = new double[numberOfNeuronsInCurrentLayer];

                for (int j = 0; j < numberOfNeuronsInCurrentLayer; j++)
                {
                    double weightGradientSum = 0.0;

                    for (int k = 0; k < numberOfNeuronsInForwardLayer; k++)
                    {
                        weightGradientSum += currentWeightMatrix[j, k] * forwardLayerGradients[k];
                    }

                    hiddenGradientMatrix[j] = weightGradientSum * _hiddenActivationFunction.DerivedActivationFunctionOutput(currentHiddenLayerInputs[j]);

                }

                _gradientMatrices.Add(hiddenGradientMatrix);

                forwardLayerGradients = hiddenGradientMatrix;
            }
        }

        private void ComputeDeltaWeights(FeedForwardData feedForward)
        {
            int numberOfWeightMatrices = _weights.Count;
            _gradientMatrices.Reverse();  // gradients right-to-left, delta weights left-to-right
            for (int i = 0; i < numberOfWeightMatrices; i++)
            {
                int numberOfRowsInCurrentWeightMatrix = _weights[i].GetLength(0);
                int numberOfColumnsInCurrentWeightMatrix = _weights[i].GetLength(1);
                var deltaWeightMatrix = new double[numberOfRowsInCurrentWeightMatrix, numberOfColumnsInCurrentWeightMatrix];
                
                var forwardGradientMatrix = _gradientMatrices[i];

                var penultimateLayerOutputValues = feedForward.LayerOutputs[i];
                int indexOfNeuronInPenultimateLayer = 0;

                for (int j = 0; j < numberOfColumnsInCurrentWeightMatrix; j++)
                {

                    for (int k = 0; k < numberOfRowsInCurrentWeightMatrix; k++)
                    {
                        deltaWeightMatrix[k, j] = forwardGradientMatrix[k] 
                                                  * penultimateLayerOutputValues[indexOfNeuronInPenultimateLayer];
                    }

                    indexOfNeuronInPenultimateLayer++;
                }

                _deltaWeightMatrices.Add(deltaWeightMatrix);

            }
        }

        private void ComputeDeltaBiases()
        {
            int numberOfBiasMatrices = _biases.Count;

            for (int i = 0; i < numberOfBiasMatrices; i++)
            {
                int numberOfRowsInCurrentBiasMatrix = _biases[i].GetLength(0);

                var deltaBiasMatrix = new double[numberOfRowsInCurrentBiasMatrix];

                var currentGradientMatrix = _gradientMatrices[i];

                for (int j = 0; j < numberOfRowsInCurrentBiasMatrix; j++)
                {
                    deltaBiasMatrix[j] = currentGradientMatrix[j];
                }

                _deltaBiasMatrices.Add(deltaBiasMatrix);
            }
        }

        private void UpdateWeights(TrainingParameters trainingParameters) 
        {

            int numberOfWeightMatrices = _weights.Count;

            for (int i = 0; i < numberOfWeightMatrices; i++)
            {
                int numberOfWeightRows = _weights[i].GetLength(0);
                int numberOfWeightColumns = _weights[i].GetLength(1);

                for (int j = 0; j < numberOfWeightRows; j++)
                {
                    for (int k = 0; k < numberOfWeightColumns; k++)
                    {
                        var deltaWeight = _deltaWeightMatrices[i][j, k] * trainingParameters.learningRate;

                        _weights[i][j, k] -= (deltaWeight + (trainingParameters.momentum * _previousDeltaWeights[i][j, k]));

                        _previousDeltaWeights[i][j, k] = deltaWeight;

                    }
                }
            }
        }

        private void UpdateWeightsAdam(TrainingParameters trainingParameters, int timeStep) 
        {

            int numberOfWeightMatrices = _weights.Count;

            for (int i = 0; i < numberOfWeightMatrices; i++)
            {
                int numberOfWeightRows = _weights[i].GetLength(0);
                int numberOfWeightColumns = _weights[i].GetLength(1);

                for (int j = 0; j < numberOfWeightRows; j++)
                {
                    for (int k = 0; k < numberOfWeightColumns; k++)
                    {
                        timeStep += 1;

                        _m = _Beta1 * _m + (1 - _Beta1) * _deltaWeightMatrices[i][j, k];

                        double mt = _m / (1 - Math.Pow(_Beta1, timeStep));

                        _v = _Beta2 * _v + (1 - _Beta2) * Math.Pow(_deltaWeightMatrices[i][j, k], 2);

                        double vt = _v / (1 - Math.Pow(_Beta2, timeStep));

                        var deltaWeight = trainingParameters.learningRate * (mt / Math.Sqrt(vt + _epsilon)); 

                        _weights[i][j, k] -= deltaWeight + _previousDeltaWeights[i][j, k];

                        _previousDeltaWeights[i][j, k] = deltaWeight;
                    }
                }
            }
        }

        private void UpdateBiases(TrainingParameters trainingParameters) 
        {
            int numberOfBiasMatrices = _biases.Count;

            for (int i = 0; i < numberOfBiasMatrices; i++)
            {
                int numberOfBiasRows = _biases[i].GetLength(0);

                for (int j = 0; j < numberOfBiasRows; j++)
                {
                    var deltaBias = _deltaBiasMatrices[i][j] * trainingParameters.learningRate;

                    _biases[i][j] -= (deltaBias + (trainingParameters.momentum * _previousDeltaBiases[i][j]));

                    _previousDeltaBiases[i][j] = deltaBias;
                }
            }
        }


        private void UpdateBiasesAdam(TrainingParameters trainingParameters, int timeStep) 
        {
            int numberOfBiasMatrices = _biases.Count;

            for (int i = 0; i < numberOfBiasMatrices; i++)
            {
                int numberOfBiasRows = _biases[i].GetLength(0);

                for (int j = 0; j < numberOfBiasRows; j++)
                {
                    timeStep += 1;

                    _m = _Beta1 * _m + (1 - _Beta1) * _deltaBiasMatrices[i][j];

                    double mt = _m / (1 - Math.Pow(_Beta1, timeStep));

                    _v = _Beta2 * _v + (1 - _Beta2) * Math.Pow(_deltaBiasMatrices[i][j], 2);

                    double vt = _v / (1 - Math.Pow(_Beta2, timeStep));

                    var deltaBias = trainingParameters.learningRate * (mt / Math.Sqrt(vt + _epsilon));

                    _biases[i][j] -= deltaBias + _previousDeltaBiases[i][j];

                    _previousDeltaBiases[i][j] = deltaBias;
                }
            }

        }

        private void ClearLists()
        {
            _deltaWeightMatrices.Clear();

            _deltaBiasMatrices.Clear();
            
            _gradientMatrices.Clear();
            
        }

        public double HiddenLayerActivate(double sum)
        {
            return _hiddenActivationFunction.ActivationFunctionOutput(sum);
        }

        public double OutputLayerActivate(double sum)
        {
            return _outputActivationFunction.ActivationFunctionOutput(sum);
        }

        public void FetchInitialWeightsAndBiases(ref List<double[,]> weights, ref List<double[]> biases)
        {
            _weights = weights;
            _biases = biases;
        }

        public void FetchPreviousDeltaWeightsAndBiases(ref List<double[,]> previousDeltaWeights, ref List<double[]> previousDeltaBiases)
        {
            _previousDeltaWeights = previousDeltaWeights;
            _previousDeltaBiases = previousDeltaBiases;
        }

        public string GetHiddenActivationFunctionAsString(string input, double bias)
        {
            return _hiddenActivationFunction.GetActivationFunctionAsString(input, bias);
        }

        public string GetOutputActivationFunctionAsString(string input, double bias)
        {
            return _outputActivationFunction.GetActivationFunctionAsString(input, bias);
        }
    }
}

