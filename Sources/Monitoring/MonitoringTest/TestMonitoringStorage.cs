using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Business.BusinessObjects.Domain;
using NDepth.Common.DisposableLock;

namespace NDepth.Monitoring.Test
{
    /// <summary>Test monitoring storage puts all monitoring event into memory collection and allows to fetch them</summary>
    public class TestMonitoringStorage : IMonitoringStorage
    {
        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();
        private static readonly List<MonitoringEvent> Storage = new List<MonitoringEvent>();

        public int FlushQueueSize
        {
            get
            {
                using (new ReadLock(Lock))
                {
                    return Storage.Count;
                }
            }
        }

        public void Store(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            using (new WriteLock(Lock))
            {
                Storage.Add(new MonitoringEvent
                {
                    Id = Storage.Count + 1,
                    Timestamp = timestamp,
                    Machine = machine,
                    Module = module,
                    Component = component,
                    Severity = severity,
                    Title = title,
                    Description = description
                });
            }
        }

        public IList<MonitoringEvent> Fetch(DateTime fromTimestamp, DateTime toTimestamp, string machine, string module, string component, long? pageId, int pageSize, bool pageForward)
        {
            using (new ReadLock(Lock))
            {
                var storageOrdered = pageForward ? Storage.OrderBy(item => item.Id) : Storage.OrderByDescending(item => item.Id);
                var storageSkipped = pageForward ? storageOrdered.SkipWhile(item => (item.Id <= (pageId ?? 0))) : storageOrdered.SkipWhile(item => (item.Id >= (pageId ?? long.MaxValue)));

                return storageSkipped
                    .Where(item => ((item.Timestamp >= fromTimestamp) && (item.Timestamp <= toTimestamp)))
                    .Where(item => (string.IsNullOrEmpty(machine) || item.Machine == machine))
                    .Where(item => (string.IsNullOrEmpty(module) || item.Module == module))
                    .Where(item => (string.IsNullOrEmpty(component) || item.Component == component))
                    .Take(pageSize)
                    .ToList();
            }
        }

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
                    using (new WriteLock(Lock))
                    {
                        Storage.Clear();
                    }
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~TestMonitoringStorage()
        {
            Dispose(false);
        }

        #endregion
    }
}
