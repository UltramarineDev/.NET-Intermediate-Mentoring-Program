namespace Client.EventArgs
{
    internal class FileCreatedEventArgs: System.EventArgs
    {
        public string Path { get; }

        public FileCreatedEventArgs(string path)
        {
            Path = path;
        }
    }
}
