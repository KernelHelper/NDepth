using System;
using System.Collections.Generic;

namespace NDepth.Common.Concurrent
{
    /// <summary>
    /// Asynchronous queue interface for range items processing.
    /// </summary>
    public interface IAsyncQueueRange<T> : IAsyncQueue<T> where T : class
    {
        /// <summary>
        /// Event that will be called to asynchronously process range of next items from the queue (raised in the processing thread).
        /// </summary>        
        event Action<IReadOnlyList<T>> HandleRange;

        /// <summary>
        /// Add new range of items into the asynchronous queue.
        /// </summary>
        /// <param name="items">Range of items to add</param>
        void AddRange(IEnumerable<T> items);
    }
}
