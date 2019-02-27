namespace MatrixTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new double[,]
            {
                { 2, 1, 1},
                { 3, 0, 1}
            };

            var b = new double[,]
            {
                {3, 1},
                {2, 1},
                {1, 0}
            };

            var c = MatrixOperations.GetMatrixProductAsync(a, b);
            MatrixOperations.PrintMatrix(c);
        }
    }
}
