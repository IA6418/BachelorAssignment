using DataSet.DataSets;
using NeuralNetwork.CommonComponents.Enums;
using NeuralNetwork.CommonComponents.Interfaces;
using NeuralNetwork.DataSet.DataPreProcessors;
using NeuralNetwork.DataSet.DataSetImporter;
using NeuralNetwork.DataSet.DataSets;
using System;
using System.Collections.Generic;

namespace NeuralNetwork.DataSet
{
    public class DataSet : IDataSet
    {
        private double[,] _dataSet; 

        private string[,] _rawDataSet;

        private double[,] _shuffledDataSet;
        public ISubDataSet TrainingSet { get; private set; }
        public ISubDataSet TestSet { get; private set; }

        private IDataSetImporter _dataImporter;

        private IDataPreProcessor _dataPreProcessor;

        public int NumberOfInputs { get; set; }
        public int NumberOfOutputs { get; set; }

        private static Random _random;

        public DataSet(string dataFilePath, bool isFirstRowHeader)
        {

            _dataImporter = new DataSetImporter.ConcreteImporters.DataSetImporter();
            _dataPreProcessor = new DataPreProcessors.ConcretePreProcessors.DataPreProcessor();

            _random = new Random();

            ImportDataFromFileIntoDataSet(dataFilePath, isFirstRowHeader);
            
        }

        private void ImportDataFromFileIntoDataSet(string dataFilePath, bool isFirstRowHeader)
        {
            _rawDataSet = _dataImporter.ImportDataFileIntoDataSet(dataFilePath, isFirstRowHeader);
 
        }

        public void PreProcessDataSet(NormalizationType normalizationType, int numberOfInputs, EncodingType encodingType, int[] columnIndicesToEncode)
        {
            _dataSet = _dataPreProcessor.PreProcessData(_rawDataSet, normalizationType, numberOfInputs, encodingType, columnIndicesToEncode);
        }

        public void GenerateTrainingAndTestSets()
        {
            TrainingSet = new TrainingDataSet(_dataSet, NumberOfInputs);
            TestSet = new TestDataSet(_dataSet, NumberOfInputs);
        }

        /// <summary>
        /// Splits the data set into a training set and a test set
        /// </summary>
        /// <param name="sizeOfTrainingSet">Sets the ratio of training data to test data. Needs to be a value between 0 and 1</param>
        public void SplitIntoTrainAndTest(double sizeOfTrainingSet)
        {

            int totalNumberOfDataRows = _dataSet.GetLength(0);
            int totalNumberOfDataColumns = _dataSet.GetLength(1);

            int numberOfRowsInTrainingSet = (int)(totalNumberOfDataRows * sizeOfTrainingSet);

            var trainingSet = new double[numberOfRowsInTrainingSet, totalNumberOfDataColumns];
            var testSet = new double[totalNumberOfDataRows - numberOfRowsInTrainingSet, totalNumberOfDataColumns];

            _shuffledDataSet = Shuffle(_dataSet); //shuffle data set before split?

            Array.Copy(_shuffledDataSet, 0, trainingSet, 0, trainingSet.Length);

            Array.Copy(_shuffledDataSet, trainingSet.Length, testSet, 0, testSet.Length);
            
            TrainingSet.DataSet = trainingSet;
            TestSet.DataSet = testSet;

            UpdateInputAndOutputMatrices();
        }

        public void ShuffleDataSet()
        {
            TrainingSet.DataSet = Shuffle(TrainingSet.DataSet);
            TestSet.DataSet = Shuffle(TestSet.DataSet);

            UpdateInputAndOutputMatrices();

        }

        private void UpdateInputAndOutputMatrices()
        {
            TrainingSet.CreateInputAndOutputMatrices();
            TestSet.CreateInputAndOutputMatrices();
        }

        public void Shuffle()
        {

            int numberOfRows = _dataSet.GetLength(0);
            int numberOfColumns = _dataSet.GetLength(1);

            var shuffledDataSet = new double[numberOfRows, numberOfColumns];


            var dataRows = new List<double[]>();


            for (int i = 0; i < numberOfRows; i++)
            {
                var row = new double[numberOfColumns];

                for (int j = 0; j < numberOfColumns; j++)
                {
                    row[j] = _dataSet[i, j];
                }

                dataRows.Add(row);
            }

            //Fisher-Yates
            int rowCount = dataRows.Count;

            while (rowCount > 1)
            {
                rowCount--;
                int randomIndex = _random.Next(rowCount + 1);
                var value = dataRows[randomIndex];
                dataRows[randomIndex] = dataRows[rowCount];
                dataRows[rowCount] = value;
            }


            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    shuffledDataSet[i, j] = dataRows[i][j];
                }
            }

            _dataSet = shuffledDataSet;
            
        }

        public double[,] Shuffle(double[,] dataSet)
        {

            int numberOfRows = dataSet.GetLength(0);
            int numberOfColumns = dataSet.GetLength(1);

            var shuffledDataSet = new double[numberOfRows, numberOfColumns];


            var dataRows = new List<double[]>();


            for (int i = 0; i < numberOfRows; i++)
            {
                var row = new double[numberOfColumns];

                for (int j = 0; j < numberOfColumns; j++)
                {
                    row[j] = dataSet[i, j];
                }

                dataRows.Add(row);
            }

            //Fisher-Yates
            int rowCount = dataRows.Count;

            while (rowCount > 1)
            {
                rowCount--;
                int randomIndex = _random.Next(rowCount + 1);
                var value = dataRows[randomIndex];
                dataRows[randomIndex] = dataRows[rowCount];
                dataRows[rowCount] = value;
            }


            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    shuffledDataSet[i, j] = dataRows[i][j];
                }
            }


            return shuffledDataSet;
        }
    }
}
