using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CorporateChat.Models
{
    public class ChatLogs<T> : IEnumerable<T>
    {
        private readonly ConcurrentQueue<T> _queue;
        private readonly int _maxCount;

        public ChatLogs(int maxCount)
        {
            _maxCount = maxCount;
            _queue = new ConcurrentQueue<T>();
        }

        public void Add(T item)
        {
            if (_queue.Count == _maxCount)
            {
                _queue.TryDequeue(out _);
            }
            
            _queue.Enqueue(item);
        }

        public IEnumerator<T> GetEnumerator() => _queue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
