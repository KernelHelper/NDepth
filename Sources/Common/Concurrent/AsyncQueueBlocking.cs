using System;
using System.Collections.Concurrent;
using System.Threading;

namespace NDepth.Common.Concurrent
{
    /// <summary>
    /// Blocking strategy types
    /// </summary>
    public enum BlockType
    {
        /// <summary>
        /// Error on buffer limit (default)
        /// </summary>
        Error,

        /// <summary>
        /// Skip on buffer limit
        /// </summary>
        Skip,

        /// <summary>
        /// Grow on buffer limit
        /// </summary>
        Grow,

        /// <summary>
        /// Block on buffer limit
        /// </summary>
        Block
    }

    /// <summary>
    /// Asynchronous queue implementation using blocking collection.
    /// </summary>
    public class AsyncQueueBlocking<T> : IAsyncQueue<T> where T : class
    {
        private BlockType _blockType;
        private int _blockLimit;

        private int _asyncQueueSize;
        private BlockingCollection<T> _processingCollection;
        private Thread _asyncProcessingThread;

        #region Constructor

        /// <summary>
        /// Create instance of the asynchronous queue (blocking collection version).
        /// </summary>
        /// <param name="name">Name of the asynchronous queue</param>
        /// <param name="blockType">Type of the blocking strategy</param>
        /// <param name="blockLimit">Blocking buffer limit</param>         
        public AsyncQueueBlocking(string name, BlockType blockType = BlockType.Error, int blockLimit = 1000000)
        {
            Name = name;
            BlockType = blockType;
            BlockLimit = blockLimit;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Name of the asynchronous queue
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the blocking strategy
        /// </summary>
        public BlockType BlockType { get; set; }

        /// <summary>
        /// Blocking buffer limit
        /// </summary>
        public int BlockLimit { get; set; }

        #endregion

        #region IAsyncQueue implementation

        public bool IsProcessing { get; private set; }
        public bool IsOverflowError { get; private set; }
        public int AsyncQueueSize { get { return _asyncQueueSize; } }

        public event Action HandleStart = delegate { };
        public event Action HandleStop = delegate { };
        public event Action<T> HandleItem = delegate { };
        public event Action<int> HandleOverflow = delegate { };

        public void StartProcessing()
        {
            // Check the processing started flag.
            if (IsProcessing)
                throw new InvalidOperationException(Resources.Strings.AsyncQueueStartError);

            _blockType = BlockType;
            _blockLimit = Math.Max(1, BlockLimit);

            // Reset asynchronous queue size.
            Interlocked.Exchange(ref _asyncQueueSize, 0);

            // Initialize processing collection.
            _processingCollection = (_blockType == BlockType.Grow) ? new BlockingCollection<T>() : new BlockingCollection<T>(_blockLimit);

            // Create and start processing thread.
            _asyncProcessingThread = new Thread(ProcessingThreadMethod)
            {
                Name = Name,
                IsBackground = true,
                Priority = ThreadPriority.Lowest
            };
            _asyncProcessingThread.Start();

            // Set the processing started flag.
            IsProcessing = true;

            // Reset the overflow error flag.
            IsOverflowError = false;

            // Rise asynchronous processing start event.
            HandleStart();
        }

        public void StopProcessing()
        {
            // Check the processing started flag.
            if (!IsProcessing)
                return;

            // Stop adding to the processing collection.
            _processingCollection.CompleteAdding();

            // Wait for the processing thread ends.
            _asyncProcessingThread.Join();
            _asyncProcessingThread = null;

            _processingCollection = null;

            // Reset asynchronous queue size.
            Interlocked.Exchange(ref _asyncQueueSize, 0);

            // Reset the processing started flag.
            IsProcessing = false;

            // Reset the overflow error flag.
            IsOverflowError = false;

            // Rise asynchronous processing stop event.
            HandleStop();
        }

        public void AddItem(T item)
        {
            // Check the processing started flag.
            if (!IsProcessing)
                throw new InvalidOperationException(Resources.Strings.AsyncQueueAddError);

            // Skip empty items.
            if (item == null)
                return;

            AddItemInternal(item);
        }

        #endregion

        #region Asynchronous processing

        private void AddItemInternal(T item)
        {
            // Perform skip action for the corresponding blocking strategy.
            if ((_blockType == BlockType.Skip) && (_processingCollection.Count >= _blockLimit))
                return;

            // Perform error action for the corresponding blocking strategy.
            if (_blockType == BlockType.Error)
            {
                // Reset the error flag only after threshold. 
                if (IsOverflowError && (_processingCollection.Count <= (_blockLimit / 2)))
                    IsOverflowError = false;

                // Check and set error flag if it meets the block limit.
                if (_processingCollection.Count >= _blockLimit)
                {
                    if (!IsOverflowError)
                    {
                        // Rise overflow event.
                        HandleOverflow(_blockLimit);
                        IsOverflowError = true;
                    }
                    return;
                }
            }                

            // Update asynchronous queue size.
            Interlocked.Increment(ref _asyncQueueSize);

            // Add new item into the collection.
            _processingCollection.Add(item);
        }

        private void ProcessingThreadMethod()
        {
            // Handle all items in processing collection until it complete.
            foreach (var item in _processingCollection.GetConsumingEnumerable())
            {
                // Rise item processing event.
                HandleItem(item);

                // Update asynchronous queue size.
                Interlocked.Decrement(ref _asyncQueueSize);
            }
        }

        #endregion

        #region IDisposable implementation

        // Disposed flag.
        private bool _disposed;

        // Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposingManagedResources)
        {
            if (!_disposed)
            {
                if (disposingManagedResources)
                {
                    // Dispose managed resources here...

                    // Stop processing thread.
                    StopProcessing();
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~AsyncQueueBlocking()
        {
            Dispose(false);
        }

        #endregion
    }
}
