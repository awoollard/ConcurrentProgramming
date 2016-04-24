using System;
using System.Threading;

namespace HelloApp
{
    class HelloWorld
    {
        public static void Main(string[] args)
        {
            Thread[] threads = new Thread[5];
            for (int threadNum = 0; threadNum < 5; threadNum++)
            {
                Hello hello = new Hello(threadNum + 1);
                ThreadStart threadDelegate = new ThreadStart(hello.SayHello);
                threads[threadNum] = new Thread(threadDelegate);
                threads[threadNum].Start();
            }
        }
    }

    public class Hello
    {
        private int threadNum;

        public Hello(int threadNum)
        {
            this.threadNum = threadNum;
        }

        public void SayHello()
        {
            while (true)
            {
                Console.WriteLine("Hello World - from Thread " + threadNum);
            }
        }
    }
}

