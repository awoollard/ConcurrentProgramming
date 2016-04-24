using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AWoollard.Concurrent.Utils;

namespace _3._2___BoundChannel
{
    /// <summary>
    /// Tests for the BoundChannel class.
    /// </summary>
    public class BoundChannelTest
    {
        private readonly BoundChannel<int> _boundChannel;
        public BoundChannelTest()
        {
            _boundChannel = new BoundChannel<int>(5);
        }

        public void Run()
        {
            Console.WriteLine((Test1() && Test2())
                ? "All BoundChannelTest tests passed"
                : "One or more BoundChannelTest tests failed");
        }

        /// <summary>
        /// Adds five items to the BoundChannel, then takes five items from it and validates the values
        /// </summary>
        /// <returns>True if the test passes, false if not</returns>
        private bool Test1()
        {
            var originalList = new[] { 1, 2, 3, 4, 5 };
            var listFromChannel = new List<int>();

            for (var i = 0; i < originalList.Length; i++)
            {
                _boundChannel.Put(originalList[i]);
            }

            for (var i = 0; i < originalList.Length; i++)
            {
                listFromChannel.Add(_boundChannel.Take());
            }

            return listFromChannel.SequenceEqual(originalList);
        }

        /// <summary>
        /// Tries to add an extra item past the maximum limit of the BoundChannel.
        /// </summary>
        /// <param name="timeoutSeconds">How long to wait for the blocking thread</param>
        /// <returns>True if the test passes, false if not</returns>
        private bool Test2(int timeoutSeconds = 3)
        {
            var originalList = new[] { 1, 2, 3, 4, 5, 6 };
            var listFromChannel = new List<int>();
            var i = 0;

            while (i < originalList.Length-1)
            {
                _boundChannel.Put(originalList[i]);
                i++;
            }

            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            var timeStarted = DateTime.UtcNow;

            var thread = new Thread(() => TryPutOverMaximum(originalList[i]));
            thread.Start();

            if (!thread.Join(timeout - (DateTime.UtcNow - timeStarted)))
            {
                // Thread is blocking as it should
                return true;
            }

            // Thread didn't block when attempting to put an item over the maximum limit
            return false;
        }

        private void TryPutOverMaximum(int item)
        {
            _boundChannel.Put(item);
        }
    }
}