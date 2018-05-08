using LinearAlgebra;
using NeuralNetwork.CommonComponents.Interfaces;
using NeuralNetwork.CommonComponents.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.Core.Factories;

namespace NeuralNetwork.Network
{
    public class ArtificialNeuralNetwork : INetwork
    {
        public IDataSet DataSet { get; set; }
        private IOptimizationStrategy _strategy;
        private IInitialRandomDistributionType _randomDistribution;
        
        public LayerStructure _layerStructure { get; set; }
        private TrainingParameters _trainingParameters;
        private FeedForwardData _feedForwardData = new FeedForwardData();
        
        private List<double[,]> _weights;
        private List<double[,]> _previousDeltaWeights;
        
        private List<double[]> _biases;      
        private List<double[]> _previousDeltaBiases;

        public List<double> _listOfErrors; 
        

        private string _savePath;

        //private string _filePath;


        public ArtificialNeuralNetwork(LayerStructure layerStructure, TrainingParameters trainingParameters,
                                       IDataSet dataSet, IOptimizationStrategy strategy, IInitialRandomDistributionType randomDistribution)
        {
            _layerStructure = layerStructure;           
            _trainingParameters = trainingParameters;
            DataSet = dataSet;
            _strategy = strategy;
            _strategy.LayerStructure = layerStructure;
            _randomDistribution = randomDistribution;

            CreateDataSets();

            SetWeights();
            InitPreviousDeltaWeights();

            SetBiases();
            InitPreviousDeltaBiases();

            
            _strategy.FetchInitialWeightsAndBiases(ref _weights, ref _biases);

            _strategy.FetchPreviousDeltaWeightsAndBiases(ref _previousDeltaWeights, ref _previousDeltaBiases);

            _savePath = Path.GetTempPath();

        }

        public IOptimizationStrategy Strategy { get { return _strategy; } set { _strategy = value; } }
        public IInitialRandomDistributionType RandomDistribution { get { return _randomDistribution; } set { _randomDistribution = value; } }

        public LayerStructure LayerStructure { get { return _layerStructure; } set { _layerStructure = value; } }

        public TrainingParameters TrainingParameters { get { return _trainingParameters; } set { _trainingParameters = value; } }

        public List<double[,]> Weights { get { return _weights; } set { _weights = value; } }

        public List<double[]> Biases { get { return _biases; } set { _biases = value; } }

        public string SavePath { get { return _savePath; } set { _savePath = value; } }


        private void CreateDataSets()
        {
            DataSet.NumberOfInputs = _layerStructure.numberOfInputNodes;
            DataSet.NumberOfOutputs = _layerStructure.numberOfOutputNodes;
            DataSet.GenerateTrainingAndTestSets();
        }

        private void SetWeights() 
        {
            _weights = new List<double[,]>();

            SetInputWeights();

            SetHiddenWeights();
        }

        private void SetInputWeights() 
        {
            var firstLayer = 0;
            var inputWeightMatrix = new double[_layerStructure.numberOfInputNodes, _layerStructure.HiddenLayerList[firstLayer]];
            var transposedInputWeightMatrix = Matrix.Transpose(inputWeightMatrix);

            var lastRowIndex = transposedInputWeightMatrix.GetUpperBound(0);
            var lastColumnIndex = transposedInputWeightMatrix.GetUpperBound(1);

            for (int i = 0; i <= lastRowIndex; i++)
            {
                for (int j = 0; j <= lastColumnIndex; j++)
                {
                    transposedInputWeightMatrix[i, j] = _randomDistribution.CalculateRandomValue();
                }

            }
            _weights.Add(transposedInputWeightMatrix);

        }

        private void SetHiddenWeights()
        {
            var numberOfHiddenLayers = _layerStructure.HiddenLayerList.Count;

            var penultimateHiddenLayer = numberOfHiddenLayers - 1;

            for (int i = 0; i < penultimateHiddenLayer; i++)
            {
                var neuronsInCurrentLayer = _layerStructure.HiddenLayerList[i];
                var neuronsInNextLayer = _layerStructure.HiddenLayerList[i + 1];

                double[,] weightMatrix = new double[neuronsInCurrentLayer, neuronsInNextLayer];

                var transposedWeightMatrix = Matrix.Transpose(weightMatrix);

                var lastRowIndex = transposedWeightMatrix.GetUpperBound(0);
                var lastColumnIndex = transposedWeightMatrix.GetUpperBound(1);

                for (int j = 0; j <= lastRowIndex; j++)
                {
                    for (int k = 0; k <= lastColumnIndex; k++)
                    {
                        transposedWeightMatrix[j, k] = _randomDistribution.CalculateRandomValue();             
                    }
                }
                _weights.Add(transposedWeightMatrix);
               
            }

            for (int i = numberOfHiddenLayers - 1; i < numberOfHiddenLayers; i++)
            {
                double[,] weightMatrix = new double[_layerStructure.HiddenLayerList[i], _layerStructure.numberOfOutputNodes];

                var transposedWeightMatrix = Matrix.Transpose(weightMatrix);

                var lastRowIndex = transposedWeightMatrix.GetUpperBound(0);
                var lastColumnIndex = transposedWeightMatrix.GetUpperBound(1);

                for (int j = 0; j <= lastRowIndex; j++)
                {
                    for (int k = 0; k <= lastColumnIndex; k++)
                    {
                        transposedWeightMatrix[j, k] = _randomDistribution.CalculateRandomValue();
                    }
                }
                _weights.Add(transposedWeightMatrix);
            }

        }

