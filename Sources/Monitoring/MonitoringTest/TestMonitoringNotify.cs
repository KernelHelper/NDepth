using System;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring.Test
{
    /// <summary>Test monitoring notify puts notification event into the memory collection</summary>
    public class TestMonitoringNotify : IMonitoringNotify
    {
        private readonly TestMonitoringStorage _storage = new TestMonitoringStorage();

        public int FlushQueueSize { get { return _storage.FlushQueueSize; } }

        public void Notify(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            _storage.Store(timestamp, machine, module, component, severity, string.Format(Resources.Strings.MonitoringNotification, title), description);
        }

        public void Dispose()
        {
            _storage.Dispose();
        }
    }

    /// <summary>Test monitoring Email notify puts Email notification event into the memory collection</summary>
    public class TestMonitoringNotifyEmail : IMonitoringNotifyEmail
    {
        private readonly TestMonitoringStorage _storage = new TestMonitoringStorage();

        public int FlushQueueSize { get { return _storage.FlushQueueSize; } }

        public void NotifyWithEmail(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            _storage.Store(timestamp, machine, module, component, severity, string.Format(Resources.Strings.MonitoringEmailNotification, title), description);
        }

        public void Dispose()
        {
            _storage.Dispose();
        }
    }

    /// <summary>Test monitoring SMS notify puts SMS notification event into the memory collection</summary>
    public class TestMonitoringNotifySms : IMonitoringNotifySms
    {
        private readonly TestMonitoringStorage _storage = new TestMonitoringStorage();

        public int FlushQueueSize { get { return _storage.FlushQueueSize; } }

        public void NotifyWithSms(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description)
        {
            _storage.Store(timestamp, machine, module, component, severity, string.Format(Resources.Strings.MonitoringSmsNotification, title), description);
        }

        public void Dispose()
        {
            _storage.Dispose();
        }
    }
}
