using System;

namespace Common.Models
{
    [Serializable]
    public class MessageSequence<T>
    {
        public int SequenceId { get; set; }
        public long Position { get; set; }
        public long Size { get; set; }
        public T Body { get; set; }
    }
}
