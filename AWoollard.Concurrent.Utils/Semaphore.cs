using System.Threading;

namespace AWoollard.Concurrent.Utils
{
    /// <summary>
    /// A semaphore has an amount of tokens which determines access to a resource.
    /// Threads can acquire tokens from the semaphore when they use a resource,
    /// and once the semaphore has run out of tokens it will force any further
    /// requesting threads to block until tokens are released back into the semaphore
    /// by a thread (usually once another thread has finished using a resource).
    /// </summary>
    public class Semaphore
    {
        private uint _count;
        private readonly object _semaphoreLock;

        /// <summary>
        /// Initialises the semaphore with an amount of n tokens.
        /// </summary>
        /// <param name="n">The amount of tokens to initialise the semaphore with</param>
        public Semaphore(uint n = 0)
        {
            _semaphoreLock = new object();
            _count = n;
        }

        /// <summary>
        /// Releases an amount of n tokens, waking up threads waiting for tokens.
        /// </summary>
        /// <param name="n">The amount of tokens</param>
        public virtual void Release(uint n = 1)
        {
            lock (_semaphoreLock)
            {
                _count += n;
                if (_count > 0)
                {
                    Monitor.Pulse(_semaphoreLock);
                }
            }
        }

        /// <summary>
        /// Requests a token from the semaphore.
        /// Blocks if there are no tokens available.
        /// </summary>
        public virtual void Acquire()
        {
            lock (_semaphoreLock)
            {
                while(_count == 0)
                    Monitor.Wait(_semaphoreLock);
                _count--;
            }
        }
    }
}
