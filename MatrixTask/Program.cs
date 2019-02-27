using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixTask
{
    class Program
    {
        private static bool ValidateMatrix(double[,] matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            return true;
        }

        private static double GetMatrixProductElement(double[,] a, double[,] b, int row, int column)
        {
            ValidateMatrix(a);
            ValidateMatrix(b);

            var rows = a.GetLength(0);
            var columns = b.GetLength(1);

            var result = 0.0;

            for (int currentColumn = 0; currentColumn < columns; currentColumn++)
            {
                for (int currentRow = 0; currentRow < rows; currentRow++)
                {
                    var first = a[row, currentColumn];
                    var second = b[currentRow, column];
                    result += a[row, currentColumn] * b[currentRow, column];
                }
            }

            return result;
        }

        public static double[,] GetMatrixProductAsync(double[,] a, double[,] b)
        {
            ValidateMatrix(a);
            ValidateMatrix(b);

            if (a.GetLength(1) != b.GetLength(0))
            {
                throw new InvalidOperationException("Number of columns of first matrix" +
                        "must be equal to number of rows of second matrix");
            }

            var rows = a.GetLength(0);
            var columns = b.GetLength(1);

            var resultsArray = new Task<double>[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var currentRow = row;
                    var currentColumn = column;
                    resultsArray[row, column] =
                        Task<double>.Factory.StartNew(() => GetMatrixProductElement(
                            a, 
                            b, 
                            currentRow, 
                            currentColumn));
                }
            }

            var result = new double[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    result[row, column] = resultsArray[row, column].Result;
                }
            }

            return result;
        }

        static void Main(string[] args)
        {
            var a = new double[,]
            {
                { 2, 0, -1},
                { 0, -2, 2}
            };

            var b = new double[,]
            {
                {4, 1, 0},
                {3, 2, 1},
                {0, 1, 0}
            };

            var c = GetMatrixProductAsync(a, b);
        }
    }
}
