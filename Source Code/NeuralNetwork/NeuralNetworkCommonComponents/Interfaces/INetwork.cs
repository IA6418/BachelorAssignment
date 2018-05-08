using NeuralNetwork.CommonComponents.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.CommonComponents.Interfaces
{
    public interface INetwork
    {
        FeedForwardData FeedForward(double[] inputMatrix);

        IDataSet DataSet { get; set; }

        IInitialRandomDistributionType RandomDistribution { get; set; }

        IOptimizationStrategy Strategy { get; set; }

        LayerStructure LayerStructure { get; set; }

        TrainingParameters TrainingParameters { get; set; }

        List<double[,]> Weights { get; set; }

        List<double[]> Biases { get; set; }

        string SavePath { get; set; }
    }
}
