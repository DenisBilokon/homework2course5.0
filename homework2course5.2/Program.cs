using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        int[] array = new int[1000];
        Random rand = new Random();
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = rand.Next(1000);
        }

        QuickSortParallel(array, 0, array.Length - 1);

        foreach (int value in array)
        {
            Console.Write(value + " ");
        }
        Console.WriteLine();
    }

    static void QuickSortParallel(int[] array, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(array, left, right);

            Thread leftThread = new Thread(() => QuickSortParallel(array, left, pivotIndex - 1));
            Thread rightThread = new Thread(() => QuickSortParallel(array, pivotIndex + 1, right));
            leftThread.Start();
            rightThread.Start();

            leftThread.Join();
            rightThread.Join();
        }
    }

    static int Partition(int[] array, int left, int right)
    {
        int pivot = array[left];
        while (true)
        {
            while (array[left] < pivot)
            {
                left++;
            }

            while (array[right] > pivot)
            {
                right--;
            }

            if (left < right)
            {
                int temp = array[left];
                array[left] = array[right];
                array[right] = temp;

                left++;
                right--;
            }
            else
            {
                return right;
            }
        }
    }
}
