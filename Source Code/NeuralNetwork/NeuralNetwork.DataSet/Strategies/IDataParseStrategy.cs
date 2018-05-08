using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.Strategies
{
    public interface IDataParseStrategy
    {
        string[,] ParseData(string dataFilePath, bool firstRowHeader);
    }
}
