using System;

namespace AWoollard.Concurrent.Utils
{
    /// <summary>
    /// A mutex which can be used by multiple threads to access a shared resource
    /// </summary>
    public class Mutex : Semaphore
    {
        /// <summary>
        /// Initialises the mutex
        /// </summary>
        public Mutex()
        {
            base.Release();
        }

        /// <summary>
        /// Releases the Mutex
        /// </summary>
        /// <param name="n">Amount of tokens (only 1 is supported)</param>
        public override void Release(uint n = 1)
        {
            if (n > 1)
            {
                throw new Exception("Mutex: Cannot release more than one token");
            }
            base.Release(n);
        }
    }
}
