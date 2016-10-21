using System;
using AWoollard.Concurrent.Utils;

namespace _8._1___Connection_Manager
{
    /// <summary>
    /// Echoes a message from a connection object to the console.
    /// </summary>
    public class MessageEchoer : ChannelActiveObject<Connection>
    {
        /// <summary>
        /// Writes the connections message from the channel to the console.
        /// </summary>
        /// <param name="item"></param>
        protected override void Process(Connection item)
        {
            Console.WriteLine(item.Message);
        }
    }
}
