using NeuralNetwork.NeuralNetwork.DataSet.Strategies;
using System.Collections.Generic;

namespace NeuralNetwork.DataSet.Strategies.ConcreteDataEncodingStrategies
{
    public class OneHot : IDataEncodingStrategy
    {
        string[,] _dataSet;
        int[] _columnIndexesToEncode;
        int _numberOfAddedColumns;

        private List<string[]> _encodedData = new List<string[]>();
        private List<string[]> _encodedDataSet = new List<string[]>();

        private HashSet<string> _uniqueCategoricalColumnValues = new HashSet<string>();

        public OneHot()
        {
            
        }

        public string[,] Encode(string[,] dataSet, int[] columnIndexesToEncode)
        {
            _dataSet = dataSet;
            _columnIndexesToEncode = columnIndexesToEncode;

            _encodedDataSet = ConvertDataArrayIntoColumnList(_dataSet);

            int numberOfColumnsToEncode = _columnIndexesToEncode.Length;

            for (int i = 0; i < numberOfColumnsToEncode; i++)
            {
                _uniqueCategoricalColumnValues.Clear();

                int columnIndexToEncode = _columnIndexesToEncode[i];

                FetchUniqueCategoricalValuesInColumn(columnIndexToEncode);

                var binaryValueDummyColumns = ConvertCategoricalDataIntoBinaryData(columnIndexToEncode);

                InsertDummyColumnsIntoDataSet(columnIndexToEncode, binaryValueDummyColumns);
            }

            var encodedDataSet = ConvertColumnListIntoDataArray(_encodedDataSet);

            return encodedDataSet;
        }

        public void FetchUniqueCategoricalValuesInColumn(int columnIndexToEncode)
        {
            int numberOfDataRows = _dataSet.GetLength(0);

            for (int j = 0; j < numberOfDataRows; j++)
            {
                var dataValue = _dataSet[j, columnIndexToEncode];
                _uniqueCategoricalColumnValues.Add(dataValue);
            }

        }

        public List<string[]> ConvertCategoricalDataIntoBinaryData(int columnIndexToEncode)
        {
            int numberOfDummyColumnsToCreate = _uniqueCategoricalColumnValues.Count;
            var encodedDummyColumns = CreateDummyColumns(numberOfDummyColumnsToCreate);

            string lowValue;
            string highValue;

            //experience shows that categorical data only consisting of two unique values are more efficiently encoded by -1 and 1
            if(_uniqueCategoricalColumnValues.Count == 2)
            {
                lowValue = "-1";
                highValue = "1";
            }
            else
            {
                lowValue = "0.1";
                highValue = "0.9";
            }

            int numberOfDataRows = _dataSet.GetLength(0);
            int dummyIndex = 0;
            foreach (var categoryValue in _uniqueCategoricalColumnValues)
            {

                for (int i = 0; i < numberOfDataRows; i++)
                {
                    if (_dataSet[i, columnIndexToEncode] == categoryValue)
                    {
                        encodedDummyColumns[dummyIndex][i] = highValue;
                    }
                    else
                    {
                        encodedDummyColumns[dummyIndex][i] = lowValue;
                    }


                }
                dummyIndex++;
            }

            return encodedDummyColumns;
        }

        private List<string[]> CreateDummyColumns(int numberOfUniqueValues)
        {
            int numberOfDataRows = _dataSet.GetLength(0);
            List<string[]> dummyColumns = new List<string[]>();

            for (int i = 0; i < numberOfUniqueValues; i++)
            {
                string[] dummyColumn = new string[numberOfDataRows];

                dummyColumns.Add(dummyColumn);
            }

            return dummyColumns;

        }

        private List<string[]> ConvertDataArrayIntoColumnList(string[,] data)
        {
            List<string[]> dataColumnsInListForm = new List<string[]>();

            int numberOfRows = data.GetLength(0);
            int numberOfColumns = data.GetLength(1);

            for (int i = 0; i < numberOfColumns; i++)
            {
                var column = new string[numberOfRows];

                for (int j = 0; j < numberOfRows; j++)
                {
                    column[j] = data[j, i];
                }

                dataColumnsInListForm.Add(column);
            }

            return dataColumnsInListForm;
        }

        private string[,] ConvertColumnListIntoDataArray(List<string[]> columnList)
        {
            int numberOfColumns = columnList.Count;
            int numberOfRows = columnList[0].GetLength(0); //gets the number of rows in one column, which will be the number of datarows in data array

            string[,] dataArray = new string[numberOfRows, numberOfColumns];

            for (int i = 0; i < numberOfColumns; i++)
            {
                for (int j = 0; j < numberOfRows; j++)
                {
                    dataArray[j, i] = columnList[i][j];
                }
            }

            return dataArray;
        }

        private void InsertDummyColumnsIntoDataSet(int columnIndexToEncode, List<string[]> dummyColumnsToInsert)
        {
            int columnIndexToInsertAt = columnIndexToEncode + _numberOfAddedColumns;

            _encodedDataSet.InsertRange(columnIndexToInsertAt, dummyColumnsToInsert);

            _numberOfAddedColumns += dummyColumnsToInsert.Count;

            int indexOfOldColumn = columnIndexToEncode + _numberOfAddedColumns;

            _encodedDataSet.RemoveAt(indexOfOldColumn);

            _numberOfAddedColumns--;





        }


    }
}
