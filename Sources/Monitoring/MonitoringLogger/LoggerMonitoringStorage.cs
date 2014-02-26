using System;
using System.Collections.Generic;
using Common.Logging;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Business.BusinessObjects.Domain;

namespace NDepth.Monitoring.Logger
{
    /// <summary>Logger monitoring storage stores monitoring events to logger and does not fetch them</summary>
    public class LoggerMonitoringStorage : IMonitoringStorage
    {
        private static readonly ILog Logger = LogManager.GetLogger("MonitoringStorage");

        public int FlushQueueSize { get { return 0; } }

        public void Store(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            Logger.LogWithSeverity(severity, Resources.Strings.MonitoringEvent, title, description, machine, module, component, severity);
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
