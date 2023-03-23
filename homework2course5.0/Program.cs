using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static Queue<int> queue = new Queue<int>();
    static object lockObject = new object();

    static void Main(string[] args)
    {
        Thread producerThread = new Thread(Producer);
        Thread consumerThread = new Thread(Consumer);
       producerThread.Start();
       consumerThread.Start();
        producerThread.Join();
        consumerThread.Join();
    }

    static void Producer()
    {
        Random random = new Random();
        while (true)
        {
            lock (lockObject)
            {
                int number = random.Next(1, 101);
                queue.Enqueue(number);
                Console.WriteLine($"Producer produced {number}");
                Monitor.Pulse(lockObject);
            }
            Thread.Sleep(random.Next(500, 1001));
        }
    }

    static void Consumer()
    {
        while (true)
        {
            lock (lockObject)
            {
                while (queue.Count == 0)
                {
                    Monitor.Wait(lockObject);
                }
                int number = queue.Dequeue();
                Console.WriteLine($"Consumer consumed {number}");
            }
            Thread.Sleep(1000);
        }
    }
}
