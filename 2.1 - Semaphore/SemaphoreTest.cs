using System;
using System.Collections.Generic;
using System.Threading;
using Semaphore = AWoollard.Concurrent.Utils.Semaphore;

namespace _2._1___Semaphore
{
    /// <summary>
    /// Tests for the Semaphore class.
    /// </summary>
    public class SemaphoreTest
    {
        private static List<Thread> _threads;
        private static object _lockObject;
        private static Semaphore _semaphore;

        /// <summary>
        /// Runs the semaphore tests.
        /// </summary>
        public static void Run()
        {
            _lockObject = new object();

            //Test1();
            //Test2();
            Test3();
        }

        /// <summary>
        /// Acquires a token from the semaphore and performs an operation lasting one second.
        /// </summary>
        private static void OneSecondOperation()
        {
            while (true)
            {
                _semaphore.Acquire();
                Console.WriteLine(Thread.CurrentThread.Name + ": Starting 1 second operation...");
                Thread.Sleep(1000);
                Console.WriteLine(Thread.CurrentThread.Name + ": Finished 1 second operation.");
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Tests that threads block in acquire until a token arrives.
        /// Also tests the case where multiple threads are waiting to acquire, 
        /// and one token is released, only one of the waiting threads proceeds
        /// </summary>
        private static void Test1()
        {
            _threads = new List<Thread>();
            _semaphore = new Semaphore(1);

            for (var i = 0; i < 5; i++)
            {
                _threads.Add(new Thread(OneSecondOperation) {Name = "Thread " + (i + 1)});
                _threads[i].Start();
            }
        }

        private static int _tokenCount = 0;
        /// <summary>
        /// Releases a maximum of 4 tokens to the semaphore.
        /// Fifth thread to call this method will block at Acquire
        /// </summary>
        private static void AttemptToFinish()
        {
            lock (_lockObject)
            {
                _tokenCount++;

                // Release only 4 tokens
                if (_tokenCount == 4)
                    _semaphore.Release(4);
            }

            Console.WriteLine(Thread.CurrentThread.Name + " is waiting for a token");
            _semaphore.Acquire();

            Console.WriteLine(Thread.CurrentThread.Name + " has finished");
        }

        /// <summary>
        /// Tests the case where multiple threads are waiting to acquire,
        /// and multiple tokens are released, the number of threads that
        /// proceed equal the number of tokens released.
        /// </summary>
        private static void Test2()
        {
            _semaphore = new Semaphore(0);
            _threads = new List<Thread>();

            // Only 4 of the 5 threads will finish
            for (var i = 0; i < 5; i++)
            {
                _threads.Add(new Thread(AttemptToFinish) {Name = "Thread " + (i + 1)});
                _threads[i].Start();
            }
        }

        private static void TryAcquireAndWrite()
        {
            for (var i = 1; i <= 10; i++)
            {
                // Thread will wait at the acquire call here until a token is released
                _semaphore.Acquire();
                Console.WriteLine(i + " tokens have been released");
            }
        }

        /// <summary>
        /// Tests the case where a token is released to a Semaphore with no waiting threads, so the next thread to Acquire will proceed without waiting.
        /// </summary>
        private static void Test3()
        {
            _semaphore = new Semaphore(0);
            new Thread(TryAcquireAndWrite).Start();

            Console.WriteLine("Pressing enter releases Semaphore");
            while (true)
            {
                Console.ReadLine();
                _semaphore.Release();
            }
        }
    }
}