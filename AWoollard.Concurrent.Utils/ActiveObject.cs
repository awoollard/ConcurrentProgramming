using System.Threading;

namespace AWoollard.Concurrent.Utils
{
    /// <summary>
    /// An ActiveObject containing a thread which will execute the Run method when Start is called.
    /// The intention is that a class can inherit from ActiveObject, overriding its Run method
    /// to provide custom functionality.
    /// </summary>
    public abstract class ActiveObject
    {
        private readonly Thread _thread;

        /// <summary>
        /// Constructor for the ActiveObject
        /// </summary>
        /// <param name="name">The name for the ActiveObject</param>
        protected ActiveObject(string name = "ActiveObject")
        {
            _thread = new Thread(Run) {Name = name};
        }

        /// <summary>
        /// Starts the execution of the ActiveObjects thread
        /// </summary>
        public void Start()
        {
            _thread.Start();
        }

        /// <summary>
        /// The method which the ActiveObjects thread will run once it is started
        /// </summary>
        protected abstract void Run();
    }
}
