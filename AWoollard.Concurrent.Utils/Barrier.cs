using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;

namespace AWoollard.Concurrent.Utils
{
    /// <summary>
    /// A barrier which allows threads to wait for each other to reach
    /// a certain point of execution before proceeding.
    /// </summary>
    public class Barrier
    {
        private readonly Dictionary<Thread, bool> _threads;
        private readonly Semaphore _semaphore;
        private readonly uint _threadsRequired;
        private uint _amountArrived;

        /// <summary>
        /// Initialises the Barrier.
        /// </summary>
        /// <param name="n">Amount of threads required to complete the barrier</param>
        public Barrier(uint n)
        {
            _threads = new Dictionary<Thread, bool>();
            _semaphore = new Semaphore();
            _threadsRequired = n;
            _amountArrived = 0;
        }

        /// <summary>
        /// Blocks threads until the amount of threads which have arrived equals the amount required to proceed.
        /// </summary>
        /// <returns>Whether or not this thread is the one to cause the other threads to proceed</returns>
        public bool Arrive(Thread thread)
        {
            bool outValue;
            if (_threads.TryGetValue(thread, out outValue))
                // A thread has tried arriving twice - return false
                return false;

            _threads.Add(thread, true);
            _amountArrived++;
            if (_amountArrived < _threadsRequired)
            {
                // Blocks the thread until other threads increment the
                // amountArrived variable to equal the threadsRequired value,
                // eventually releasing the semaphore.
                _semaphore.Acquire();
                return false;
            }

            _semaphore.Release();
            return true;
        }
    }
}
