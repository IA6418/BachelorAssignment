using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.DataSetImporter
{
    public interface IDataSetImporter
    {
        string[,] ImportDataFileIntoDataSet(string dataFilePath, bool firstRowHeader);

        string GetFileExtension(string dataFilePath);
    }
}
