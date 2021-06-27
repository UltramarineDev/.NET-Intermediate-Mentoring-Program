using Common;

namespace Server
{
    internal class MessageQueueServiceBase
    {
        protected readonly string _pathToWrite;

        public MessageQueueServiceBase(string pathToWrite)
        {
            _pathToWrite = pathToWrite;
        }
    }
}
