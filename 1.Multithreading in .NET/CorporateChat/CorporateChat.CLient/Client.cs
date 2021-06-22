using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using CorporateChat.Models;

namespace CorporateChat.CLient
{
    internal class Client
    {
        private readonly List<string> _messages;
        private readonly Random _random;

        private const int MessagesSize = 10;
        private const int Port = 13000;
        private const int BufferSize = 1024;

        public Client()
        {
            _random = new Random();
            _messages = new List<string>();
            
            InitializeMessages();
        }

        public void Start(object obj)
        {
            var cancellationToken = (CancellationToken)obj;
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    ConnectToServer();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception: {ex}");
            }
        }

        private void ConnectToServer()
        {
            try
            {
                var client = new TcpClient("localhost", Port);

                using var stream = client.GetStream();

                var messageInfo = new ChatMessageInfo { ClientName = GetClientName() };

                SendMessages(stream, messageInfo);

                ReceiveMessage(stream);

                client.Close();
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket exception: {ex}");
            }
        }

        private string GetClientName()
        {
            return $"Client {_random.Next(100)}";
        }

        private void SendMessages(Stream stream, ChatMessageInfo messageInfo)
        {
            var formatter = new BinaryFormatter();

            var countToSend = _random.Next(MessagesSize);

            for (var i = 0; i < countToSend; i++)
            {
                messageInfo.Message = GetMessageToSend();
                formatter.Serialize(stream, messageInfo);

                Console.WriteLine($"Sent from {messageInfo.ClientName}: {messageInfo.Message}");
                Thread.Sleep(_random.Next(MessagesSize) * 100);
            }
        }

        private void InitializeMessages()
        {
            for (var i = 0; i < MessagesSize; i++)
            {
                _messages.Add($"Test message {i}...");
            }
        }

        private string GetMessageToSend()
        {
            var randomNumber = _random.Next(_messages.Count);

            return _messages[randomNumber];
        }

        private static void ReceiveMessage(Stream stream)
        {
            var buffer = new byte[BufferSize];

            stream.Read(buffer, 0, buffer.Length);
            var data = Encoding.ASCII.GetString(buffer);
            
            Console.WriteLine($"Received: {data}");
        }
    }
}
