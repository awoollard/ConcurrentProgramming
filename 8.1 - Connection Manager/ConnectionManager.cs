using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using AWoollard.Concurrent.Utils;

namespace _8._1___Connection_Manager
{
    public class ConnectionManager : ActiveObject
    {
        private readonly List<Socket> _sockets;
        private readonly Socket _serverSocket;

        public ConnectionManager(int port = 64646, int maxConnections = 16)
        {
            _sockets = new List<Socket>();
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            _serverSocket.Listen(maxConnections);

            _sockets.Add(_serverSocket);
        }

        protected override void Run()
        {
            while (true)
            {
                Socket.Select(_sockets, null, null, -1);
                if (_sockets[0] == _serverSocket)
                {
                    var client = _sockets[0].Accept();
                    _sockets.Add(client);
                    _sockets.Remove(_serverSocket);

                }

                foreach (var socket in _sockets)
                {
                    if (socket.Available == 0)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                    else
                    {
                        Console.WriteLine("received data");
                    }
                }
            }
        }
    }
}