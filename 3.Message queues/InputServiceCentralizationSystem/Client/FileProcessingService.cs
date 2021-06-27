using System;
using System.IO;
using Client.EventArgs;
using Common;
using Common.Models;

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
            var message = new FileMessage
            {
                FileName = Path.GetFileName(path),
                Data = File.ReadAllBytes(path)
            };
            
            _queueClient.PublishMessage(message);
            
            Console.WriteLine("Message sent.");

            Console.ReadLine();
        }
    }
}
