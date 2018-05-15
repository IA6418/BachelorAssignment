using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.CommonComponents.Interfaces;
using NeuralNetwork.Core.CrossValidationStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core.Factories
{
    public class CrossValidationStrategyFactory : ICrossValidationStrategyFactory
    {
        public ICrossValidationStrategy CreateCrossValidationStrategy(CrossValidationType crossValidationType, INetwork network, INetworkEvaluator evaluator)
        {
            switch (crossValidationType)
            {
                case CrossValidationType.KFold:
                    return new KFold(network, evaluator);
                case CrossValidationType.HoldOut:
                    return new HoldOut();
                default:
                    return new HoldOut();
            }
        }
    }
}
