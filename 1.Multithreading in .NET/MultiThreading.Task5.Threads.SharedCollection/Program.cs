/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> _collection = new List<int>();
        private static ManualResetEvent manualEvent = new ManualResetEvent(false);
        private static object locker = new object();
        private const int ElementsCount = 10;
        private static bool IsCompleted = false;

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the " +
                              "second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            ManipulateWithCollection();

            Console.ReadLine();
        }

        private static void ManipulateWithCollection()
        {
            var firstTask = Task.Factory.StartNew(AddElements);
            var secondTask = Task.Factory.StartNew(PrintElements);

            Task.WaitAll(firstTask, secondTask);
        }

        private static void AddElements()
        {
            for (var i = 0; i < ElementsCount; i++)
            {
                lock (locker)
                {
                    Thread.Sleep(1000);
                    _collection.Add(i);
                    Console.WriteLine($"Created element {i}");
                }

                manualEvent.Set();
            }

            IsCompleted = true;
        }

        private static void PrintElements()
        {
            while (!IsCompleted)
            {
                manualEvent.WaitOne();
                Console.WriteLine("Thread got manual event");

                foreach (var element in _collection)
                {
                    Console.WriteLine(element);
                }

                manualEvent.Reset();
            }
        }
    }
}
