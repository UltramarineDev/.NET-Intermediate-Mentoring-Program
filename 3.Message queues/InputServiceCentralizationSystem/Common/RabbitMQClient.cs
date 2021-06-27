using System;
using Common.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common
{
    public class RabbitMQClient
    {
        private const string HostName = "localhost";
        private const string UserName = "root";
        private const string Password = "root";
        private const string QueueName = "fileQueue";

        private IModel _channel;
        
        public event EventHandler<MessageReceivedEventArgs> MessageReceivedEvent;

        public RabbitMQClient()
        {
            ConnectToQueue();
        }

        private void ConnectToQueue()
        {
            var factory = new ConnectionFactory()
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public void PublishMessage(byte[] body)
        {
            _channel.BasicPublish(exchange: string.Empty, routingKey: QueueName, basicProperties: null, body: body);
        }

        public void Subscribe()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += HandleReceiveMessage;

            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        }

        private void HandleReceiveMessage(object model, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            
            Console.WriteLine("Message received.");

            var messageReceivedEvent = MessageReceivedEvent;

            if (messageReceivedEvent == null)
            {
                return;
            }

            var eventArgs = new MessageReceivedEventArgs(body);

            messageReceivedEvent(this, eventArgs);
        }
    }
}