        private void SetBiases()  
        {
            _biases = new List<double[]>();

            SetHiddenBiases();

            SetOutputBiases();
        }

        private void SetHiddenBiases() 
        {

            var numberofHiddenLayers = _layerStructure.HiddenLayerList.Count;

            for (int i = 0; i < numberofHiddenLayers; i++)
            {
                var neuronsInCurrentLayer = _layerStructure.HiddenLayerList[i];

                var biasMatrix = new double[neuronsInCurrentLayer];

                for (int j = 0; j < neuronsInCurrentLayer; j++)
                {
                    biasMatrix[j] = _randomDistribution.CalculateRandomValue();
                }
                _biases.Add(biasMatrix);
            }
        }

        private void SetOutputBiases() 
        {
            var numberOfOutputNeurons = _layerStructure.numberOfOutputNodes;

            var outputBiasMatrix = new double[numberOfOutputNeurons];

            for (int i = 0; i < numberOfOutputNeurons; i++)
            {
                outputBiasMatrix[i] = _randomDistribution.CalculateRandomValue();
            }
            _biases.Add(outputBiasMatrix);
        }


        private void InitPreviousDeltaWeights()
        {
            _previousDeltaWeights = new List<double[,]>();

            int numberOfWeightMatrices = _weights.Count;

            for (int i = 0; i < numberOfWeightMatrices; i++)
            {
                int numberOfRows = _weights[i].GetLength(0);
                int numberOfColumns = _weights[i].GetLength(1);

                var previousWeightMatrix = new double[numberOfRows, numberOfColumns];

                _previousDeltaWeights.Add(previousWeightMatrix);
            }
        }

        private void InitPreviousDeltaBiases()
        {
            _previousDeltaBiases = new List<double[]>();

            int numberOfBiasMatrices = _biases.Count;

            for (int i = 0; i < numberOfBiasMatrices; i++)
            {
                int numberOfRows = _biases[i].GetLength(0);

                var previousBiasMatrix = new double[numberOfRows];

                _previousDeltaBiases.Add(previousBiasMatrix);
            }
        }

