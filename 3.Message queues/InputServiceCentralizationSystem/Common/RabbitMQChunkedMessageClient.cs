using System;
using Common.EventArgs;
using Common.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common
{
    public class RabbitMQChunkedMessageClient : RabbitMQClientBase
    {
        private const string QueueName = "fileChunkedQueue";

        public event EventHandler<ChunkedMessageReceivedEventArgs<FileMessage>> ChunkedMessageReceivedEvent;

        public RabbitMQChunkedMessageClient() : base(QueueName) { }

        public void SubscribeToChunked()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += HandleReceiveChunkedMessage;

            _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
        }

        private void HandleReceiveChunkedMessage(object model, BasicDeliverEventArgs e)
        {
            var message = GetMessage<MessageSequence<FileMessage>>(e);

            var messageReceivedEvent = ChunkedMessageReceivedEvent;

            if (messageReceivedEvent == null)
            {
                return;
            }

            var eventArgs = new ChunkedMessageReceivedEventArgs<FileMessage>(message);

            messageReceivedEvent(this, eventArgs);
        }
    }
}
