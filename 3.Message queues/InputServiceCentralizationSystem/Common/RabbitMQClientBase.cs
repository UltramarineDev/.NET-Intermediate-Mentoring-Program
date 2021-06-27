using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common
{
    public class RabbitMQClientBase
    {
        private readonly string _queueName;
        private const string HostName = "localhost";
        private const string UserName = "root";
        private const string Password = "root";

        protected IModel _channel;

        public RabbitMQClientBase(string queueName)
        {
            _queueName = queueName;

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

            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public void PublishMessage<T>(T message)
        {
            if (message == null)
            {
                return;
            }

            var formatter = new BinaryFormatter();
            using var stream = new MemoryStream();

            formatter.Serialize(stream, message);

            var data = stream.ToArray();

            _channel.BasicPublish(exchange: string.Empty, routingKey: _queueName, basicProperties: null, body: data);
        }

        protected T GetMessage<T>(BasicDeliverEventArgs e) where T : class
        {
            var bytes = e.Body.ToArray();

            Console.WriteLine("Message received.");

            using var stream = new MemoryStream(bytes);

            var formatter = new BinaryFormatter();

            return (T)formatter.Deserialize(stream);
        }
    }
}
