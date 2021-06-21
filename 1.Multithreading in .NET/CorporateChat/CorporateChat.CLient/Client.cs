﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CorporateChat.Models;

namespace CorporateChat.CLient
{
    internal class Client
    {
        private readonly List<string> _messages;

        private const int MessagesSize = 10;
        private const int Port = 13000;
        private const int BufferSize = 1024;

        private bool _shouldStop;

        public Client()
        {
            _messages = new List<string>();
            InitializeMessages();
        }

        public void Start()
        {
            try
            {
                while (!_shouldStop)
                {
                    Task.Run(ConnectToServer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception: {ex}");
            }
        }

        public void Stop()
        {
            _shouldStop = true;
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

        private static string GetClientName()
        {
            var rnd = new Random();
            return $"Client {rnd.Next(MessagesSize)}";
        }

        private void SendMessages(Stream stream, ChatMessageInfo messageInfo)
        {
            var formatter = new BinaryFormatter();

            var rnd = new Random();
            var countToSend = rnd.Next(MessagesSize);

            for (var i = 0; i < countToSend; i++)
            {
                messageInfo.Message = GetMessageToSend();
                formatter.Serialize(stream, messageInfo);

                Console.WriteLine($"Sent from {messageInfo.ClientName}: {messageInfo.Message}");
                Thread.Sleep(rnd.Next(MessagesSize) * 1000);
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
            var rnd = new Random();
            var randomNumber = rnd.Next(_messages.Count);

            return _messages[randomNumber];
        }

        private static void ReceiveMessage(Stream stream)
        {
            var buffer = new byte[BufferSize];

            int length;
            while ((length = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                var data = Encoding.ASCII.GetString(buffer, 0, length);
                Console.WriteLine($"Received: {data}");
            }
        }
    }
}
