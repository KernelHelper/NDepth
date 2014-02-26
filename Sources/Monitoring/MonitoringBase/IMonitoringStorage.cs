using System;
using System.Collections.Generic;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Business.BusinessObjects.Domain;

namespace NDepth.Monitoring
{
    /// <summary>Monitoring storage interface provides functionality to store and fetch monitoring events</summary>
    public interface IMonitoringStorage : IFlushQueueInfo, IDisposable
    {
        /// <summary>Store monitoring event</summary>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="machine">Machine name</param>
        /// <param name="module">Module name</param>
        /// <param name="component">Component name</param>
        /// <param name="severity">Severity</param>
        /// <param name="title">Title of the monitoring event</param>
        /// <param name="description">Description of the monitoring event</param>
        void Store(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description);

        /// <summary>Fetch monitoring events with date time range and paging</summary>
        /// <param name="fromTimestamp">From timestamp</param>
        /// <param name="toTimestamp">To timestamp</param>
        /// <param name="machine">Machine name (null for any machine)</param>
        /// <param name="module">Module name (null for any module)</param>
        /// <param name="component">Component name (null for any component)</param>
        /// <param name="pageId">Page fetch last Id (null for the first page)</param>
        /// <param name="pageSize">Page fetch size</param>
        /// <param name="pageForward">Page fetch forward direction flag (true - fetch forward, false - fetch backward)</param>
        /// <returns>List of monitoring events sorted by </returns>
        IList<MonitoringEvent> Fetch(DateTime fromTimestamp, DateTime toTimestamp, string machine, string module, string component, long? pageId, int pageSize, bool pageForward);
    }
}
