using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using CorporateChat.Models;

namespace CorporateChat.Server
{
    internal class Server
    {
        private readonly ChatLogs<string> _receivedMessages;
        private readonly Dictionary<string, TcpClient> _connectedClients;
        private const int Port = 13000;
        private const string StopMessage = "Server stopping...";

        public Server()
        {
            _receivedMessages = new ChatLogs<string>(10);
            _connectedClients = new Dictionary<string, TcpClient>();
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

                    var thread = new Thread(() => ProcessClient(client));
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception : {ex}");
            }
            finally
            {
                SendMessageToClients(StopMessage);
                server?.Stop();
            }
        }

        private void ProcessClient(TcpClient client)
        {
            while (true)
            {
                try
                {
                    var stream = client.GetStream();

                    var formatter = new BinaryFormatter();
                    var messageInfo = (ChatMessageInfo)formatter.Deserialize(stream);

                    Console.WriteLine($"Received from client {messageInfo.ClientName}: {messageInfo.Message}");

                    SendMessageHistory(client, messageInfo.ClientName);

                    SendMessageToClients(messageInfo.Message);

                    UpdateStoredData(messageInfo, client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to process client: {ex}");
                    return;
                }
            }
        }

        private void UpdateStoredData(ChatMessageInfo messageInfo, TcpClient client)
        {
            if (messageInfo == null || client == null)
            {
                return;
            }

            if (!_connectedClients.ContainsKey(messageInfo.ClientName))
            {
                _connectedClients.Add(messageInfo.ClientName, client);
            }

            _receivedMessages.Add(messageInfo.Message);
        }

        private void SendMessageToClients(string message)
        {
            var clients = _connectedClients.Values.Where(client => client.Connected);

            foreach (var client in clients)
            {
                var stream = client.GetStream();

                var bytes = Encoding.ASCII.GetBytes(message);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
        }

        private void SendMessageHistory(TcpClient client, string clientName)
        {
            if (_connectedClients.ContainsKey(clientName))
            {
                return;
            }

            var stream = client.GetStream();

            foreach (var message in _receivedMessages)
            {
                var bytes = Encoding.ASCII.GetBytes(message);
                stream.Write(bytes, 0, bytes.Length);
            }

            stream.Flush();
        }
    }
}
