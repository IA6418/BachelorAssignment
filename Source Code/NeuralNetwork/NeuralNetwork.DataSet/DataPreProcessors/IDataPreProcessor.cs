
using NeuralNetwork.CommonComponents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.DataPreProcessors
{
    public interface IDataPreProcessor
    {
        double[,] PreProcessData(string[,] rawDataSet, NormalizationType normalizationType, int numberOfInputs, EncodingType encodingType, int[] columnIndexesToEncode);
    }
}
