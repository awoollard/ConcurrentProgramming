namespace AWoollard.Concurrent.Utils
{
    /// <summary>
    /// A channel active object is a thread which listens to a channel. Inherit from this class and implement
    /// the <see cref="Process"/> method to specify the desired behaviour for each item that is put in the channel.
    /// </summary>
    /// <typeparam name="T">The type of objects to store in the channel and process</typeparam>
    public abstract class ChannelActiveObject<T> : ActiveObject
    {
        /// <summary>
        /// The Channel
        /// </summary>
        public Channel<T> Channel { get; }

        /// <summary>
        /// Initialises the ChannelActiveObject with a channel (or creates a new channel if not specified)
        /// and a thread name (defaults to "Channel")
        /// </summary>
        /// <param name="channel">The existing Channel object</param>
        /// <param name="threadName">The thread name (defaults to "Channel")</param>
        protected ChannelActiveObject(Channel<T> channel = null, string threadName = "Channel") : base(threadName)
        {
            Channel = channel ?? new Channel<T>();
        }

        /// <summary>
        /// Continously take items from the channel (blocking if there aren't any available)
        /// and passes them into the <see cref="Process"/> method.
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
        /// This method is essentially a call-back which can perform some behaviour for each item that is put in the channel.
        /// </summary>
        /// <param name="item">An item from the channel</param>
        protected abstract void Process(T item);
    }
}