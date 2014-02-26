using System;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring.Null
{
    /// <summary>Null monitoring notify does nothing on any notification event</summary>
    public class NullMonitoringNotify : IMonitoringNotify
    {
        public int FlushQueueSize { get { return 0; } }

        public void Notify(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            // Do nothing.
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }

    /// <summary>Null monitoring Email notify does nothing on any Email notification event</summary>
    public class NullMonitoringNotifyEmail : IMonitoringNotifyEmail
    {
        public int FlushQueueSize { get { return 0; } }

        public void NotifyWithEmail(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            // Do nothing.
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }

    /// <summary>Null monitoring SMS notify does nothing on any SMS notification event</summary>
    public class NullMonitoringNotifySms : IMonitoringNotifySms
    {
        public int FlushQueueSize { get { return 0; } }

        public void NotifyWithSms(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            // Do nothing.
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }
}
