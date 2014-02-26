using System;
using System.Threading;
using System.Threading.Tasks;
using Disruptor;
using Disruptor.Dsl;

namespace NDepth.Common.Concurrent
{
    /// <summary>
    /// Claim strategy types
    /// </summary>
    public enum ClaimStrategyType
    {
        /// <summary>
        /// Optimized strategy can be used when there is a single publisher thread claiming sequences.
        /// This strategy must <b>not</b> be used when multiple threads are used for publishing concurrently on the same Sequencer.
        /// </summary>
        SingleThreaded,

        /// <summary>
        /// Strategy to be used when there are multiple publisher threads claiming sequences.
        /// This strategy is reasonably forgiving when the multiple publisher threads are highly contended or working in an
        /// environment where there is insufficient CPUs to handle multiple publisher threads.  It requires 2 CAS operations
        /// for a single publisher, compared to the MultithreadedLowContention strategy which needs only a single CAS and a
        /// lazySet per publication.
        /// </summary>
        MultiThreaded,

        /// <summary>
        /// Strategy to be used when there are multiple producer threads claiming sequences.
        /// This strategy requires sufficient cores to allow multiple publishers to be concurrently claiming sequences.
        /// </summary>
        MultiThreadedLowContention
    }

    /// <summary>
    /// Wait strategy types
    /// </summary>
    public enum WaitStrategyType
    {
        /// <summary>
        /// Blocking strategy that uses a lock and condition variable for waiting on a barrier.
        /// This strategy should be used when performance and low-latency are not as important as CPU resource.
        /// </summary>
        Blocking,

        /// <summary>
        /// Busy Spin strategy that uses a busy spin loop for waiting on a barrier.
        /// This strategy will use CPU resource to avoid syscalls which can introduce latency jitter.  
        /// It is best used when threads can be bound to specific CPU cores.
        /// </summary>
        BusySpin,

        /// <summary>
        /// Sleeping strategy that uses a <see cref="T:System.Threading.SpinWait"/> while the are waiting on a barrier.
        /// This strategy is a good compromise between performance and CPU resource. Latency spikes can occur after quiet periods.
        /// </summary>
        Sleeping,

        /// <summary>
        /// Yielding strategy that uses a Thread.Sleep(0) for waiting on a barrier after an initially spinning.
        /// This strategy is a good compromise between performance and CPU resource without incurring significant latency spikes.
        /// Default strategy.
        /// </summary>
        Yielding
    }

    /// <summary>
    /// Asynchronous queue implementation using LMAX Disruptor.
    /// </summary>
    public class AsyncQueueDisruptor<T> : IAsyncQueue<T> where T : class
    {
        private ClaimStrategyType _claimStrategy;
        private WaitStrategyType _waitStrategy;
        private int _bufferSize;

        private int _asyncQueueSize;
        private ProcessingConsumer _processingConsumer;
        private Disruptor<ValueEntry> _processingDisruptor;
        private RingBuffer<ValueEntry> _processingRingBuffer;

        #region Constructor

