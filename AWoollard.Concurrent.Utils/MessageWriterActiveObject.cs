using System;

namespace AWoollard.Concurrent.Utils
{
    /// <summary>
    /// An ActiveObject which contains a message. Calling its Run method will print the message.
    /// </summary>
    public class MessageWriterActiveObject : ActiveObject
    {
        private readonly string _message;

        /// <summary>
        /// Initialises an MessageWriterActiveObject with a name and a message.
        /// </summary>
        /// <param name="name">The name of the MessageWriterActiveObject</param>
        /// <param name="message">The message to be printed to console</param>
        public MessageWriterActiveObject(string name, string message) : base(name)
        {
            _message = message;
        }

        /// <summary>
        /// Prints the message to the console
        /// </summary>
        protected override void Run()
        {
            Console.WriteLine(_message);
        }
    }
}
