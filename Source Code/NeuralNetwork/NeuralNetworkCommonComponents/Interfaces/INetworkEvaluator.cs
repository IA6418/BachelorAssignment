using DataSet.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.CommonComponents.Interfaces
{
   
    public interface INetworkEvaluator
    {
        double EvaluateRegressionNetwork(double maximumAllowedError, ISubDataSet subDataSet);

        double EvaluateClassificationNetwork(ISubDataSet subDataSet);
    }
}
