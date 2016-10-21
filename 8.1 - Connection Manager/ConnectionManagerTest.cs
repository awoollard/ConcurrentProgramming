using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace _8._1___Connection_Manager
{
    public class ConnectionManagerTest
    {
        /// <summary>
        /// Initialises the ConnectionManager used for the tests.
        /// </summary>
        public ConnectionManagerTest()
        {
            var connectionManager = new ConnectionManager();
            connectionManager.Start();
        }

        /// <summary>
        /// Runs the tests.
        /// </summary>
        public void Run()
        {
            Console.WriteLine(TestConnectingOneClient() && TestConnectingMultipleClients()
                ? "All ConnectionManager tests passed"
                : "One or more ConnectionManager tests failed");
        }

        /// <summary>
        /// Tests that a TCP client can connect to our connection manager.
        /// </summary>
        /// <returns></returns>
        private static bool TestConnectingOneClient()
        {
            Console.WriteLine("=== Test 1 ===");
            Console.WriteLine("Tests that a TCP client can connect to our connection manager.");

            const string ip = "127.0.0.1";
            const int port = 64646;

            var ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Attempting to connect to " + ip + ":" + port + ".");

            try
            {
                socket.Connect(ipEndPoint);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Failure: Connection failed with SocketException: " + e);
                return false;
            }

            Thread.Sleep(1000);
            Console.WriteLine("Success: Connection succeeded");

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            return true;
        }

        /// <summary>
        /// Tests that multiple TCP clients can connect to our connection manager.
        /// </summary>
        /// <returns></returns>
        private static bool TestConnectingMultipleClients()
        {
            Console.WriteLine("=== Test 2 ===");
            Console.WriteLine("Tests that multiple TCP clients can connect to our connection manager.");

            const string ip = "127.0.0.1";
            const int port = 64646;

            var ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var socket3 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Attempting to connect to " + ip + ":" + port + ".");

            try
            {
                socket1.Connect(ipEndPoint);
                socket2.Connect(ipEndPoint);
                socket3.Connect(ipEndPoint);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Failure: Connection failed with SocketException: " + e);
                return false;
            }

            socket1.Shutdown(SocketShutdown.Both);
            socket1.Close();

            socket2.Shutdown(SocketShutdown.Both);
            socket2.Close();

            socket3.Shutdown(SocketShutdown.Both);
            socket3.Close();

            Thread.Sleep(1000);
            Console.WriteLine("Success: All sockets connected");
            return true;
        }
    }
}
