using System;
using System.Collections.Generic;
using System.Threading;
using Common.Logging;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Business.BusinessObjects.Domain;
using NDepth.Common.Concurrent;
using NDepth.Database.Hibernate;

namespace NDepth.Monitoring.Hibernate
{
    /// <summary>Database monitoring storage puts all monitoring event into database and allows to fetch them using NHibernate component</summary>
    public class HibernateMonitoringStorage : IMonitoringStorage
    {
        private static int _asyncHibernateMonitoringStorageCount;

        private const int MaxItemsPerTransaction = 100;

        private readonly HibernateCore _hibernateCore;
        private readonly ILog _logger;
        private readonly AsyncQueueHotSwap<MonitoringEvent> _storageQueue;

        #region Constructor

        /// <summary>
        /// Create instance of the asynchronous database monitoring storage and start the flushing thread (hot swap version)
        /// </summary>
        /// <param name="hibernateCore">Hibernate core</param>
        public HibernateMonitoringStorage(HibernateCore hibernateCore)
        {
            // Prepare Hibernate core.
            _hibernateCore = hibernateCore;

            // Prepare storage name.
            var name = "HibernateMonitoringStorage-" + Interlocked.Increment(ref _asyncHibernateMonitoringStorageCount);

            // Prepare storage logger.
            _logger = LogManager.GetLogger(name);

            // Create and start storage queue.
            _storageQueue = new AsyncQueueHotSwap<MonitoringEvent>(name);
            _storageQueue.HandleRange += FlushToDBStorage;
            _storageQueue.HandleOverflow += growLimit => _logger.ErrorFormat(Resources.Strings.DBMonitoringQueueOverflow, growLimit);
            _storageQueue.StartProcessing();
        }

        #endregion

        #region Update configuration

        /// <summary>Update configuration of the current database monitoring storage</summary>
        /// <param name="growType">Type of the growing strategy</param>
        /// <param name="growLimit">Growing limit</param>
        public void UpdateConfiguration(GrowType growType, int growLimit)
        {
            // Stop previous storage queue processing thread.
            _storageQueue.StopProcessing();

            _storageQueue.GrowType = growType;
            _storageQueue.GrowLimit = growLimit;

            // Start new storage queue processing thread.
            _storageQueue.StartProcessing();
        }

        #endregion

        #region Monitoring storage interface

        public int FlushQueueSize { get { return _storageQueue.AsyncQueueSize; } }

        public void Store(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            // Add new monitoring event into the queue.
            _storageQueue.AddItem(new MonitoringEvent
            {
                Timestamp = timestamp,
                Machine = machine,
                Module = module,
                Component = component,
                Severity = severity,
                Title = title,
                Description = description
            });
        }

        public IList<MonitoringEvent> Fetch(DateTime fromTimestamp, DateTime toTimestamp, string machine, string module, string component, long? pageId, int pageSize, bool pageForward)
        {
            try
            {
                return _hibernateCore.PerformTransaction(session =>
                {
                    var storageBase = session.QueryOver<MonitoringEvent>();
                    var storageWhereTimeStamp = storageBase.Where(item => ((item.Timestamp >= fromTimestamp) && (item.Timestamp <= toTimestamp)));
                    var storageSkipped = pageForward ? storageWhereTimeStamp.Where(item => (item.Id > (pageId ?? 0))) : storageWhereTimeStamp.Where(item => (item.Id < (pageId ?? long.MaxValue)));
                    var storageOrdered = pageForward ? storageSkipped.OrderBy(item => item.Id).Asc : storageSkipped.OrderBy(item => item.Id).Desc;
                    var storageWhereMachine = string.IsNullOrEmpty(machine) ? storageOrdered : storageOrdered.Where(item => item.Machine == machine);
                    var storageWhereModule = string.IsNullOrEmpty(module) ? storageWhereMachine : storageWhereMachine.Where(item => item.Module == module);
                    var storageWhereComponent = string.IsNullOrEmpty(component) ? storageWhereModule : storageWhereModule.Where(item => item.Component == component);
                    return storageWhereComponent.Take(pageSize).List();
                });
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat(Resources.Strings.DBMonitoringFetchError, ex);
                throw;
            }
        }

        #endregion

        #region Asynchronous flushing

        private void FlushToDBStorage(IReadOnlyList<MonitoringEvent> buffer)
        {
            var exit = false;
            var error = false;
            var index = 0;

            // Split the buffer to transactions with bulk inserts.
            do
            {
                try
                {
                    // Start the new transaction.
                    var i = index;
                    var count = 0;
                    _hibernateCore.PerformTransaction(session =>
                    {
                        for (; (i < buffer.Count) && (count <= MaxItemsPerTransaction); i++)
                        {
                            var monitoringEvent = buffer[i];
                            if (monitoringEvent != null)
                            {
                                session.Save(monitoringEvent);
                                count++;
                            }
                            else
                            {
                                exit = true;
                                break;
                            }
                        }
                    });

                    // Increment the current index.
                    index += count;

                    // Reset error flag.
                    error = false;
                }
                catch (Exception ex)
                {
                    // Log the first error.
                    if (!error)
                        _logger.ErrorFormat(Resources.Strings.DBMonitoringQueueFlushError, ex, buffer.Count - index);

                    // Set the error flag.
                    error = true;

                    // Give a control to the next thread.
                    Thread.Yield();
                }
            } while (!exit && (buffer.Count - index > 0));
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
                    _storageQueue.Dispose();
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~HibernateMonitoringStorage()
        {
            Dispose(false);
        }

        #endregion
    }
}
