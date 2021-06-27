using Common;
using System;
using System.IO;
using Common.EventArgs;

namespace Server
{
    internal class MessageQueueService
    {
        private readonly RabbitMQClient _queueClient;
        private readonly string _pathToWrite;

        public MessageQueueService(string pathToWrote)
        {
            _pathToWrite = pathToWrote;
            _queueClient = new RabbitMQClient();
            _queueClient.MessageReceivedEvent += ProcessMessage;
        }

        public void Listen()
        {
            _queueClient.Subscribe();

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private void ProcessMessage(object sender, MessageReceivedEventArgs e)
        {
            File.WriteAllBytes($"{_pathToWrite}\\{e.Message?.FileName}", e.Message?.Data);
        }
    }
}
