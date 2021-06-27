using System;
using System.IO;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path to write:");
            var path = Console.ReadLine();

            try
            {
                Path.GetFullPath(path);
            }
            catch (Exception)
            {
                Console.WriteLine("Not valid path.");
                return;
            }

            var messageQueueService = new MessageQueueService(path);

            messageQueueService.Listen();
        }
    }
}
