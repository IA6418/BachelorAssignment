using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.CommonComponents.Interfaces
{
    public interface ICrossValidationStrategy
    {
        double CrossValidate(double[,] dataSet, int KNumberOfFolds, double maximumAllowedDeviation);

        double CrossValidate(double[,] dataSet, int KNumberOfFolds);
    }
}
