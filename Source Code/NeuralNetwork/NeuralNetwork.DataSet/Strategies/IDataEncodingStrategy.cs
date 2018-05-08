using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.NeuralNetwork.DataSet.Strategies
{
    public interface IDataEncodingStrategy
    {
        string[,] Encode(string[,] dataSet, int[] columnIndecesToEncode);
    }
}
