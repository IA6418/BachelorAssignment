using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using NeuralNetwork;

namespace NetworkSerializer
{
    static public class XMLNetworkSerializer
    {
        static public void SaveNetwork(string dataFileUsed, ArtificialNeuralNetwork network) //public void SaveNetwork(ArtificialNeuralNetwork network)
        {

            XmlWriter writer = XmlWriter.Create(network.FilePath.ToString() + "/NeuralNetwork.xml");

            writer.WriteStartElement("NeuralNetwork"); //doc start

            writer.WriteAttributeString("Date", DateTime.Now.ToString());

            writer.WriteAttributeString("Type", "BackPropagation");

            writer.WriteAttributeString("DataFileUsedForTraining", dataFileUsed);


            //Network settings------------8::::::::::::>---------------------------


            writer.WriteStartElement("Settings");

            writer.WriteStartElement("DistributionType");

            writer.WriteElementString("InitialRandomDistributionType", network.RandomDistribution.ToString());

            writer.WriteEndElement();


            //Layer settings------------8::::::::::::>---------------------------


            writer.WriteStartElement("LayerStrucure");

            writer.WriteElementString("NumberOfInputNeurons", network.LayerStructure.numberOfInputNodes.ToString());
            writer.WriteElementString("NumberOfOutputNeurons", network.LayerStructure.numberOfOutputNodes.ToString());
            writer.WriteElementString("NumberOfHiddenLayers", network.LayerStructure.HiddenLayerList.Count.ToString());

            for (int i = 0; i < network.LayerStructure.HiddenLayerList.Count; i++)
            {
                writer.WriteElementString("NumberOfNeuronsInHiddenLayer" + (i + 1).ToString(), network.LayerStructure.HiddenLayerList[i].ToString());
            }

            writer.WriteEndElement();


            //Strategy settings------------8::::::::::::>---------------------------


            writer.WriteStartElement("Strategy");

            writer.WriteElementString("OptimizationStrategy", network.Strategy.ToString());
            writer.WriteElementString("HiddenLayerActivationFunction", network.Strategy.HiddenActivationFunction.ToString());
            writer.WriteElementString("OutputLayerActivationFunction", network.Strategy.OutputActivationFunction.ToString());
            writer.WriteElementString("CostFunction", network.Strategy.CostFunction.ToString());

            writer.WriteEndElement();


            //Weights settings------------8::::::::::::>---------------------------


            writer.WriteStartElement("Weights");

            for (int i = 0; i < network.Weights.Count; i++)
            {

                writer.WriteStartElement("WeightsForLayer" + (i + 1).ToString());
                writer.WriteAttributeString("NumberOfRows", network.Weights[i].GetLength(0).ToString());
                writer.WriteAttributeString("NumberOfColumns", network.Weights[i].GetLength(1).ToString());

                int counter = 0;
                foreach (double weight in network.Weights[i])
                {
                    writer.WriteElementString("Weight" + (counter + 1).ToString(), weight.ToString());
                    counter++;
                }

                writer.WriteEndElement();
            }
            writer.WriteEndElement();


            //Bias settings------------8::::::::::::>---------------------------


            writer.WriteStartElement("Biases"); //Biases start

            for (int i = 0; i < network.Biases.Count; i++)
            {

                writer.WriteStartElement("BiasesForLayer" + (i + 1).ToString());
                writer.WriteAttributeString("NumberOfRows", network.Biases[i].GetLength(0).ToString());

                int counter = 0;
                foreach (double bias in network.Biases[i])
                {
                    writer.WriteElementString("Weight" + (counter + 1).ToString(), bias.ToString());
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
    }
}
