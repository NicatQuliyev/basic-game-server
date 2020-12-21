using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;

namespace TcpListenerDebug
{
    class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static TcpListener tcpListener;

        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        public static void Start(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;

            Console.WriteLine("Starting server...");
            InitializeServerData();


            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectionCallback), null);

            Console.WriteLine($"Server started on *:{Port}");
        }

        private static void TCPConnectionCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectionCallback), null);

            Console.WriteLine($"Incomingg connection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if(clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private static void InitializeServerData()
        {
            for(int i = 1; i < MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
        }

    }
}
