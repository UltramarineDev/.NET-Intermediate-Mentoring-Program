using System;
using Common.EventArgs;
using Common.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common
{
    public class RabbitMQClient : RabbitMQClientBase
    {
        private const string QueueName = "fileQueue";

        public event EventHandler<MessageReceivedEventArgs> MessageReceivedEvent;

        public RabbitMQClient() : base(QueueName) { }

        public void Subscribe()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += HandleReceiveMessage;

            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        }

        private void HandleReceiveMessage(object model, BasicDeliverEventArgs e)
        {
            var message = GetMessage<FileMessage>(e);

            var messageReceivedEvent = MessageReceivedEvent;

            if (messageReceivedEvent == null)
            {
                return;
            }

            var eventArgs = new MessageReceivedEventArgs(message);

            messageReceivedEvent(this, eventArgs);
        }
    }
}
