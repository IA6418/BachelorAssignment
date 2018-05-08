using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.CommonComponents.Structs
{   //this should be a class
    public struct FeedForwardData
    {
        public List<double[]> LayerInputs;
        public List<double[]> LayerOutputs;

    }
}
