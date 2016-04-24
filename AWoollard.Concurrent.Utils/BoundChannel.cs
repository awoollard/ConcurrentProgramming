namespace AWoollard.Concurrent.Utils
{
    /// <summary>
    /// A bound channel which can be used for interthread communication
    /// </summary>
    /// <typeparam name="T">The type of items stored in the channel</typeparam>
    public class BoundChannel<T> : Channel<T>
    {
        private readonly Semaphore _semaphore;

        /// <summary>
        /// The initialiser for the bound channel
        /// </summary>
        /// <param name="maximum">The maximum amount of items permitted in the channel at any time</param>
        public BoundChannel(uint maximum) : base()
        {
            _semaphore = new Semaphore();
            _semaphore.Release(maximum);
        }

        /// <summary>
        /// Puts an item into the channel
        /// </summary>
        /// <param name="item">The item</param>
        public override void Put(T item)
        {
            _semaphore.Acquire();
            base.Put(item);
        }

        /// <summary>
        /// Takes an item from the channel
        /// </summary>
        /// <returns>The item</returns>
        public override T Take()
        {
            var item = base.Take();
            _semaphore.Release();
            return item;
        }
    }
}