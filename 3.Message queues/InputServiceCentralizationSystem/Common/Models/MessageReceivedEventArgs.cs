using System;

namespace Common.Models
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public byte[] Body { get; }

        public MessageReceivedEventArgs(byte[] body)
        {
            Body = body;
        }
    }
}
