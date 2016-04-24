using System;
using System.Threading;
using Mutex = AWoollard.Concurrent.Utils.Mutex;

namespace _3._3___Mutex
{
    /// <summary>
    /// Tests for the Mutex class.
    /// </summary>
    public class MutexTest
    {
        private readonly Mutex _mutex;

        /// <summary>
        /// Initialises the mutex used for the tests.
        /// </summary>
        public MutexTest()
        {
            _mutex = new Mutex();
        }

        /// <summary>
        /// Runs the tests.
        /// </summary>
        public void Run()
        {
            Console.WriteLine((Test1() && Test2() && Test3())
                ? "All MutexTest tests passed"
                : "One or more MutexTest tests failed");
        }

        /// <summary>
        /// Tries to acquire and release a mutex.
        /// </summary>
        /// <returns></returns>
        private bool Test1()
        {
            _mutex.Acquire();
            _mutex.Release();

            return true;
        }

        /// <summary>
        /// Tries to acquire a mutex once it's already acquired by another thread.
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        private bool Test2(int timeoutSeconds = 3)
        {
            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            var timeStarted = DateTime.UtcNow;

            var thread1 = new Thread(_mutex.Acquire);
            var thread2 = new Thread(_mutex.Acquire);
            thread1.Start();
            thread2.Start();

            if (!thread2.Join(timeout - (DateTime.UtcNow - timeStarted)))
            {
                // Thread is blocking as it should because the Mutex is already acquired
                return true;
            }

            // Thread didn't block
            return false;
        }

        /// <summary>
        /// Tests if the Mutex throws an exception if Mutex.Release is called with > 1 token
        /// </summary>
        /// <returns></returns>
        private bool Test3()
        {
            try
            {
                _mutex.Release(2);
            }
            catch (Exception)
            {
                return true;
            }
            return false;
        }

    }
}
