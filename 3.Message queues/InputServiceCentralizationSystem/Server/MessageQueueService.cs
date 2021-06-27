using System;
using System.IO;
using Common;
using Common.EventArgs;

namespace Server
{
    internal class MessageQueueService : MessageQueueServiceBase
    {
        private readonly RabbitMQClient _queueClient;

        public MessageQueueService(string pathToWrite) : base(pathToWrite)
        {
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
