using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NDepth.Common.Concurrent
{
    /// <summary>
    /// Growing strategy types
    /// </summary>
    public enum GrowType
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
        Grow
    }

    /// <summary>
    /// Asynchronous queue implementation using hot swap technique.
    /// </summary>
    public class AsyncQueueHotSwap<T> : IAsyncQueueRange<T> where T : class
    {
        private GrowType _growType;
        private int _growLimit;

        private int _asyncQueueSize;
        private List<T> _currentCollection;
        private List<T> _processingCollection;
        private AutoResetEvent _asyncProcessingWaiter;
        private object _asyncProcessingLocker;
        private Thread _asyncProcessingThread;

        #region Constructor

        /// <summary>
        /// Create instance of the asynchronous queue (hot swap version).
        /// </summary>
        /// <param name="name">Name of the asynchronous queue</param>
        /// <param name="growType">Type of the growing strategy</param>
        /// <param name="growLimit">Growing limit</param>         
        public AsyncQueueHotSwap(string name, GrowType growType = GrowType.Error, int growLimit = 1000000)
        {
            Name = name;
            GrowType = growType;
            GrowLimit = growLimit;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Name of the asynchronous queue
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the growing strategy
        /// </summary>
        public GrowType GrowType { get; set; }

        /// <summary>
        /// Growing limit
        /// </summary>
        public int GrowLimit { get; set; }

        #endregion

        #region IAsyncQueue implementation

        public bool IsProcessing { get; private set; }
        public bool IsOverflowError { get; private set; }
        public int AsyncQueueSize { get { return _asyncQueueSize; } }

        public event Action HandleStart = delegate { };
        public event Action HandleStop = delegate { };
        public event Action<T> HandleItem = delegate { };
        public event Action<IReadOnlyList<T>> HandleRange = delegate { };
        public event Action<int> HandleOverflow = delegate { };

        public void StartProcessing()
        {
            // Check the processing started flag.
            if (IsProcessing)
                throw new InvalidOperationException(Resources.Strings.AsyncQueueStartError);

            _growType = GrowType;
            _growLimit = Math.Max(1, GrowLimit);

            // Reset asynchronous queue size.
            Interlocked.Exchange(ref _asyncQueueSize, 0);

            // Initialize processing collections.
            _currentCollection = new List<T>();
            _processingCollection = new List<T>();
            _asyncProcessingWaiter = new AutoResetEvent(false);
            _asyncProcessingLocker = new object();

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
            AddItemInternal(null);

            // Wait for the processing thread ends.
            _asyncProcessingThread.Join();
            _asyncProcessingThread = null;

            _currentCollection = null;
            _processingCollection = null;
            _asyncProcessingWaiter = null;
            _asyncProcessingLocker = null;

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

        public void AddRange(IEnumerable<T> items)
        {
            // Check the processing started flag.
            if (!IsProcessing)
                throw new InvalidOperationException(Resources.Strings.AsyncQueueAddError);

            foreach (var item in items.Where(item => item != null))
                AddItemInternal(item);
        }

        #endregion

        #region Asynchronous processing

        private void AddItemInternal(T item)
        {
            var isFirstItem = false;

            lock (_asyncProcessingLocker)
            {
                if (item != null)
                {
                    var processingQueueSize = _currentCollection.Count + _processingCollection.Count;

                    // Perform skip action for the corresponding growing strategy.
                    if ((_growType == GrowType.Skip) && (processingQueueSize >= _growLimit))
                        return;

                    // Perform error action for the corresponding growing strategy.
                    if (_growType == GrowType.Error)
                    {
                        // Reset the error flag only after threshold. 
                        if (IsOverflowError && (processingQueueSize <= (_growLimit / 2)))
                            IsOverflowError = false;

                        // Check and set error flag if it meets the grow limit.
                        if (processingQueueSize >= _growLimit)
                        {
                            if (!IsOverflowError)
                            {
                                // Rise overflow event.
                                HandleOverflow(_growLimit);
                                IsOverflowError = true;
                            }
                            return;
                        }
                    }                    
                }

                // Check for the first item.
                if (_currentCollection.Count == 0)
                    isFirstItem = true;

                // Update asynchronous queue size.
                Interlocked.Increment(ref _asyncQueueSize);

                // Add new item into the collection.
                _currentCollection.Add(item);
            }

            // Notify about new items available.
            if (isFirstItem)
                _asyncProcessingWaiter.Set();
        }

        private void ProcessingThreadMethod()
        {
            var exit = false;

            while (!exit)
            {
                // Perform the hot swap operation.
                lock (_asyncProcessingLocker)
                {
                    Swap(ref _currentCollection, ref _processingCollection);
                }

                // Handle all items in batch mode.
                HandleRange(_processingCollection);

                // Handle all items in single mode.
                foreach (var item in _processingCollection)
                {
                    // Rise item processing event.
                    if (item != null)
                        HandleItem(item);
                    
                    // Update asynchronous queue size.
                    Interlocked.Decrement(ref _asyncQueueSize);

                    // Check for last consuming item.
                    if (item == null)
                    {
                        exit = true;
                        break;
                    }
                }

                // Clear processing collection.
                _processingCollection.Clear();

                // Wait for next available produced items and wakeup.
                if (!exit)
                    _asyncProcessingWaiter.WaitOne();
            }
        }

        private static void Swap<TT>(ref TT a, ref TT b)
        {
            var tmp = a;
            a = b;
            b = tmp;
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
        ~AsyncQueueHotSwap()
        {
            Dispose(false);
        }

        #endregion
    }
}
