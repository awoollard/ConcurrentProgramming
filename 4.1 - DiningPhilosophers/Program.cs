using System;
using System.Collections.Generic;
using System.Threading;
using Mutex = AWoollard.Concurrent.Utils.Mutex;

namespace DiningPhilosophers
{
    public class DiningPhilosophers
    {
        private readonly List<Mutex> _chopstick;
        private readonly List<Philosopher> _philosopher;
        public DiningPhilosophers()
        {
            _chopstick = new List<Mutex>();
            _philosopher = new List<Philosopher>();

            for (var i = 0; i < 5; i++)
            {
                _chopstick.Add(new Mutex());
                _philosopher.Add(new Philosopher());
            }
        }

        public void Run()
        {
            var threadList = new List<Thread>
            {
                new Thread(() => _philosopher[0].Dine(_chopstick[0], _chopstick[1])),
                new Thread(() => _philosopher[1].Dine(_chopstick[1], _chopstick[2])),
                new Thread(() => _philosopher[2].Dine(_chopstick[2], _chopstick[3])),
                new Thread(() => _philosopher[3].Dine(_chopstick[3], _chopstick[4])),
                new Thread(() => _philosopher[4].Dine(_chopstick[4], _chopstick[0]))
            };

            for (var i = 0; i < threadList.Count; i++)
            {
                var thread = threadList[i];
                thread.Name = "Philosopher " + (i + 1);
                thread.Start();
            }
        }
    }

    public class Philosopher
    {
        private static void Eat(Mutex leftChopstick, Mutex rightChopstick)
        {
            Console.WriteLine(Thread.CurrentThread.Name + " is picking up his left chopstick");
            leftChopstick.Acquire();
            Console.WriteLine(Thread.CurrentThread.Name + " has picked up his left chopstick");
            Console.WriteLine(Thread.CurrentThread.Name + " is picking up his right chopstick");
            rightChopstick.Acquire();
            Console.WriteLine(Thread.CurrentThread.Name + " has picked up his right chopstick");

            Console.WriteLine(Thread.CurrentThread.Name + " is eating");
            // This represents a Philosopher eating
            Thread.Sleep(100);

            Console.WriteLine(Thread.CurrentThread.Name + " is putting down his left chopstick");
            leftChopstick.Release();
            Console.WriteLine(Thread.CurrentThread.Name + " has put down his left chopstick");
            Console.WriteLine(Thread.CurrentThread.Name + " is putting down his right chopstick");
            rightChopstick.Release();
            Console.WriteLine(Thread.CurrentThread.Name + " has put down his right chopstick");
        }

        private static void Think()
        {
            // This represents a Philosopher thinking
            Thread.Sleep(100);
        }

        public void Dine(Mutex leftChopstick, Mutex rightChopstick)
        {
            Console.WriteLine(Thread.CurrentThread.Name + " sat down at the table");
            while (true)
            {
                Eat(leftChopstick, rightChopstick);
                Think();
            }
        }
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            new DiningPhilosophers().Run();
        }
    }
}
