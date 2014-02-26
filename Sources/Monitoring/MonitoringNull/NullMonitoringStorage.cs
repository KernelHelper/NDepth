using System;
using System.Collections.Generic;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Business.BusinessObjects.Domain;

namespace NDepth.Monitoring.Null
{
    /// <summary>Null monitoring storage does not store and fetch monitoring events</summary>
    public class NullMonitoringStorage : IMonitoringStorage
    {
        public int FlushQueueSize { get { return 0; } }

        public void Store(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            // Do nothing.
        }

        public IList<MonitoringEvent> Fetch(DateTime fromTimestamp, DateTime toTimestamp, string machine, string module, string component, long? pageId, int pageSize, bool pageForward)
        {
            // Return empty list.
            return new List<MonitoringEvent>();
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }
}
