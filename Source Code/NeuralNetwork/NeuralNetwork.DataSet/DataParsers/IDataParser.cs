using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.DataParsers
{
    public interface IDataParser
    {
        string[,] ParseData(string filePath, string fileExtension, bool firstRowColumn);
    }
}
