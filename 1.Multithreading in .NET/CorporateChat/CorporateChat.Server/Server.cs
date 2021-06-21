using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using CorporateChat.Models;

namespace CorporateChat.Server
{
    internal class Server
    {
        private readonly ChatLogs<string> _receivedMessages;
        private readonly List<TcpClient> _connectedClients;

        private const int Port = 13000;

        public Server()
        {
            _receivedMessages = new ChatLogs<string>(10);
            _connectedClients = new List<TcpClient>();
        }

        public void Listen()
        {
            TcpListener server = null;
            try
            {
                var localAddr = IPAddress.Parse("127.0.0.1");
                
                server = new TcpListener(localAddr, Port);
                server.Start();

                while (true)
                {
                    Console.WriteLine("Waiting for a connection... ");

                    var client = server.AcceptTcpClient();

                    Console.WriteLine("Connected");

                    SendMessageHistory(client);
                    _connectedClients.Add(client);
                    
                    var stream = client.GetStream();

                    var formatter = new BinaryFormatter();

                    var messageInfo = (ChatMessageInfo)formatter.Deserialize(stream);

                    Console.WriteLine($"Received from client {messageInfo.ClientName}: {messageInfo.Message}");
                    _receivedMessages.Add(messageInfo.Message);

                    SendMessageToClients(messageInfo.Message);

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception : {ex}");
            }
            finally
            {
                SendStopNotification();
                server?.Stop();
            }
        }

        private void SendMessageToClients(string message)
        {
            foreach (var client in _connectedClients)
            {
                if (!client.Connected)
                {
                    continue;
                }
                
                var stream = client.GetStream();

                var bytes = Encoding.ASCII.GetBytes(message);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        private void SendMessageHistory(TcpClient client)
        {
            if (_connectedClients.Contains(client))
            {
                return;
            }

            var stream = client.GetStream();

            foreach (var message in _receivedMessages)
            {
                var bytes = Encoding.ASCII.GetBytes(message);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        private void SendStopNotification()
        {
            const string message = "Server stopping...";
            
            foreach (var client in _connectedClients)
            {
                if (!client.Connected)
                {
                    continue;
                }
                
                var stream = client.GetStream();

                var bytes = Encoding.ASCII.GetBytes(message);
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
