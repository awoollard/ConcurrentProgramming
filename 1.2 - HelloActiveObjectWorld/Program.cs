using AWoollard.Concurrent.Utils;

namespace HelloWorldMessageWriter
{
    /// <summary>
    /// Prints two messages to the console.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point for the HelloWorldMessageWriter application. Creates and starts two MessageWriterActiveObjects.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var messageWriter1 = new MessageWriterActiveObject("Message Writer 1", "Hello World");
            var messageWriter2 = new MessageWriterActiveObject("Message Writer 2", "Goodbye World");
            messageWriter1.Start();
            messageWriter2.Start();
        }
    }
}
