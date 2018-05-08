using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.CommonComponents.Interfaces;
using NeuralNetwork.Core.CrossValidationStrategies;
using NeuralNetwork.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Core
{
    public class CrossValidator
    {
        private ICrossValidationStrategy _crossValidationStrategy;
        private ICrossValidationStrategyFactory _crossValidationStrategyFactory;
        private INetwork _network;
        private INetworkEvaluator _evaluator;

        public CrossValidator(INetwork network, INetworkEvaluator evaluator, ICrossValidationStrategyFactory crossValidationStrategyFactory)
        {
            _network = network;
            _evaluator = evaluator;
            _crossValidationStrategyFactory = crossValidationStrategyFactory;
        }
        /// <summary>
        /// K-Fold Cross-Validation for classification networks
        /// </summary>
        /// <param name="KNumberOfFolds"></param>
        /// <returns></returns>
        public double KFold(int KNumberOfFolds)
        {
            _crossValidationStrategy = _crossValidationStrategyFactory.CreateCrossValidationStrategy(CrossValidationType.KFold, _network, _evaluator);

            double validationScore = _crossValidationStrategy.CrossValidate(_network.DataSet.TrainingSet.DataSet, KNumberOfFolds);

            Console.WriteLine(string.Format("{0}-Fold Cross-Validation completed with a score of {1}%", KNumberOfFolds, (validationScore*100)));

            return validationScore;
        }

        /// <summary>
        /// K-Fold Cross-Validation for regression networks
        /// </summary>
        /// <param name="KNumberOfFolds"></param>
        /// <param name="maximumAllowedDeviation"></param>
        /// <returns></returns>
        public double KFold(int KNumberOfFolds, double maximumAllowedDeviation)
        {
            _crossValidationStrategy = _crossValidationStrategyFactory.CreateCrossValidationStrategy(CrossValidationType.KFold, _network, _evaluator);

            double validationScore = _crossValidationStrategy.CrossValidate(_network.DataSet.TrainingSet.DataSet, KNumberOfFolds, maximumAllowedDeviation);

            Console.WriteLine(string.Format("{0}-Fold Cross-Validation completed with a score of {1}%", KNumberOfFolds, (validationScore*100)));

            return validationScore;
        }
    }
}