        /// <summary>
        /// Create instance of the asynchronous queue (LMAX Disruptor version).
        /// </summary>
        /// <param name="name">Name of the asynchronous queue</param>
        /// <param name="claimStrategy">Type of the claim strategy</param>
        /// <param name="waitStrategy">Type of the wait strategy</param>
        /// <param name="bufferSize">Disruptor buffer size</param>         
        public AsyncQueueDisruptor(string name, ClaimStrategyType claimStrategy = ClaimStrategyType.MultiThreaded, WaitStrategyType waitStrategy = WaitStrategyType.Yielding, int bufferSize = 1024)
        {
            Name = name;
            ClaimStrategy = claimStrategy;
            WaitStrategy = waitStrategy;
            BufferSize = bufferSize;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Name of the asynchronous queue
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the claim strategy
        /// </summary>
        public ClaimStrategyType ClaimStrategy { get; set; }

        /// <summary>
        /// Type of the wait strategy
        /// </summary>
        public WaitStrategyType WaitStrategy { get; set; }

        /// <summary>
        /// Disruptor buffer size
        /// </summary>
        public int BufferSize { get; set; }

        #endregion

        #region IAsyncQueue implementation

        public bool IsProcessing { get; private set; }
        public bool IsOverflowError { get { return false; } }
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

            _claimStrategy = ClaimStrategy;
            _waitStrategy = WaitStrategy;
            _bufferSize = Math.Max(128, BufferSize);

            // Reset asynchronous queue size.
            Interlocked.Exchange(ref _asyncQueueSize, 0);

            IClaimStrategy disruptorClaimStrategy;
            switch (_claimStrategy)
            {
                case ClaimStrategyType.SingleThreaded:
                    disruptorClaimStrategy = new SingleThreadedClaimStrategy(_bufferSize);
                    break;
                case ClaimStrategyType.MultiThreaded:
                    disruptorClaimStrategy = new MultiThreadedClaimStrategy(_bufferSize);
                    break;
                case ClaimStrategyType.MultiThreadedLowContention:
                    disruptorClaimStrategy = new MultiThreadedLowContentionClaimStrategy(_bufferSize);
                    break;
                default:
                    disruptorClaimStrategy = new MultiThreadedClaimStrategy(_bufferSize);
                    break;
            }

            IWaitStrategy disruptorWaitStrategy;
            switch (_waitStrategy)
            {
                case WaitStrategyType.Blocking:
                    disruptorWaitStrategy = new BlockingWaitStrategy();
                    break;
                case WaitStrategyType.BusySpin:
                    disruptorWaitStrategy = new BusySpinWaitStrategy();
                    break;
                case WaitStrategyType.Sleeping:
                    disruptorWaitStrategy = new SleepingWaitStrategy();
                    break;
                case WaitStrategyType.Yielding:
                    disruptorWaitStrategy = new YieldingWaitStrategy();
                    break;
                default:
                    disruptorWaitStrategy = new YieldingWaitStrategy();
                    break;
            }

            // Initialize processing consumer.
            _processingConsumer = new ProcessingConsumer(this);

            // Initialize processing disruptor.
            _processingDisruptor = new Disruptor<ValueEntry>(() => new ValueEntry(), disruptorClaimStrategy, disruptorWaitStrategy, TaskScheduler.Default);
            _processingDisruptor.HandleEventsWith(_processingConsumer);

            // Create ring buffer.
            _processingRingBuffer = _processingDisruptor.Start();

            // Set the processing started flag.
            IsProcessing = true;

            // Rise asynchronous processing start event.
            HandleStart();
        }

        public void StopProcessing()
        {
            // Check the processing started flag.
            if (!IsProcessing)
                return;

            // Stop adding to the processing disruptor.
            _processingDisruptor.Shutdown();

            _processingConsumer = null;
            _processingDisruptor = null;
            _processingRingBuffer = null;

            // Reset asynchronous queue size.
            Interlocked.Exchange(ref _asyncQueueSize, 0);

            // Reset the processing started flag.
            IsProcessing = false;

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

        sealed class ValueEntry
        {
            public T Value { get; set; }
        }

        class ProcessingConsumer : IEventHandler<ValueEntry>
        {
            private readonly AsyncQueueDisruptor<T> _appender;

            public ProcessingConsumer(AsyncQueueDisruptor<T> appender)
            {
                _appender = appender;
            }

            public void OnNext(ValueEntry value, long sequence, bool endOfBatch)
            {
                _appender.AddFromDisruptor(value.Value);
            }
        }

        private void AddFromDisruptor(T item)
        {
            // Rise item processing event.
            HandleItem(item);

            // Update asynchronous queue size.
            Interlocked.Decrement(ref _asyncQueueSize);            
        }

        private void AddItemInternal(T item)
        {
            // Add new item into the collection.
            var sequenceNo = _processingRingBuffer.Next();
            _processingRingBuffer[sequenceNo].Value = item;
            _processingRingBuffer.Publish(sequenceNo);

            // Update asynchronous queue size.
            Interlocked.Increment(ref _asyncQueueSize);
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
        ~AsyncQueueDisruptor()
        {
            Dispose(false);
        }

        #endregion
    }
}