        public void ShowMatrix(List<double[,]> matrices)
        {
            var rows = 0;
            var columns = 1;

            foreach (double[,] matrix in matrices)
            {
                for (int j = 0; j <= matrix.GetUpperBound(rows); j++)
                {
                    for (int k = 0; k <= matrix.GetUpperBound(columns); k++)
                    {
                        Console.Write(matrix[j, k].ToString() + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }               
        }

        public FeedForwardData FeedForward(double[] inputMatrix)
        {
            //GenerateLayerInputs(inputMatrix);
            var layerOutputs = new List<double[]>();
            var layerInputs = new List<double[]>();

            layerInputs.Add(inputMatrix);  // w*i+b sum from each layer + input layer(input value only)
            layerOutputs.Add(inputMatrix); //"activated" outputs from each layer + input layer(not activated)

            int numberOfActivationLayers = _layerStructure.HiddenLayerList.Count + 1; //number of hidden layers + outputlayer

            if (_weights[0].GetLength(1) == inputMatrix.GetLength(0))
            {
                for (int i = 0; i < numberOfActivationLayers; i++) //feed forward through all the layers
                {

                    var nextLayerInput = new double[_weights[i].GetLength(0)];
                    var nextLayerOutput = new double[_weights[i].GetLength(0)];

                    nextLayerInput = Matrix.Multiply(_weights[i], layerOutputs[i]);
                    nextLayerInput = Matrix.Add(nextLayerInput, _biases[i]);

                    layerInputs.Add(nextLayerInput);
                  
                    for (int j = 0; j < nextLayerInput.GetLength(0); j++)
                    {
                        if (i != numberOfActivationLayers - 1) //hidden layer
                        {
                            nextLayerOutput[j] = _strategy.HiddenLayerActivate(nextLayerInput[j]);
                        }
                        else //output layer
                        {
                            nextLayerOutput[j] = _strategy.OutputLayerActivate(nextLayerInput[j]);
                        }
                    }
                    layerOutputs.Add(nextLayerOutput);
                }

            }
            else
            {
                throw new Exception("just...no");
            }

            _feedForwardData.LayerInputs = layerInputs;
            _feedForwardData.LayerOutputs = layerOutputs;

            return _feedForwardData;          
        }


        public void SplitDataSetIntoTrainAndTestSets(double sizeOfTrainingSet)
        {
            DataSet.SplitIntoTrainAndTest(sizeOfTrainingSet);
        }

        public void TrainNetwork()
        {
            _listOfErrors = new List<double>();

            int count = 0;            
            int numberOfDataRowsInTrainingSet = DataSet.TrainingSet.NumberOfDataRows;
            double accumulatedError = 1;

            for (int i = 0; i < _trainingParameters.epochs && accumulatedError > 0.000000001 ; i++)
            {
                double error = 0.0;
                DataSet.ShuffleDataSet();                

                for (int j = 0; j < numberOfDataRowsInTrainingSet; j++)
                {
                    var inputMatrix = DataSet.TrainingSet.InputMatrices[j];
                    var targetOutputMatrix = DataSet.TrainingSet.OutputMatrices[j];

                    var feedForwardData = FeedForward(inputMatrix);

                    //error += _strategy.Backpropagate(feedForwardData, targetOutputMatrix, _trainingParameters);
                    int timeStep = i;

                    error += _strategy.Backpropagate(feedForwardData, targetOutputMatrix, _trainingParameters, timeStep);

                    accumulatedError = error;                   
                    
                }

                _listOfErrors.Add(error);

                Console.WriteLine("error: " + error);

                Console.WriteLine();
                Console.WriteLine(count);

                count++;
            }
        }

        public void SaveListOfErrors()
        {

            double[] errorsArray = _listOfErrors.ToArray();

            string[] errorData = new string[errorsArray.Length];

            for (int i = 0; i < errorsArray.Length; i++)
            {
                errorData[i] = i.ToString() + "  " + errorsArray[i].ToString();
            }

            File.WriteAllLines(_savePath.ToString() + "/Errors.txt", errorData);

            _listOfErrors.Clear();

        }

        /// <summary>
        /// Extracts the single output regression network's approximated function and writes it to a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetApproximatedFunction(string filePath)
        {
            string approximatedFunction = "";

            int numberOfHiddenLayers = _layerStructure.HiddenLayerList.Count;
            List<string[]> previousLayerOutputs = new List<string[]>();


            //1. Get number of inputs and make a variable for each of them.            
            int numberOfInputs = _layerStructure.numberOfInputNodes;
            var functionInputs = new string[numberOfInputs];

            for (int i = 0; i < numberOfInputs; i++)
            {
                functionInputs[i] = "x" + i.ToString();
            }

            previousLayerOutputs.Add(functionInputs);

            //2. Create the hidden layer parts of the function
            StringBuilder weightInputHiddenSum = new StringBuilder();

            for (int i = 0; i < numberOfHiddenLayers; i++)
            {
                var previousWeights = _weights[i];
                var nextWeights = _weights[i + 1];

                int numberOfNeuronsInCurrentLayer = _layerStructure.HiddenLayerList[i];
                int numberOfNeuronsInPreviousLayer = previousWeights.GetLength(1);
                int numberOFNeuronsInNextLayer = nextWeights.GetLength(1);
                var currentHiddenLayerBias = _biases[i];

                var currentLayerInputs = previousLayerOutputs[i];

                var currentLayerOutputs = new string[numberOfNeuronsInCurrentLayer];

                for (int j = 0; j < numberOfNeuronsInCurrentLayer; j++)
                {
                    double bias = currentHiddenLayerBias[j];
                    weightInputHiddenSum.Clear();

                    for (int k = 0; k < numberOfNeuronsInPreviousLayer; k++)
                    {

                        double previousWeight = previousWeights[j, k];
                        string input = currentLayerInputs[k];

                        weightInputHiddenSum.Append(string.Format("({0} * {1})+", previousWeight, input));

                    }
                    weightInputHiddenSum = weightInputHiddenSum.Remove(weightInputHiddenSum.Length - 1, 1);

                    var currentNodeOutput = _strategy.GetHiddenActivationFunctionAsString(weightInputHiddenSum.ToString(), bias);
                    currentLayerOutputs[j] = currentNodeOutput;

                }
                previousLayerOutputs.Add(currentLayerOutputs);

            }

            //3. Create the final approximated function with the output layer activation

            int numberOfOutputs = _layerStructure.numberOfOutputNodes;

            int numberOfNeuronsInLastHiddenLayer = _layerStructure.HiddenLayerList[numberOfHiddenLayers - 1];

            var lastHiddenToOutputWeights = _weights[_weights.Count - 1];
            var outputsFromLastHiddenLayer = previousLayerOutputs[previousLayerOutputs.Count - 1];
            var outputLayerBiases = _biases[_biases.Count - 1];

            string weightinputSum = "";

            for (int i = 0; i < numberOfOutputs; i++)
            {
                var bias = outputLayerBiases[i];
                for (int j = 0; j < numberOfNeuronsInLastHiddenLayer; j++)
                {
                    double previousWeight = lastHiddenToOutputWeights[i, j];
                    string input = outputsFromLastHiddenLayer[j];

                    weightinputSum += string.Format("({0} * {1})+", previousWeight, input);

                }
                weightinputSum = weightinputSum.TrimEnd('+');
                approximatedFunction = _strategy.GetOutputActivationFunctionAsString(weightinputSum, bias);

            }

            try
            {
                File.WriteAllText(filePath, approximatedFunction);

            }
            catch (IOException exception)
            {

                Console.WriteLine("Error writing to {0}. Error Message: {1}", filePath, exception.Message);
            }

            return approximatedFunction;
        }
        
    }
}
