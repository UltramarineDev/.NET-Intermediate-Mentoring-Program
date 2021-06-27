using System;
using System.IO;
using Client.EventArgs;

namespace Client
{
    public class FileSystemListener
    {
        private readonly string _directoryToListen;

        internal event EventHandler<FileCreatedEventArgs> FileCreatedEvent;

        public FileSystemListener(string path)
        {
            _directoryToListen = path;
        }

        public void Listen()
        {
            using var watcher = new FileSystemWatcher(_directoryToListen);

            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Filter = "*.pdf";
            watcher.EnableRaisingEvents = true;

            watcher.WaitForChanged(WatcherChangeTypes.Changed);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var fileCreatedEvent = FileCreatedEvent;

            if (fileCreatedEvent == null)
            {
                return;
            }

            var eventArgs = new FileCreatedEventArgs(e.FullPath);

            fileCreatedEvent(this, eventArgs);
        }
    }
}
