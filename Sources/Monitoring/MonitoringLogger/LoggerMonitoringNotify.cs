using System;
using Common.Logging;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring.Logger
{
    /// <summary>Logger monitoring notify writes to log any notification event</summary>
    public class LoggerMonitoringNotify : IMonitoringNotify
    {
        private static readonly ILog Logger = LogManager.GetLogger("MonitoringNotify");

        public int FlushQueueSize { get { return 0; } }

        public void Notify(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            Logger.LogWithSeverity(severity, Resources.Strings.MonitoringNotification, title, description, machine, module, component, severity);
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }

    /// <summary>Logger monitoring Email notify writes to log any Email notification event</summary>
    public class LoggerMonitoringNotifyEmail : IMonitoringNotifyEmail
    {
        private static readonly ILog Logger = LogManager.GetLogger("MonitoringNotifyEmail");

        public int FlushQueueSize { get { return 0; } }

        public void NotifyWithEmail(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            Logger.LogWithSeverity(severity, Resources.Strings.MonitoringEmailNotification, title, description, machine, module, component, severity);
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }

    /// <summary>Logger monitoring SMS notify writes to log any SMS notification event</summary>
    public class LoggerMonitoringNotifySms : IMonitoringNotifySms
    {
        private static readonly ILog Logger = LogManager.GetLogger("MonitoringNotifySms");

        public int FlushQueueSize { get { return 0; } }

        public void NotifyWithSms(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            Logger.LogWithSeverity(severity, Resources.Strings.MonitoringSmsNotification, title, description, machine, module, component, severity);
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }
}
