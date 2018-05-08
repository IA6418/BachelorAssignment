using DataSet.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.CommonComponents.Interfaces
{
    public interface IDataSet
    {
        ISubDataSet TrainingSet { get; }
        ISubDataSet TestSet { get; }

        int NumberOfInputs { get; set; }
        int NumberOfOutputs { get; set; }

        void SplitIntoTrainAndTest(double sizeOfTrainingSet);

        void GenerateTrainingAndTestSets();

        double[,] Shuffle(double[,] dataSet);

        void ShuffleDataSet();

    }
    
    
}
