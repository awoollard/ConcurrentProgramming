using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using AWoollard.Concurrent.Utils;

namespace _8._1___Connection_Manager
{
    /// <summary>
    /// The Connection Manager is responsible for managing all incoming network communication to
    /// the program. The port to listen for connections is specified in the constructor, as well
    /// as the maximum amount of connections the server should support.
    /// It uses a single thread to accept incoming connections and to read messages from clients.
    /// By using a single thread, the Connection Manager avoids the issue of needing to create
    /// multiple threads to handle multiple connections.
    /// </summary>
    public class ConnectionManager : ActiveObject
    {
        private readonly List<Socket> _sockets;
        private readonly Socket _serverSocket;
        private static MessageEchoer _messageEchoer;

        /// <summary>
        /// Initialises the Connection Manager with a specified port and maximum amount of connections.
        /// Once initialised the Connection Manager starts listening on the port.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="maxConnections"></param>
        public ConnectionManager(int port = 64646, int maxConnections = 16)
        {
            _sockets = new List<Socket>();
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            Console.WriteLine("ConnectionManager: Bound to port " + port);
            _serverSocket.Listen(maxConnections);
            Console.WriteLine("ConnectionManager: Listening with a maximum of " + maxConnections + " connections");
            _sockets.Add(_serverSocket);

            _messageEchoer = new MessageEchoer();
            _messageEchoer.Start();
        }

        /// <summary>
        /// Starts accepting and handling connections, passing each
        /// connection to a MessageEchoer where appropriate.
        /// </summary>
        protected override void Run()
        {
            while (true)
            {
                // Copy _sockets into socketsCopy on every loop
                var socketsCopy = new List<Socket>(_sockets);
                // Blocks until there is new data on one of the sockets
                Socket.Select(socketsCopy, null, null, -1);

                if (socketsCopy[0] == _serverSocket)
                {
                    // New connection incoming
                    var client = socketsCopy[0].Accept();
                    _sockets.Add(client);
                    // Remove this socket from the copy because the next loop isn't concerned with it
                    socketsCopy.Remove(_serverSocket);
                    Console.WriteLine("ConnectionManager: Accepted connection from " +
                                      (IPEndPoint) client.RemoteEndPoint);
                }

                foreach (var client in socketsCopy)
                {
                    if (client.Available != 0)
                    {
                        // There is new data available on the socket
                        var connection = new Connection(client);
                        // Read the data from the socket
                        connection.Message = connection.Reader.ReadLine();
                        // Place the data onto the channel
                        //Console.WriteLine(connection.Message);
                        _messageEchoer.Channel.Put(connection);
                    }
                    else
                    {
                        Console.WriteLine("ConnectionManager: Client "
                                          + (IPEndPoint) client.RemoteEndPoint +
                                          " disconnected.");
                        client.Close();
                        _sockets.Remove(client);
                    }
                }
            }
        }
    }
}