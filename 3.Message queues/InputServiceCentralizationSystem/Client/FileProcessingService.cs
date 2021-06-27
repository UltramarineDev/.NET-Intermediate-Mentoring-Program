using Client.Models;
using System;
using System.IO;
using Common;

namespace Client
{
    internal class FileProcessingService
    {
        private readonly RabbitMQClient _queueClient;
        
        public FileProcessingService(FileSystemListener listener)
        {
            listener.FileCreatedEvent += HandleFileCreatedEvent;

            _queueClient = new RabbitMQClient();
        }

        private void HandleFileCreatedEvent(object sender, FileCreatedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Path))
            {
                return;
            }

            Console.WriteLine($"Start processing {e.Path}");
            Send(e.Path);
        }

        private void Send(string path)
        {
            var bytes = File.ReadAllBytes(path);

            _queueClient.PublishMessage(bytes);
            
            Console.WriteLine("Message sent.");

            Console.ReadLine();
        }
    }
}
