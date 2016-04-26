using System;
using System.Collections.Generic;
using System.Threading;
using Barrier = AWoollard.Concurrent.Utils.Barrier;

namespace _6._1___Barrier
{
    public class BarrierTest
    {
        private Barrier _barrier;

        /// <summary>
        /// Run the Channel tests
        /// </summary>
        public void Run()
        {
            Console.WriteLine((Test1() && Test2() && Test3())
                ? "All Barrier tests passed"
                : "One or more Barrier tests failed");
        }

        private void ArriveAtBarrier()
        {
            _barrier.Arrive(Thread.CurrentThread);
        }

        /// <summary>
        /// Tests whether or not all the threads block while the number that have
        /// arrived is less than the number required by the barrier
        /// </summary>
        /// <param name="timeoutSeconds">How long to wait for the blocking threads in seconds</param>
        /// <returns>True if the test passed, false if the test failed</returns>
        private bool Test1(double timeoutSeconds = 1)
        {
            // Set the amount of threads required to proceed to 3
            _barrier = new Barrier(3);

            var threadPool = new List<Thread>
            {
                new Thread(ArriveAtBarrier),
                new Thread(ArriveAtBarrier)
            };

            var timeout = TimeSpan.FromSeconds(timeoutSeconds);

            foreach (var thread in threadPool)
            {
                thread.Start();
            }

            if (!threadPool[0].Join(timeout))
            {
                if (!threadPool[1].Join(timeout))
                {
                    // Threads are blocking as they should because the
                    // amount of threads arrived is less than the amount required
                    return true;
                }
            }

            // One of the threads didn't block
            return false;
        }

        /// <summary>
        /// Tests the case where the nth thread arrives and all threads proceed
        /// </summary>
        /// <param name="timeoutSeconds">How long to wait for the blocking threads in seconds</param>
        /// <returns>True if the test passed, false if the test failed</returns>
        private bool Test2(double timeoutSeconds = 1)
        {
            // Set the amount of threads required to proceed to 3
            _barrier = new Barrier(3);

            var threadPool = new List<Thread>
            {
                new Thread(ArriveAtBarrier),
                new Thread(ArriveAtBarrier),
                new Thread(ArriveAtBarrier)
            };

            var timeout = TimeSpan.FromSeconds(timeoutSeconds);

            threadPool[0].Start();
            threadPool[1].Start();

            if (!threadPool[0].Join(timeout))
            {
                if (!threadPool[1].Join(timeout))
                {
                    // Threads are blocking as they should because the
                    // amount of threads arrived is less than the amount required
                    threadPool[2].Start();
                    if (threadPool[2].Join(timeout))
                    {
                        // Thread correctly didn't block because the amount of threads
                        // to arrive reached the required amount
                        return true;
                    }
                }
            }

            // Either the first or second thread didn't block, or the last thread did block.
            return false;
        }

        private void ArriveAtBarrierTwice()
        {
            _barrier.Arrive(Thread.CurrentThread);
            _barrier.Arrive(Thread.CurrentThread);
        }

        /// <summary>
        /// Tests whether or not the barrier can be reused, without it being possible for a thread to
        /// arrive twice for one group
        /// </summary>
        /// <param name="timeoutSeconds">How long to wait for the blocking threads in seconds</param>
        /// <returns>True if the test passed, false if the test failed</returns>
        private bool Test3(double timeoutSeconds = 1)
        {
            // Set the amount of threads required to proceed to 3
            _barrier = new Barrier(3);

            var threadPool = new List<Thread>
            {
                new Thread(ArriveAtBarrier),
                new Thread(ArriveAtBarrierTwice)
            };

            var timeout = TimeSpan.FromSeconds(timeoutSeconds);

            threadPool[0].Start();
            threadPool[1].Start();

            if (!threadPool[0].Join(timeout))
            {
                if (!threadPool[1].Join(timeout))
                {
                    // Threads are blocking as they should because the
                    // amount of threads arrived is less than the amount required
                    // and trying to arrive at the barrier twice had no effect
                    return true;
                }
            }

            // One of the threads didnt block
            return false;
        }
    }
}