using System;
using System.IO;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path to directory:");
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

            var listener = new FileSystemListener(path);

           // _ = new FileProcessingService(listener);
            _ = new ChunkedFileProcessingService(listener);

            listener.Listen();
        }
    }
}
