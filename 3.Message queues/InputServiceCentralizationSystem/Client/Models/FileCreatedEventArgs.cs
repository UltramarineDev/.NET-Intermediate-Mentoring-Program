using System;

namespace Client.Models
{
    internal class FileCreatedEventArgs: EventArgs
    {
        public string Path { get; }

        public FileCreatedEventArgs(string path)
        {
            Path = path;
        }
    }
}
