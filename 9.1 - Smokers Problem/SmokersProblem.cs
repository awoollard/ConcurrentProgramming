using System.Threading;
using Semaphore = AWoollard.Concurrent.Utils.Semaphore;

namespace SmokersProblem
{
    public class SmokersProblem
    {
        private static Semaphore _agentSem;
        private static Semaphore _tobacco;
        private static Semaphore _paper;
        private static Semaphore _match;
        private readonly Thread _agentA;
        private readonly Thread _agentB;
        private readonly Thread _agentC;

        public SmokersProblem()
        {
            _agentSem = new Semaphore(1);
            _tobacco = new Semaphore(0);
            _paper = new Semaphore(0);
            _match = new Semaphore(0);
            _agentA = new Thread(AgentA);
            _agentB = new Thread(AgentB);
            _agentC = new Thread(AgentC);
        }

        private static void AgentA()
        {
            _agentSem.Acquire();
            _tobacco.Release();
            _paper.Release();
        }

        private static void AgentB()
        {
            _agentSem.Acquire();
            _paper.Release();
            _match.Release();
        }

        private static void AgentC()
        {
            _agentSem.Acquire();
            _tobacco.Release();
            _match.Release();
        }

        public void Run()
        {
            _agentA.Start();
            _agentB.Start();
            _agentC.Start();
        }


    }

    public static class Program
    {
        /// <summary>
        /// Entry point for the SmokersProblem program. 
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            var smokersProblem = new SmokersProblem();
            smokersProblem.Run();
        }
    }
}
