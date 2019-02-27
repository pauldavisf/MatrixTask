using System;
using System.Threading.Tasks;

namespace MatrixTask
{
    public static class MatrixOperations
    {
        public static void ValidateMatrix(double[,] matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }
        }

        public static double GetMatrixProductElement(double[,] a, double[,] b, int row, int column)
        {
            ValidateMatrix(a);
            ValidateMatrix(b);

            var columns = a.GetLength(1);

            var result = 0.0;

            for (int k = 0; k < columns; k++)
            {
                result += a[row, k] * b[k, column];
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

        public static void PrintMatrix(double[,] matrix)
        {
            ValidateMatrix(matrix);

            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    Console.Write(matrix[row, column] + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}
