using System;
using System.IO;
using Common;
using Common.EventArgs;
using Common.Models;

namespace Server
{
    internal class ChunkedMessageQueueService
    {
        private readonly RabbitMQChunkedMessageClient _queueClient;
        private readonly string _pathToWrite;

        public ChunkedMessageQueueService(string pathToWrite)
        {
            _pathToWrite = pathToWrite;
            _queueClient = new RabbitMQChunkedMessageClient();
            _queueClient.ChunkedMessageReceivedEvent += ProcessChunkedMessage;
        }

        public void Listen()
        {
            _queueClient.SubscribeToChunked();

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private void ProcessChunkedMessage(object sender, ChunkedMessageReceivedEventArgs<FileMessage> e)
        {
            var message = e.Message;
            if (message == null)
            {
                return;
            }

            if (message.Position >= message.Size)
            {
                return;
            }

            var body = message.Body;

            if (body == null)
            {
                return;
            }

            using var fileStream = new FileStream($"{_pathToWrite}\\{body.FileName}", FileMode.Append);

            fileStream.Write(body.Data, 0, body.Data.Length);
            fileStream.Flush();

            Console.WriteLine("Chunk saved.");
        }
    }
}
