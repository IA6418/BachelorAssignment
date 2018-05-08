using NeuralNetwork.CommonComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.InitialRandomDistributionTypes
{
    public class GaussianDistribution : IInitialRandomDistributionType
    {
        private static Random random = new Random();
        public double CalculateRandomValue()
        {
            
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();

            double randomStandardNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            double randomNormal = 0 + 1 * randomStandardNormal;

            return randomNormal;
        }
    }
}
