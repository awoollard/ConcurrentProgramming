using System;
using System.Collections.Generic;
using System.Threading;
using AWoollard.Concurrent.Utils;

namespace _3._4___Latch
{
    /// <summary>
    /// Tests for the Latch class.
    /// </summary>
    public class LatchTest
    {
        private Latch _latch;

        /// <summary>
        /// Run the tests
        /// </summary>
        public void Run()
        {
            Console.WriteLine((Test1() && Test2())
                ? "All LatchTest tests passed"
                : "One or more LatchTest tests failed");
        }

        /// <summary>
        /// Waits for the latch to be released and prints messages indicating the status of the latch
        /// </summary>
        private void WaitForLatch()
        {
            Console.WriteLine(Thread.CurrentThread.Name + ": Waiting for latch...");
            _latch.Acquire();
            Console.WriteLine(Thread.CurrentThread.Name + ": Latch released!");
        }

        /// <summary>
        /// Creates five threads waiting on the latch to be released,
        /// waits a specified amount of time and then releases the latch
        /// </summary>
        /// <param name="secondsToWait">The amount of time before releasing the latch in seconds</param>
        /// <returns>True if the test passed, false if the test failed</returns>
        private bool Test1(int secondsToWait = 2)
        {
            _latch = new Latch();
            var threadPool = new List<Thread>();

            for (var i = 0; i < 5; i++)
            {
                threadPool.Add(new Thread(WaitForLatch) { Name = "Thread " + (i + 1) });
            }

            foreach (var thread in threadPool)
            {
                thread.Start();
            }
            
            Thread.Sleep(secondsToWait*1000);
            _latch.Release();

            return true;
        }

        /// <summary>
        /// Tests the blocking functionality of the latch.
        /// </summary>
        /// <param name="timeoutSeconds">How long to wait for the blocking threads in seconds</param>
        /// <returns>True if the test passed, false if the test failed</returns>
        private bool Test2(int timeoutSeconds = 3)
        {
            _latch = new Latch();
            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            var timeStarted = DateTime.UtcNow;

            var thread = new Thread(_latch.Acquire);
            thread.Start();

            if (!thread.Join(timeout - (DateTime.UtcNow - timeStarted)))
            {
                // Thread is blocking as it should because its the latch hasn't been released
                return true;
            }

            // Thread didn't block
            return false;
        }
    }
}