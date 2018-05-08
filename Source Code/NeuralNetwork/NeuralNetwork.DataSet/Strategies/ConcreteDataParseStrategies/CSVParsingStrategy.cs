using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataSet.Strategies.ConcreteStrategies
{
    public class CSVParsingStrategy : IDataParseStrategy
    {
        public string[,] ParseData(string dataFilePath, bool firstRowHeader)
        {

            int firstRowHeaderFactor = firstRowHeader ? 1 : 0;

            string[,] rawDataSet;
            string[,] rawDataSetMinusHeader;

            string dataFile = File.ReadAllText(dataFilePath);

            dataFile = dataFile.Replace('\n', '\r');

            string[] lines = dataFile.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);


            int numberOfRows = lines.Length;
            int numberOfColumns = lines[0].Split(',').Length;


            rawDataSet = new string[numberOfRows, numberOfColumns];
            rawDataSetMinusHeader = new string[numberOfRows - firstRowHeaderFactor, numberOfColumns];

            for (int i = 0; i < numberOfRows; i++)
            {
                string[] line_r = lines[i].Split(',');
                for (int j = 0; j < numberOfColumns; j++)
                {
                    rawDataSet[i, j] = line_r[j];
                }
            }

            for (int i = 0; i < rawDataSetMinusHeader.GetLength(0); i++)
            {
                for (int j = 0; j < rawDataSetMinusHeader.GetLength(1); j++)
                {
                    rawDataSetMinusHeader[i, j] = rawDataSet[i + firstRowHeaderFactor, j];               
                }
            }

            return rawDataSetMinusHeader;
        }
    }
}
