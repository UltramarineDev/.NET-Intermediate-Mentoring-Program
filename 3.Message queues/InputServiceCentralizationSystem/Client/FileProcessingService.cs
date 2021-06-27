using System;
using System.IO;
using Common;
using Common.Models;

namespace Client
{
    internal class FileProcessingService : FileProcessingServiceBase
    {
        private readonly RabbitMQClient _queueClient;

        public FileProcessingService(FileSystemListener listener) : base(listener)
        {
            listener.FileCreatedEvent += HandleFileCreatedEvent;

            _queueClient = new RabbitMQClient();
        }

        protected override void SendMessage(string path)
        {
            byte[] data;
            try
            {
                data = File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not read file. Exception: {ex}");
                return;
            }

            var message = new FileMessage
            {
                FileName = Path.GetFileName(path),
                Data = data
            };

            _queueClient.PublishMessage(message);

            Console.WriteLine("Message sent.");

            Console.ReadLine();
        }
    }
}
