using System.IO;
using System.Net.Sockets;

namespace _8._1___Connection_Manager
{
    public class Connection
    {
        public StreamReader Reader { get; set; }
        public StreamWriter Writer { get; set; }
        public Socket Socket { get; set; }
        public string Message { get; set; }

        public Connection(Socket client)
        {
            Socket = client;
            if(client.Connected) {
                var networkStream = new NetworkStream(client);
                Reader = new StreamReader(networkStream);
                Writer = new StreamWriter(networkStream);
            }
        }
    }
}
