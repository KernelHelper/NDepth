using System;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring
{
    /// <summary>Monitoring notify interface provides functionality to notify managers with simple notification</summary>
    public interface IMonitoringNotify : IFlushQueueInfo, IDisposable
    {
        /// <summary>Notify managers with simple notification</summary>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="machine">Machine name</param>
        /// <param name="module">Module name</param>
        /// <param name="component">Component name</param>
        /// <param name="severity">Severity</param>
        /// <param name="title">Title of the monitoring event</param>
        /// <param name="description">Description of the monitoring event</param>
        void Notify(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description);
    }

    /// <summary>Monitoring notify interface provides functionality to notify managers with Email</summary>
    public interface IMonitoringNotifyEmail : IFlushQueueInfo, IDisposable
    {
        /// <summary>Notify managers with Email</summary>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="machine">Machine name</param>
        /// <param name="module">Module name</param>
        /// <param name="component">Component name</param>
        /// <param name="severity">Severity</param>
        /// <param name="title">Title of the monitoring event</param>
        /// <param name="description">Description of the monitoring event</param>
        void NotifyWithEmail(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description);
    }

    /// <summary>Monitoring notify interface provides functionality to notify managers with SMS</summary>
    public interface IMonitoringNotifySms : IFlushQueueInfo, IDisposable
    {
        /// <summary>Notify managers with SMS</summary>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="machine">Machine name</param>
        /// <param name="module">Module name</param>
        /// <param name="component">Component name</param>
        /// <param name="severity">Severity</param>
        /// <param name="title">Title of the monitoring event</param>
        /// <param name="description">Description of the monitoring event</param>
        void NotifyWithSms(DateTime timestamp, string machine, string module, string component, Severity severity, string title, string description);
    }
}
