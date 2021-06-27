using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Common.EventArgs;
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

        public void PublishMessage(FileMessage message)
        {
            if (message == null)
            {
                return;
            }

            var formatter = new BinaryFormatter();
            using var stream = new MemoryStream();

            formatter.Serialize(stream, message);

            var data = stream.ToArray();

            _channel.BasicPublish(exchange: string.Empty, routingKey: QueueName, basicProperties: null, body: data);
        }

        public void Subscribe()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += HandleReceiveMessage;

            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        }

        private void HandleReceiveMessage(object model, BasicDeliverEventArgs e)
        {
            var bytes = e.Body.ToArray();

            Console.WriteLine("Message received.");
            
            var formatter = new BinaryFormatter();
            
            using var stream = new MemoryStream(bytes);

            var message = (FileMessage)formatter.Deserialize(stream);
            
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
