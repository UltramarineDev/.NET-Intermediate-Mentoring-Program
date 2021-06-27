using System;
using Client.EventArgs;

namespace Client
{
    internal abstract class FileProcessingServiceBase
    {
        protected const int ChunkSize = 10240;

        protected FileProcessingServiceBase(FileSystemListener listener)
        {
            listener.FileCreatedEvent += HandleFileCreatedEvent;
        }

        protected void HandleFileCreatedEvent(object sender, FileCreatedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Path))
            {
                return;
            }

            Console.WriteLine($"Start processing {e.Path}");
            
            SendMessage(e.Path);
        }

        protected abstract void SendMessage(string path);
    }
}
