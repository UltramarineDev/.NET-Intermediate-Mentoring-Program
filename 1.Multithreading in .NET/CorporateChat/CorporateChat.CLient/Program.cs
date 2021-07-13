using System;
using System.Threading;

namespace CorporateChat.CLient
{
    class Program
    {
        public static object ServerClass { get; private set; }

        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            Console.WriteLine("Press 'C' to terminate the application...");
            var uiThread = new Thread(() =>
            {
                var key = Console.ReadLine();
                if (key.ToUpperInvariant() == "C")
                {
                    cts.Cancel();
                }
            });

            var client = new Client();
            var workerThread = new Thread(client.Start);

            uiThread.Start();
            workerThread.Start(cts.Token);

            workerThread.Join();
        }
    }
}
