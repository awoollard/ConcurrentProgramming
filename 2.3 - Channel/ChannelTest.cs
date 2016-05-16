using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AWoollard.Concurrent.Utils;

namespace _2._3___Channel
{
    /// <summary>
    /// Tests for the Channel class.
    /// </summary>
    public class ChannelTest
    {
        private Thread _aThread;
        private Thread _bThread;
        private readonly Channel<int> _channel;

        // Used for the first test
        private List<int> _randomNumbers;
        private List<int> _takenNumbers;

        /// <summary>
        /// Initialises the Channel used for the tests.
        /// </summary>
        public ChannelTest()
        {
            _channel = new Channel<int>();
        }

        /// <summary>
        /// Takes a list of elements and returns a pretty string summarising the list collection (comma delimited and between braces)
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="list">The list</param>
        /// <returns>A pretty string summarising the list collection</returns>
        private static string PrettyArrayToString<T>(List<T> list)
        {
            return "{" + list.Skip(1).Aggregate(list[0].ToString(), (s, i) => s + ", " + i.ToString()) + "}";
        }

        private void PutRandomNumbersIntoAChannel(int amount)
        {
            var random = new Random();
            _randomNumbers = new List<int>();

            for (var i = 0; i < amount; i++)
            {
                _randomNumbers.Add(random.Next(0, 100));
                _channel.Put(_randomNumbers[i]);
            }
            
            Console.WriteLine(Thread.CurrentThread.Name + " put " + PrettyArrayToString(_randomNumbers) + " into channel");
        }

        private void TakeNumbersFromChannel(int amount)
        {
            _takenNumbers = new List<int>();
            for (var i = 0; i < amount; i++)
            {
                _takenNumbers.Add(_channel.Take());
            }

            Console.WriteLine(Thread.CurrentThread.Name + " took " + PrettyArrayToString(_takenNumbers) + " from channel");
        }

        /// <summary>
        /// Run the Channel tests
        /// </summary>
        public void Run()
        {
            if (Test1() && Test2())
            {
                Console.WriteLine("All Channel tests passed");
            }
            else
            {
                Console.WriteLine("One or more Channel tests failed");
            }
        }

        /// <summary>
        /// Tests the case where one thread puts something onto the channel and the other takes from the channel
        /// </summary>
        private bool Test1()
        {
            _aThread = new Thread(() => PutRandomNumbersIntoAChannel(10));
            _bThread = new Thread(() => TakeNumbersFromChannel(10));

            _aThread.Start();
            _bThread.Start();

            // Wait until both threads finish execution
            _aThread.Join();
            _bThread.Join();

            return _takenNumbers.SequenceEqual(_randomNumbers);
        }

        /// <summary>
        /// Tests the case where a thread waits on an empty channel
        /// </summary>
        private bool Test2()
        {
            _aThread = new Thread(() => { });
            _bThread = new Thread(() => TakeNumbersFromChannel(1));

            var timeout = TimeSpan.FromSeconds(3);
            var timeStarted = DateTime.UtcNow;

            _aThread.Start();
            _bThread.Start();

            if (!_bThread.Join(timeout - (DateTime.UtcNow - timeStarted)))
            {
                // Thread B is blocking like it should
                _bThread.Abort();
                return true;
            }

            // Thread B didn't block
            return false;
        }
    }
}
