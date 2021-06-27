using Common.Models;

namespace Common.EventArgs
{
    public class MessageReceivedEventArgs : System.EventArgs
    {
        public FileMessage Message { get; }

        public MessageReceivedEventArgs(FileMessage message)
        {
            Message = message;
        }
    }
}
