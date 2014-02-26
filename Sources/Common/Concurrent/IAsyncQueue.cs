using System;

namespace NDepth.Common.Concurrent
{
    /// <summary>
    /// Asynchronous queue interface for single item processing.
    /// </summary>
    public interface IAsyncQueue<T> : IDisposable where T : class
    {
        /// <summary>
        /// Is asynchronous queue in processing mode?
        /// </summary>
        bool IsProcessing { get; }
        /// <summary>
        /// Is asynchronous queue get the overflow error?
        /// </summary>
        bool IsOverflowError { get; }

        /// <summary>
        /// Get size of the asynchronous queue.
        /// </summary>
        int AsyncQueueSize { get; }

        /// <summary>
        /// Event that will be called to inform about asynchronous processing started (raised in the main thread).
        /// </summary>        
        event Action HandleStart;

        /// <summary>
        /// Event that will be called to inform about asynchronous processing stopped (raised in the main thread).
        /// </summary>        
        event Action HandleStop;

        /// <summary>
        /// Event that will be called to asynchronously process next item from the queue (raised in the processing thread).
        /// </summary>        
        event Action<T> HandleItem;

        /// <summary>
        /// Event that will be called to inform about the queue overflow (raised in the main thread).
        /// </summary>        
        event Action<int> HandleOverflow;

        /// <summary>
        /// Start the processing thread.
        /// </summary>
        void StartProcessing();

        /// <summary>
        /// Stop the processing thread.
        /// </summary>
        void StopProcessing();

        /// <summary>
        /// Add new item into the asynchronous queue.
        /// </summary>
        /// <param name="item">New item to add</param>
        void AddItem(T item);
    }
}
