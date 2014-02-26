using NDepth.Common.Concurrent;

namespace log4net.Async.Strategy
{
    /// <summary>
    /// Asynchronous logging strategy. LMAX Disruptor version.
    /// </summary>
    public class AsyncStrategyDisruptor : AsyncQueueDisruptor<string>, IAsyncStrategy
    {
        #region Constructor

        /// <summary>
        /// Create instance of the asynchronous logging strategy.
        /// </summary>
        public AsyncStrategyDisruptor()
            : base("AsyncStrategyDisruptor")
        {
        }

        #endregion
    }
}
