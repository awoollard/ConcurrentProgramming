using System.Threading;

namespace AWoollard.Concurrent.Utils
{
    /// <summary>
    /// A rendezvous is an object which can be used by two concurrent threads, where both need to check that each of them
    /// has reached a particular point in execution before proceeding. Each thread should call the <see cref="Arrive"/>
    /// method to signify that it has reached the desired point in execution,  and will block until the other thread
    /// reaches the desired point as well. Once both have arrived, both threads will unblock and continue execution.
    /// </summary>
    public class Rendezvous
    {
        private readonly Semaphore _aArrived;
        private readonly Semaphore _bArrived;
        private Thread _aThread;

        /// <summary>
        /// Initializes the Rendezvous to a state where neither thread has arrived. 
        /// </summary>
        public Rendezvous()
        {
            _aArrived = new Semaphore(0);
            _bArrived = new Semaphore(0);
            _aThread = null;
        }

        /// <summary>
        /// Arrive at the Rendezvous. Use this method where two methods should wait for each other in their respective execution flows.
        /// The first thread to call this method will block. The second thread to call this method will unblock and continue execution
        /// of both threads.
        /// </summary>
        /// <param name="currentThread">One of the threads participating in the rendezvous</param>
        public void Arrive(Thread currentThread)
        {
            if (_aThread == null)
            {
                _aThread = currentThread;
                _bArrived.Release();
                _aArrived.Acquire();
            }
            else if (_aThread != currentThread)
            {
                _aArrived.Release();
                _bArrived.Acquire();
            }
        }
    }
}
