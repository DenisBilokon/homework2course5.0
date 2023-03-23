using System.Threading;
using System;

class Matrix
{
    private int[,] matrix;
    public int Rows { get; private set; }
    public int Columns { get; private set; }

    public Matrix(int rows, int columns)
    {
        matrix = new int[rows, columns];
        Rows = rows;
        Columns = columns;
    }

    public int GetValue(int row, int column)
    {
        return matrix[row, column];
    }

    public void SetValue(int row, int column, int value)
    {
        matrix[row, column] = value;
    }

    public static Matrix Multiply(Matrix a, Matrix b)
    {
        if (a.Columns != b.Rows)
        {
            throw new ArgumentException("The number of columns in the first matrix must equal the number of rows in the second matrix.");
        }

        Matrix result = new Matrix(a.Rows, b.Columns);

        int numberOfThreads = Environment.ProcessorCount;
        Semaphore semaphore = new Semaphore(numberOfThreads, numberOfThreads);

        for (int i = 0; i < result.Rows; i++)
        {
            semaphore.WaitOne();

            ThreadPool.QueueUserWorkItem((state) =>
            {
                int row = (int)state;

                for (int j = 0; j < result.Columns; j++)
                {
                    int sum = 0;

                    for (int k = 0; k < a.Columns; k++)
                    {
                        sum += a.GetValue(row, k) * b.GetValue(k, j);
                    }

                    lock (result)
                    {
                        result.SetValue(row, j, sum);
                    }
                }

                semaphore.Release();
            }, i);
        }

        for (int i = 0; i < numberOfThreads; i++)
        {
            semaphore.WaitOne();
        }

        return result;
    }
}
