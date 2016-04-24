using System;
using System.Threading;
using AWoollard.Concurrent.Utils;

namespace _3._1___ChannelActiveObject
{
    /// <summary>
    /// Tests for the Channel class.
    /// </summary>
    public class ChannelActiveObjectTest
    {
        /// <summary>
        /// Takes integers off the channel and increments a counter with them.
        /// </summary>
        private class IntegerIncrementer : ChannelActiveObject<int>
        {
            public int Result { get; private set; }

            public IntegerIncrementer() : base()
            {
                Result = 0;
            }

            protected override void Process(int item)
            {
                Result += item;
            }
        }

        private readonly IntegerIncrementer _integerIncrementer;

        /// <summary>
        /// Initialises the IntegerIncrementer used for the tests.
        /// </summary>
        public ChannelActiveObjectTest()
        {
            _integerIncrementer = new IntegerIncrementer();
        }

        /// <summary>
        /// Run the ChannelActiveObjectTest tests
        /// </summary>
        public void Run()
        {
            Console.WriteLine(Test1()
                ? "All ChannelActiveObjectTest tests passed"
                : "One or more ChannelActiveObjectTest tests failed");
        }

        
        /// <summary>
        /// Performs one hundred addition operations 
        /// </summary>
        private bool Test1()
        {
            _integerIncrementer.Start();
            for (var i = 0; i < 100; i++)   
            {
                _integerIncrementer.Channel.Put(i);
            }

            // Wait for the data to go into the channel and be processed by _integerIncrementer
            Thread.Sleep(500);
            
            return _integerIncrementer.Result == 4950;
        }
    }
}
