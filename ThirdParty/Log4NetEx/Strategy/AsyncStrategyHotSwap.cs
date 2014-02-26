using NDepth.Common.Concurrent;

namespace log4net.Async.Strategy
{
    /// <summary>
    /// Asynchronous logging strategy. Hot swap version.
    /// </summary>
    public class AsyncStrategyHotSwap : AsyncQueueHotSwap<string>, IAsyncStrategy
    {
        #region Constructor

        /// <summary>
        /// Create instance of the asynchronous logging strategy.
        /// </summary>
        public AsyncStrategyHotSwap()
            : base("AsyncStrategyHotSwap", GrowType.Grow)
        {
        }

        #endregion
    }
}
