using NeuralNetwork.CommonComponents.Structs;
using NeuralNetwork.Core.InitialRandomDistributionTypes;
using System.Collections.Generic;
using System;
using NeuralNetwork.Core.OptimizationStrategies;
using NeuralNetwork.Core.ActivationFunctions;
using NeuralNetwork.Core.CostFunctions;
using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.Network;
using NeuralNetwork.Core.NetworkEvaluators;
using NeuralNetwork.Core;
using NeuralNetwork.Core.Factories;
using NeuralNetwork.XMLService;

namespace NeuralNetwork.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set the path of the file containing the data set
            //string dataFilePath = @"C:\Users\kevin\Desktop\squaredtest.csv"; NutrioxDataset
            string dataFilePath = @"C:\Users\Bruker\Desktop\NutrioxDataset.csv";
            //string dataFilePath = @"C:\Users\Bruker\Desktop\-5to5-200Rows.csv";

            //Create a new data set
            DataSet.DataSet dataSet = new DataSet.DataSet(dataFilePath, true);

            //Apply desired data preprocessing to the data set
            dataSet.PreProcessDataSet(NormalizationType.MinMax, 2, EncodingType.None, null);

            //Create a model hyperparameter layer structure
            LayerStructure layerStructure = new LayerStructure() { numberOfInputNodes = 2, HiddenLayerList = new List<int> {5,5}, numberOfOutputNodes = 1};

            //Create an instance of the desired optimalization strategy to use

            var regularizationStrategyFactory = new RegularizationStrategyFactory();
            StochasticGradientDescent SGD = new StochasticGradientDescent(new SigmoidFunction(), new IdentityFunction(), new MeanSquaredError(), RegularizationType.None, regularizationStrategyFactory);

            //Create training hyperparameters
            TrainingParameters trainingParams = new TrainingParameters() { epochs = 500, learningRate = 0.01, momentum = 0.01, RegularizationLambda = 0.00};

            //Create an instance of a neural network
            //ArtificialNeuralNetwork ann = new ArtificialNeuralNetwork(layerStructure, trainingParams, dataSet, SGD, new GaussianDistribution());

            //Or Load a Network from XML

            XML xml = new XML();

            ArtificialNeuralNetwork ann = xml.LoadNetwork(@"C:\Users\Bruker\Desktop\BestNet.xml", dataSet) as ArtificialNeuralNetwork;

            //Apply the desired training/test data set split ratios.
            ann.SplitDataSetIntoTrainAndTestSets(0.7);

            //Initiate network training
            //ann.TrainNetwork();

            var crossValidationStrategyFactory = new CrossValidationStrategyFactory();
            NetworkEvaluator evaluator = new NetworkEvaluator(ann);
            CrossValidator crossValidator = new CrossValidator(ann, evaluator, crossValidationStrategyFactory);

            //Cross-validate the fitted model
            //crossValidator.KFold(10, 0.007);

            //Evaluate the fitted model on the test set
            evaluator.EvaluateNetwork(0.007);


            //--Optional--//

            //Serialize and save the fitted model

            //XML xml = new XML();
            //xml.SaveNetwork(dataFilePath, ann);

            //Extract model information

            //ann.SaveListOfErrors();

            //ann.GetApproximatedFunction(ann.SavePath + "/Function.txt");

            Console.ReadLine();

        }
    }
}
