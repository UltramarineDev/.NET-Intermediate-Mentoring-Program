using System;

namespace CorporateChat.CLient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press 'q' to exit");
           // var input = Console.ReadLine();

            var client = new Client();
            client.Start();

            //if (input == "q")
            //{
            //    client.Stop();
            //}
        }
    }
}
