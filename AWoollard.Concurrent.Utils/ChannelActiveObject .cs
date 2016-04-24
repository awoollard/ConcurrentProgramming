namespace AWoollard.Concurrent.Utils
{
    public abstract class ChannelActiveObject<T> : ActiveObject
    {
        /// <summary>
        /// The Channel
        /// </summary>
        public Channel<T> Channel { get; }

        /// <summary>
        /// Initialise the ChannelActiveObject with an existing Channel object and a specified thread name (defaults to "Channel")
        /// </summary>
        /// <param name="channel">The existing Channel object</param>
        /// <param name="threadName">The thread name (defaults to "Channel")</param>
        protected ChannelActiveObject(Channel<T> channel = null, string threadName = "Channel") : base(threadName)
        {
            Channel = channel ?? new Channel<T>();
        }

        /// <summary>
        /// Takes an item from the Channel and gives it to the Process method
        /// </summary>
        protected override void Run()
        {
            while (true)
            {
                Process(Channel.Take());
            }
        }

        /// <summary>
        /// An abstract method intended to receive an item from the channel and perform an operation with it.
        /// </summary>
        /// <param name="item">An item from the channel</param>
        protected abstract void Process(T item);
    }
}