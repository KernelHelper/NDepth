using NDepth.Common.Concurrent;

namespace log4net.Async.Strategy
{
    /// <summary>
    /// Asynchronous logging strategy. Blocking collection version.
    /// </summary>
    public class AsyncStrategyBlocking : AsyncQueueBlocking<string>, IAsyncStrategy
    {
        #region Constructor

        /// <summary>
        /// Create instance of the asynchronous logging strategy.
        /// </summary>
        public AsyncStrategyBlocking()
            : base("AsyncStrategyBlocking", BlockType.Block)
        {
        }

        #endregion
    }
}
