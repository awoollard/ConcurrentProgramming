using System;
using System.Threading;
using AWoollard.Concurrent.Utils;

namespace _2._2___Rendezvous
{
    /// <summary>
    /// Tests for the Rendezvous class.
    /// </summary>
    public class RendezvousTest
    {
        private Thread _aThread;
        private Thread _bThread;
        private readonly Rendezvous _rendezvous;

        /// <summary>
        /// Initialises the Threads and Rendezvous used for the tests.
        /// </summary>
        public RendezvousTest()
        {
            _rendezvous = new Rendezvous();
        }
        
        private void DoSomething()
        {
            _rendezvous.Arrive(Thread.CurrentThread);
        }

        /// <summary>
        /// Run the Rendezvous tests
        /// </summary>
        public void Run()
        {
            if (Test1() && Test2() && Test3())
            {
                Console.WriteLine("All Rendezvous tests passed");
            }
        }

        /// <summary>
        /// Tests the case where one thread arrives and the other does not arrive, so neither continue
        /// </summary>
        private bool Test1(int timeoutSeconds = 3)
        {
            _aThread = new Thread(DoSomething);
            _bThread = new Thread(DoSomething);
            _aThread.Name = "1";
            _bThread.Name = "2";

            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            var timeStarted = DateTime.UtcNow;

            _aThread.Start();

            // Check if thread A is blocking
            if (_aThread.Join(timeout - (DateTime.UtcNow - timeStarted)))
                return false;

            // Thread A is blocking as it should
            _bThread.Abort();
            return true;
        }

        /// <summary>
        /// Tests the case where both threads arrive so both continue
        /// </summary>
        private bool Test2(int timeoutSeconds = 3)
        {
            _aThread = new Thread(DoSomething);
            _bThread = new Thread(DoSomething);
            _aThread.Name = "1";
            _bThread.Name = "2";

            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            var timeStarted = DateTime.UtcNow;

            _aThread.Start();

            if (!_aThread.Join(timeout - (DateTime.UtcNow - timeStarted)))
            {
                // Thread A is blocking as it should
                timeStarted = DateTime.UtcNow;
                _bThread.Start();

                if (_bThread.Join(timeout - (DateTime.UtcNow - timeStarted)))
                {
                    // Thread B has passed the rendezvous point and finished
                    return true;
                }
                else
                {
                    // Thread B is blocking, kill it
                    _bThread.Abort();
                }
            }
            
            return false;
        }

        /// <summary>
        /// Tests if the pattern of tests can be repeated
        /// </summary>
        /// <returns></returns>
        private bool Test3()
        {
            for (var i = 0; i < 10; i++)
            {
                if (!Test1(1) || !Test2(1))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
