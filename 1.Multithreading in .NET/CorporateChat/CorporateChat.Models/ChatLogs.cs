using System.Collections;
using System.Collections.Generic;

namespace CorporateChat.Models
{
    public class ChatLogs<T> : IEnumerable<T>
    {
        private readonly Queue<T> _queue;
        private readonly int _maxCount;

        public ChatLogs(int maxCount)
        {
            _maxCount = maxCount;
            _queue = new Queue<T>(maxCount);
        }

        public void Add(T item)
        {
            if (_queue.Count == _maxCount)
            {
                _queue.Dequeue();
            }
            
            _queue.Enqueue(item);
        }

        public IEnumerator<T> GetEnumerator() => _queue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
