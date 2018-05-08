using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.CommonComponents.Structs
{
    public struct TrainingParameters
    {
        public int epochs;
        public double learningRate;
        public double momentum;
        public double RegularizationLambda;
    }
}
