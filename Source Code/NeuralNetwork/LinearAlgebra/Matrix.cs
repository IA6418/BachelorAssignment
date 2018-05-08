using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra
{
    static public class Matrix
    {
        static public double[,] Multiply(double[,] matrixA, double[,] matrixB)
        {
            if (matrixA.GetLength(1) == matrixB.GetLength(0))
            {
                var resultMatrix = new double[matrixA.GetLength(0), matrixB.GetLength(1)];

                for (int i = 0; i < resultMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < resultMatrix.GetLength(1); j++)
                    {
                        for (int k = 0; k < matrixA.GetLength(1); k++)
                        {
                            resultMatrix[i, j] += matrixA[i, k] * matrixB[k, j];
                        }
                    }
                }

                return resultMatrix;
            }
            else
            {
                throw new Exception("The number of columns in the first matrix must be equal to the number of rows in the second matrix");
            }
        }

        static public double[] Multiply(double[,] matrixA, double[] matrixB)
        {
            if(matrixA.GetLength(1) == matrixB.GetLength(0))
            {
                var resultMatrix = new double[matrixA.GetLength(0)];

                for (int i = 0; i < resultMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrixA.GetLength(1); j++)
                    {
                        resultMatrix[i] += matrixA[i, j] * matrixB[j]; 
                    }
                }

                return resultMatrix;
            }
            else
            {
                throw new Exception("The number of columns in the first matrix must be equal to the number of rows in the second matrix");
            }
        }

        static public double[,] Add(double[,] matrixA, double[,] matrixB)
        {
            if (matrixA.GetLength(0) == matrixB.GetLength(0) && matrixA.GetLength(1) == matrixB.GetLength(1))
            {
                var resultMatrix = new double[matrixA.GetLength(0), matrixA.GetLength(1)];

                for (int i = 0; i < matrixA.GetLength(0); i++)
                {
                    for (int j = 0; j < matrixA.GetLength(1); j++)
                    {
                        resultMatrix[i, j] = matrixA[i, j] + matrixB[i, j];
                    }
                }

                return resultMatrix;
            }
            else
            {
                throw new Exception("Both matrices must have the same dimensions in order to be added together");
            }
        }

        static public double[] Add(double[] matrixA, double[] matrixB)
        {
            if(matrixA.GetLength(0) == matrixB.GetLength(0))
            {
                var resultMatrix = new double[matrixA.GetLength(0)];
                 
                for (int i = 0; i < matrixA.GetLength(0); i++)
                {
                        resultMatrix[i] = matrixA[i] + matrixB[i];
                }

                return resultMatrix;
            }
            else
            {
                throw new Exception("Both matrices must have the same dimensions in order to be added together");
            }
        }

        static public double[,] Transpose(double[,] matrix)
        {
            var transposedMatrix = new double[matrix.GetLength(1), matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    transposedMatrix[i, j] = matrix[j, i];
                }
            }

            return transposedMatrix;
        }

        static public void PrintMatrix(double[,] matrix)
        {
            for (int i = 0; i <= matrix.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= matrix.GetUpperBound(1); j++)
                {
                    Console.Write(matrix[i,j].ToString() + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static public void PrintMatrix(double[] matrix)
        {
            for (int i = 0; i <= matrix.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    Console.Write(matrix[i].ToString() + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
