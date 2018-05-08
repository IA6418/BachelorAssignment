using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NeuralNetwork.CommonComponents.Interfaces;
using NeuralNetwork.CommonComponents.Structs;
using LinearAlgebra;
using NeuralNetwork.Network;

namespace NeuralNetwork.XMLService
{
    public class XML
    {

        private XmlDocument _xmlDoc;// = new XmlDocument();

        static INetwork _network;

        static string _filepath;


        public void SaveNetwork(string dataSetFileLocation, INetwork network)
        {

            XmlWriter writer = XmlWriter.Create(_network.SavePath.ToString() + "/NeuralNetwork.xml");

            writer.WriteStartElement("NeuralNetwork"); //doc start

            writer.WriteAttributeString("Date", DateTime.Now.ToString());

            writer.WriteAttributeString("Type", "BackPropagation");

            writer.WriteAttributeString("DataFileUsedForTraining", dataSetFileLocation);


            //Network settings------------8::::::::::::>---------------------------


            writer.WriteStartElement("Settings");

            writer.WriteStartElement("DistributionType");

            writer.WriteElementString("InitialRandomDistributionType", _network.RandomDistribution.ToString());

            writer.WriteEndElement();


            //TrainingParams------------8::::::::::::>---------------------------


            writer.WriteStartElement("TrainingParameters");

            writer.WriteElementString("Epochs", network.TrainingParameters.epochs.ToString());

            writer.WriteElementString("LearningRate", network.TrainingParameters.learningRate.ToString());

            writer.WriteElementString("Momentum", network.TrainingParameters.momentum.ToString());

            writer.WriteEndElement();


            //Layer settings------------8::::::::::::>---------------------------


            writer.WriteStartElement("LayerStructure");

            writer.WriteElementString("NumberOfInputNeurons", _network.LayerStructure.numberOfInputNodes.ToString());

            writer.WriteElementString("NumberOfOutputNeurons", _network.LayerStructure.numberOfOutputNodes.ToString());

            writer.WriteElementString("NumberOfHiddenLayers", _network.LayerStructure.HiddenLayerList.Count.ToString());

            for (int i = 0; i < network.LayerStructure.HiddenLayerList.Count; i++)
            {
                writer.WriteElementString("NumberOfNeuronsInHiddenLayer" + (i + 1).ToString(), network.LayerStructure.HiddenLayerList[i].ToString());
            }

            writer.WriteEndElement();


            //Strategy settings------------8::::::::::::>---------------------------


            writer.WriteStartElement("Strategy");

            writer.WriteElementString("OptimizationStrategy", _network.Strategy.ToString());

            writer.WriteElementString("HiddenLayerActivationFunction", _network.Strategy.HiddenActivationFunction.ToString());

            writer.WriteElementString("OutputLayerActivationFunction", _network.Strategy.OutputActivationFunction.ToString());

            writer.WriteElementString("CostFunction", _network.Strategy.CostFunction.ToString());

            writer.WriteEndElement();


            //Weights settings------------8::::::::::::>---------------------------


            writer.WriteStartElement("Weights");

            for (int i = 0; i < network.Weights.Count; i++)
            {

                writer.WriteStartElement("WeightsForLayer" + (i + 1).ToString());

                writer.WriteAttributeString("LayerNumber", (i + 1).ToString());

                writer.WriteAttributeString("NumberOfRows", network.Weights[i].GetLength(0).ToString());

                writer.WriteAttributeString("NumberOfColumns", network.Weights[i].GetLength(1).ToString());

                int counter = 0; int rowCounter = 1; int columnCounter = 1;

                foreach (double weight in network.Weights[i])
                {
                    writer.WriteStartElement("Neuron");

                    writer.WriteAttributeString("Weight" + (counter + 1).ToString(), weight.ToString());

                    writer.WriteAttributeString("Row", rowCounter.ToString());

                    writer.WriteAttributeString("Column", columnCounter.ToString());

                    writer.WriteEndElement();

                    counter++;

                    if (columnCounter < network.Weights[i].GetLength(1))
                    {
                        columnCounter++;
                    }
                    else
                    {
                        rowCounter++;

                        columnCounter = 1;
                    }
                }

                writer.WriteEndElement();
            }
            writer.WriteEndElement();


            //Bias settings------------8::::::::::::>---------------------------


            writer.WriteStartElement("Biases"); //Biases start

            for (int i = 0; i < network.Biases.Count; i++)
            {

                writer.WriteStartElement("BiasesForLayer" + (i + 1).ToString());

                writer.WriteAttributeString("LayerNumber", (i + 1).ToString());

                writer.WriteAttributeString("NumberOfRows", network.Biases[i].GetLength(0).ToString());

                int counter = 0;

                foreach (double bias in network.Biases[i])
                {
                    writer.WriteStartElement("Neuron");

                    writer.WriteAttributeString("Bias" + (counter + 1).ToString(), bias.ToString());

                    writer.WriteAttributeString("Row", (counter + 1).ToString());

                    writer.WriteEndElement();

                    counter++;
                }

                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement(); //settings end

            writer.WriteEndElement(); // doc end

            writer.Flush();

            writer.Close();

        }

        public INetwork LoadNetwork(string xmlFilePath, IDataSet dataset)
        {

            //XmlDocument _xmlDoc = new XmlDocument();
             _xmlDoc = new XmlDocument();

            _filepath = xmlFilePath;

            _xmlDoc.Load(_filepath);


            //Create Training Params---------------------8::::::::::::::>----------------------------------------


            TrainingParameters trainingParams = new TrainingParameters()
            {
                epochs = Convert.ToInt32(GetXmlValue("NeuralNetwork/Settings/TrainingParameters/Epochs")),

                learningRate = Convert.ToDouble(GetXmlValue("NeuralNetwork/Settings/TrainingParameters/LearningRate")),

                momentum = Convert.ToDouble(GetXmlValue("NeuralNetwork/Settings/TrainingParameters/Momentum"))
            };


            //Create LayerStructure---------------------8::::::::::::::>----------------------------------------


            List<int> hiddenLayersList = new List<int>();

            int numberOfHiddenLayers = Convert.ToInt32(GetXmlValue("NeuralNetwork/Settings/LayerStructure/NumberOfHiddenLayers"));

            for (int i = 1; i <= numberOfHiddenLayers; i++)
            {
                int numberOfNeuronsInHiddenLayer = Convert.ToInt32(GetXmlValue("NeuralNetwork/Settings/LayerStructure/NumberOfNeuronsInHiddenLayer" + i.ToString()));

                hiddenLayersList.Add(numberOfNeuronsInHiddenLayer);
            }

            LayerStructure layerStructure = new LayerStructure()
            {
                numberOfInputNodes = Convert.ToInt32(GetXmlValue("NeuralNetwork/Settings/LayerStructure/NumberOfInputNeurons")),

                numberOfOutputNodes = Convert.ToInt32(GetXmlValue("NeuralNetwork/Settings/LayerStructure/NumberOfOutputNeurons")),

                HiddenLayerList = hiddenLayersList
            };


            //Create Distribution Type---------------------8::::::::::::::>----------------------------------------


            string distribution = GetXmlValue("NeuralNetwork/Settings/DistributionType/InitialRandomDistributionType");

            var distributionType = Type.GetType(distribution + ",NeuralNetwork.Core");

            IInitialRandomDistributionType initialDistribution = Activator.CreateInstance(distributionType) as IInitialRandomDistributionType;


            //Create Strategy---------------------8::::::::::::::>----------------------------------------


            string hiddenFunction = GetXmlValue("NeuralNetwork/Settings/Strategy/HiddenLayerActivationFunction");

            string outputFunction = GetXmlValue("NeuralNetwork/Settings/Strategy/OutputLayerActivationFunction");

            string costFunc = GetXmlValue("NeuralNetwork/Settings/Strategy/CostFunction");

            var hiddenFunctionType = Type.GetType(hiddenFunction + ",NeuralNetwork.Core");

            var outputFunctionType = Type.GetType(outputFunction + ",NeuralNetwork.Core");

            var costFunctionType = Type.GetType(costFunc + ",NeuralNetwork.Core");

            IActivationFunction hiddenActivationFunction = Activator.CreateInstance(hiddenFunctionType) as IActivationFunction;

            IActivationFunction outputActivationFunction = Activator.CreateInstance(outputFunctionType) as IActivationFunction;

            ICostFunction costFunction = Activator.CreateInstance(costFunctionType) as ICostFunction;


            string optimizationStrategy = GetXmlValue("NeuralNetwork/Settings/Strategy/OptimizationStrategy");

            var optStrategy = Type.GetType(optimizationStrategy + ",NeuralNetwork.Core");

            IOptimizationStrategy strategy = Activator.CreateInstance(optStrategy, hiddenActivationFunction, outputActivationFunction, costFunction, layerStructure) as IOptimizationStrategy;


            //Create Network---------------------8::::::::::::::>----------------------------------------


            _network = new ArtificialNeuralNetwork(layerStructure, trainingParams, dataset, strategy, initialDistribution);


            //Set Weights---------------------8::::::::::::::>----------------------------------------


            _network.Weights.Clear();

            LoadInputWeights();

            LoadHiddenWeights();


            //Set Biases---------------------8::::::::::::::>----------------------------------------


            _network.Biases.Clear();

            LoadHiddenBiases();

            LoadOutputBiases();


            //Clear XMLDoc---------------------8::::::::::::::>----------------------------------------


            _xmlDoc = null;

            return _network;

        }
        private string GetXmlValue(string xmlPath)
        {

            //_xmlDoc.Load(_filepath);

            XmlNode node = _xmlDoc.SelectSingleNode(xmlPath);

            return node.InnerText;

        }

        private void LoadInputWeights()
        {
            var firstLayer = 0;

            var inputWeightMatrix = new double[_network.LayerStructure.numberOfInputNodes, _network.LayerStructure.HiddenLayerList[firstLayer]];

            var transposedInputWeightMatrix = Matrix.Transpose(inputWeightMatrix);

            var lastRowIndex = transposedInputWeightMatrix.GetUpperBound(0);

            var lastColumnIndex = transposedInputWeightMatrix.GetUpperBound(1);

            int counter = 1;

            for (int i = 0; i <= lastRowIndex; i++)
            {
                for (int j = 0; j <= lastColumnIndex; j++)
                {
                    double weight = Convert.ToDouble(GetXmlValue("NeuralNetwork/Settings/Weights/WeightsForLayer1/Neuron/@Weight" + counter.ToString()));

                    transposedInputWeightMatrix[i, j] = weight;

                    counter++;
                }
            }
            _network.Weights.Add(transposedInputWeightMatrix);
        }

        private void LoadHiddenWeights()
        {
            var numberOfHiddenLayers = _network.LayerStructure.HiddenLayerList.Count;

            var penultimateHiddenLayer = numberOfHiddenLayers - 1;

            for (int i = 0; i < penultimateHiddenLayer; i++)
            {
                var neuronsInCurrentLayer = _network.LayerStructure.HiddenLayerList[i];

                var neuronsInNextLayer = _network.LayerStructure.HiddenLayerList[i + 1];

                double[,] weightMatrix = new double[neuronsInCurrentLayer, neuronsInNextLayer];

                var transposedWeightMatrix = Matrix.Transpose(weightMatrix);

                var lastRowIndex = transposedWeightMatrix.GetUpperBound(0);

                var lastColumnIndex = transposedWeightMatrix.GetUpperBound(1);

                int counter = 1;

                for (int j = 0; j <= lastRowIndex; j++)
                {
                    for (int k = 0; k <= lastColumnIndex; k++)
                    {
                        double weight = Convert.ToDouble(GetXmlValue("NeuralNetwork/Settings/Weights/WeightsForLayer" + (i + 2).ToString() + "/Neuron/@Weight" + counter.ToString()));

                        transposedWeightMatrix[j, k] = weight;

                        counter++;
                    }
                }
                _network.Weights.Add(transposedWeightMatrix);

            }

            for (int i = numberOfHiddenLayers - 1; i < numberOfHiddenLayers; i++)
            {
                double[,] weightMatrix = new double[_network.LayerStructure.HiddenLayerList[i], _network.LayerStructure.numberOfOutputNodes];

                var transposedWeightMatrix = Matrix.Transpose(weightMatrix);

                var lastRowIndex = transposedWeightMatrix.GetUpperBound(0);

                var lastColumnIndex = transposedWeightMatrix.GetUpperBound(1);

                int counter = 1;

                for (int j = 0; j <= lastRowIndex; j++)
                {
                    for (int k = 0; k <= lastColumnIndex; k++)
                    {
                        double weight = Convert.ToDouble(GetXmlValue("NeuralNetwork/Settings/Weights/WeightsForLayer" + (i + 2).ToString() + "/Neuron/@Weight" + counter.ToString()));

                        transposedWeightMatrix[j, k] = weight;

                        counter++;
                    }
                }
                _network.Weights.Add(transposedWeightMatrix);
            }

        }

        private void LoadHiddenBiases()
        {
            var numberofHiddenLayers = _network.LayerStructure.HiddenLayerList.Count;

            for (int i = 0; i < numberofHiddenLayers; i++)
            {
                var neuronsInCurrentLayer = _network.LayerStructure.HiddenLayerList[i];

                var biasMatrix = new double[neuronsInCurrentLayer];

                int counter = 1;

                for (int j = 0; j < neuronsInCurrentLayer; j++)
                {
                    double bias = Convert.ToDouble(GetXmlValue("NeuralNetwork/Settings/Biases/BiasesForLayer" + (i + 1).ToString() + "/Neuron/@Bias" + counter.ToString()));

                    biasMatrix[j] = bias;

                    counter++;
                }
                _network.Biases.Add(biasMatrix);
            }
        }

        private void LoadOutputBiases()
        {
            int numberOfLayers = _network.LayerStructure.HiddenLayerList.Count + 1;

            int numberOfOutputNeurons = _network.LayerStructure.numberOfOutputNodes;

            var outputBiasMatrix = new double[numberOfOutputNeurons];

            int counter = 1;

            for (int i = 0; i < numberOfOutputNeurons; i++)
            {
                double bias = Convert.ToDouble(GetXmlValue("NeuralNetwork/Settings/Biases/BiasesForLayer" + numberOfLayers + "/Neuron/@Bias" + counter.ToString()));

                outputBiasMatrix[i] = bias;

                counter++;
            }
            _network.Biases.Add(outputBiasMatrix);
        }

    }
}
