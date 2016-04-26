using System;
using System.Collections.Generic;
using System.Threading;
using Mutex = AWoollard.Concurrent.Utils.Mutex;

namespace DiningPhilosophers
{
    /// <summary>
    /// Represents a Philosopher being left or right-handed.
    /// Determines whether the philosopher reaches for
    /// the left chopstick or right chopstick first.
    /// </summary>
    public enum HandOrientation
    {
        LeftHanded,
        RightHanded
    }

    /// <summary>
    /// Represents the Dining Philosophers problem where there are
    /// five philosophers each trying to eat their meals with a limited
    /// amount of chopsticks. The philosophers take breaks to think.
    /// </summary>
    public class DiningPhilosophers
    {
        private readonly List<Mutex> _chopstick;
        private readonly List<Philosopher> _philosopher;

        /// <summary>
        /// Initialises the DiningPhilosophers.
        /// Creates five philosophers, and five chopsticks.
        /// </summary>
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

        /// <summary>
        /// Creates a thread pool where each thread represents a philosopher dining.
        /// Executes Philosopher.Dine for each philosopher.
        /// All of the philosophers except one are left handed, which prevents deadlocking.
        /// </summary>
        public void Run()
        {
            var threadList = new List<Thread>
            {
                new Thread(() => _philosopher[0].Dine(HandOrientation.LeftHanded, _chopstick[0], _chopstick[1])),
                new Thread(() => _philosopher[1].Dine(HandOrientation.LeftHanded, _chopstick[1], _chopstick[2])),
                new Thread(() => _philosopher[2].Dine(HandOrientation.LeftHanded, _chopstick[2], _chopstick[3])),
                new Thread(() => _philosopher[3].Dine(HandOrientation.LeftHanded, _chopstick[3], _chopstick[4])),
                new Thread(() => _philosopher[4].Dine(HandOrientation.RightHanded, _chopstick[4], _chopstick[0]))
            };

            for (var i = 0; i < threadList.Count; i++)
            {
                var thread = threadList[i];
                thread.Name = "Philosopher " + (i + 1);
                thread.Start();
            }
        }
    }

    /// <summary>
    /// A Philosopher which has a chopstick to his left, and a chopstick to his right.
    /// The philosopher will attempt to pick up his chopsticks in an order specified by
    /// the HandOrientation parameter, and occasionally takes breaks to think.
    /// </summary>
    public class Philosopher
    {
        /// <summary>
        /// Picks up a chopstick. Waits if the chopstick is in use by another Philosopher.
        /// Prints messages to the console regarding what the philosopher is doing.
        /// </summary>
        /// <param name="chopstick">The chopstick to pick up</param>
        /// <param name="leftOrRight">Should be set to "left" if the chopstick is
        /// to the left of the philosopher, or "right" if the chopstick is to the
        /// right of the philosopher. Simply used for logging.</param>
        private static void PickUpChopstick(Mutex chopstick, string leftOrRight)
        {
            Console.WriteLine(Thread.CurrentThread.Name + " is picking up his " + leftOrRight + " chopstick");
            chopstick.Acquire();
            Console.WriteLine(Thread.CurrentThread.Name + " has picked up his " + leftOrRight + " chopstick");
        }

        /// <summary>
        /// Puts down a chopstick, potentially allowing another Philosopher to use it.
        /// Prints messages to the console regarding what the philosopher is doing.
        /// </summary>
        /// <param name="chopstick">The chopstick to put down</param>
        /// <param name="leftOrRight">Should be set to "left" if the chopstick is
        /// to the left of the philosopher, or "right" if the chopstick is to the
        /// right of the philosopher. Simply used for logging.</param>
        private static void PutDownChopstick(Mutex chopstick, string leftOrRight)
        {
            Console.WriteLine(Thread.CurrentThread.Name + " is putting down his " + leftOrRight + " chopstick");
            chopstick.Release();
            Console.WriteLine(Thread.CurrentThread.Name + " has put down his " + leftOrRight + " chopstick");
        }

        /// <summary>
        /// Instructs the philosopher to pick up his chopsticks depending on the
        /// philosophers hand orientation.
        /// </summary>
        /// <param name="handOrientation">The hand orientation of the philosopher</param>
        /// <param name="leftChopstick">The chopstick to the left of the philosopher</param>
        /// <param name="rightChopstick">The chopstick to the right of the philosopher</param>
        private static void Eat(HandOrientation handOrientation, Mutex leftChopstick, Mutex rightChopstick)
        {
            switch (handOrientation)
            {
                case HandOrientation.LeftHanded:
                    PickUpChopstick(leftChopstick, "left");
                    PickUpChopstick(rightChopstick, "right");
                    break;
                case HandOrientation.RightHanded:
                    PickUpChopstick(rightChopstick, "right");
                    PickUpChopstick(leftChopstick, "left");
                    break;
            }
            

            Console.WriteLine(Thread.CurrentThread.Name + " is eating");
            // This represents a Philosopher eating
            Thread.Sleep(100);

            switch (handOrientation)
            {
                case HandOrientation.LeftHanded:
                    PutDownChopstick(leftChopstick, "left");
                    PutDownChopstick(rightChopstick, "right");
                    break;
                case HandOrientation.RightHanded:
                    PutDownChopstick(rightChopstick, "right");
                    PutDownChopstick(leftChopstick, "left");
                    break;
            }
        }

        /// <summary>
        /// Instructs a philosopher to contemplate life.
        /// </summary>
        private static void Think()
        {
            // This represents a Philosopher thinking
            Thread.Sleep(100);
        }

        /// <summary>
        /// Seats a philosopher at the table where a meal is before them, and two chopsticks.
        /// Continuously instructs the philosopher to eat, taking breaks to think.
        /// </summary>
        /// <param name="handOrientation">The hand orientation of the philosopher, either left or right</param>
        /// <param name="leftChopstick">The chopstick to the left of the philosopher</param>
        /// <param name="rightChopstick">The chopstick to the right of the philosopher</param>
        public void Dine(HandOrientation handOrientation, Mutex leftChopstick, Mutex rightChopstick)
        {
            Console.WriteLine(Thread.CurrentThread.Name + " sat down at the table");
            while (true)
            {
                Eat(handOrientation, leftChopstick, rightChopstick);
                Think();
            }
        }
    }

    public static class Program
    {
        /// <summary>
        /// Entry point for the DiningPhilosophers program. 
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            new DiningPhilosophers().Run();
        }
    }
}
