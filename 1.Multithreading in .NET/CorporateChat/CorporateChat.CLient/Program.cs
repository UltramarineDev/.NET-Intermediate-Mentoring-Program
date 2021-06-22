using System;

namespace CorporateChat.CLient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press 'q' to exit");

            var client = new Client();
            client.Start();

            var input = Console.ReadLine();

            if (input == "q")
            {
                client.Stop();
            }
        }
    }
}
