﻿namespace CorporateChat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();
            server.Listen();
        }
    }
}
