using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSet.DataSets
{
    public interface ISubDataSet
    {
        double[,] DataSet { get; set; }

        List<double[]> InputMatrices { get; set; }

        List<double[]> OutputMatrices { get; set; }

        int NumberOfDataRows { get;}

        void CreateInputAndOutputMatrices();
    }
}
