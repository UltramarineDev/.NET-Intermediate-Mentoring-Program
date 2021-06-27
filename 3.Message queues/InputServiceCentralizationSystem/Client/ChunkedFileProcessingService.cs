using System;
using System.IO;
using Common;
using Common.Models;

namespace Client
{
    internal class ChunkedFileProcessingService : FileProcessingServiceBase
    {
        private readonly RabbitMQChunkedMessageClient _queueClient;

        public ChunkedFileProcessingService(FileSystemListener listener): base(listener)
        {
            _queueClient = new RabbitMQChunkedMessageClient();
        }
        
        protected override void SendMessage(string path)
        {
            try
            {
                using var fileStream = File.OpenRead(path);
                using var streamReader = new StreamReader(fileStream);

                var remainingFileSize = Convert.ToInt32(fileStream.Length);
                var totalFileSize = Convert.ToInt32(fileStream.Length);

                while (remainingFileSize > 0)
                {
                    int bytesRead;
                    byte[] data;

                    if (remainingFileSize > ChunkSize)
                    {
                        data = new byte[ChunkSize];
                        bytesRead = fileStream.Read(data, 0, ChunkSize);
                    }
                    else
                    {
                        data = new byte[remainingFileSize];
                        bytesRead = fileStream.Read(data, 0, remainingFileSize);
                    }

                    var message = new MessageSequence<FileMessage>
                    {
                        SequenceId = 1,
                        Body = new FileMessage { FileName = Path.GetFileName(path), Data = data },
                        Size = totalFileSize,
                        Position = bytesRead
                    };

                    _queueClient.PublishMessage(message);

                    remainingFileSize -= bytesRead;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not read file. Exception: {ex}");
            }
        }
    }
}
