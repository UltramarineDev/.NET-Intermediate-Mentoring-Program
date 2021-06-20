/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        private const int RandomMin = 0;
        private const int RandomMax = 1000;
        private const int RandomIntegersCount = 10;

        private static readonly Random Rand = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            ChainTasks();

            Console.ReadLine();
        }

        private static void ChainTasks()
        {
            var firstTask = Task.Run(GetRandomIntegers);

            var secondTask = firstTask.ContinueWith(antecedent => GetMultipliedArray(antecedent.Result));

            var thirdTask = secondTask.ContinueWith(antecedent =>
            {
                var array = antecedent.Result;
                Array.Sort(array);
                DisplayArray(array);

                return array;
            });

            thirdTask.ContinueWith(antecedent =>
            {
                Console.WriteLine("Fourth task: {0}", antecedent.Result.Average());
            });
        }

        private static int[] GetRandomIntegers()
        {
            var array = new int[RandomIntegersCount];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = Rand.Next(RandomMin, RandomMax);
                Console.WriteLine($"First task: {array[i]}");
            }

            return array;
        }

        private static int[] GetMultipliedArray(int[] array)
        {
            var rnd = Rand.Next(RandomMin, RandomMax);
            Console.WriteLine($"Second task random number: {rnd}");
            
            for (var i = 0; i < array.Length; i++)
            {
                array[i] *= rnd;
                Console.WriteLine("Second task: {0}", array[i]);
            }

            return array;
        }

        private static void DisplayArray(int[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                Console.WriteLine("Third task: {0}", array[i]);
            }
        }
    }
}
