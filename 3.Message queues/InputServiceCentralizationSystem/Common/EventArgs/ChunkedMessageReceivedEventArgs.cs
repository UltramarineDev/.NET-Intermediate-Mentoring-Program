using Common.Models;

namespace Common.EventArgs
{
    public class ChunkedMessageReceivedEventArgs<T> : System.EventArgs
    {
        public MessageSequence<T> Message { get; }

        public ChunkedMessageReceivedEventArgs(MessageSequence<T> message)
        {
            Message = message;
        }
    }
}
