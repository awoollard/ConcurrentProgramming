using System.Collections.Generic;

namespace AWoollard.Concurrent.Utils
{
    /// <summary>
    /// A channel which can be used for interthread communication. Threads can <see cref="Put"/> items
    /// into the channel, and other threads can <see cref="Take"/> the items from the channel in the order they
    /// were placed (FIFO). This allows threads to pass data between each other.
    /// </summary>
    /// <typeparam name="T">The type of items stored in the channel</typeparam>
    public class Channel<T>
    {
        private readonly Queue<T> _queue;
        private readonly Semaphore _semaphore;

        /// <summary>
        /// Initialises the Channel with no items.
        /// </summary>
        public Channel()
        {
            _queue = new Queue<T>();
            _semaphore = new Semaphore();
        }

        /// <summary>
        /// Puts an item into the channel.
        /// </summary>
        /// <param name="item">The item to go into the channel</param>
        public virtual void Put(T item)
        {
            lock (this)
            {
                _queue.Enqueue(item);
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Takes an item from the channel
        /// </summary>
        /// <returns>The item taken from the channel</returns>
        public virtual T Take()
        {
            lock (this)
            {
                _semaphore.Acquire();
                return _queue.Dequeue();
            }
        }
    }
}