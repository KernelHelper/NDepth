using NDepth.Common.Concurrent;

namespace log4net.Async.Strategy
{
    /// <summary>
    /// Asynchronous logging strategy interface.
    /// </summary>
    public interface IAsyncStrategy : IAsyncQueue<string>
    {

    }
}
