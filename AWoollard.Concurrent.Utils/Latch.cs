namespace AWoollard.Concurrent.Utils
{
    /// <summary>
    /// A latch which can be used by threads to wait for other
    /// threads to complete an operation before continuing execution
    /// </summary>
    public class Latch : Semaphore
    {
        /// <summary>
        /// Initialises the latch
        /// </summary>
        public Latch() : base(0)
        {
            
        }

        /// <summary>
        /// Acquires the latch. Blocks threads until Latch.Release is called.
        /// </summary>
        public override void Acquire()
        {
            // Since the latch is initialised with zero tokens,
            // all threads will immediately block on Acquire
            base.Acquire();
            base.Release();
        }
        
    }
}
