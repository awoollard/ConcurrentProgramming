using System.Threading;

namespace AWoollard.Concurrent.Utils
{
    /// <summary>
    /// Allows two threads to meet at a given point. This ensures that both threads are ready before either proceeds.
    /// </summary>
    public class Rendezvous
    {
        private readonly Semaphore _aArrived;
        private readonly Semaphore _bArrived;

        /// <summary>
        /// Initializes the Rendezvous
        /// </summary>
        public Rendezvous()
        {
            _aArrived = new Semaphore();
            _bArrived = new Semaphore();
        }

        /// <summary>
        /// Arrive at the Rendezvous. Use this method where two methods should wait for each other in their respective execution flows.
        /// </summary>
        /// <param name="currentThread">One of the threads participating in the rendezvous</param>
        public void Arrive(Thread currentThread)
        {
            switch (currentThread.Name)
            {
                case "1":
                    _bArrived.Release();
                    _aArrived.Acquire();
                    break;
                case "2":
                    _aArrived.Release();
                    _bArrived.Acquire();
                    break;
            }
        }
    }
}
