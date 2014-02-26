using System;
using System.Collections.Generic;
using Common.Logging;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Business.BusinessObjects.Domain;

namespace NDepth.Monitoring
{
    /// <summary>Monitoring interface provides functionality to store monitoring events in the storage</summary>
    public interface IMonitoring : IFlushQueueInfo
    {
        /// <summary>Machine name</summary>
        string MachineName { get; }
        /// <summary>Module name</summary>
        string ModuleName { get; }
        /// <summary>Component name</summary>
        string ComponentName { get; }

        // Register monitoring event with the given severity and title.
        void Register(Severity severity, string title, object message);
        void Register(Severity severity, string title, object message, Exception exception);
        void RegisterFormat(Severity severity, string title, string format, params object[] args);
        void RegisterFormat(Severity severity, string title, string format, Exception exception, params object[] args);
        void RegisterFormat(Severity severity, string title, IFormatProvider formatProvider, string format, params object[] args);
        void RegisterFormat(Severity severity, string title, IFormatProvider formatProvider, string format, Exception exception, params object[] args);
        void Register(Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback);
        void Register(Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback, Exception exception);
        void Register(Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback);
        void Register(Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception);

        // Register repeatable monitoring event with the given severity and title.
        void RegisterRepeat(TimeSpan timespan, Severity severity, string title, object message);
        void RegisterRepeat(TimeSpan timespan, Severity severity, string title, object message, Exception exception);
        void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, string format, params object[] args);
        void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, string format, Exception exception, params object[] args);
        void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, string format, params object[] args);
        void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, string format, Exception exception, params object[] args);
        void RegisterRepeat(TimeSpan timespan, Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback);
        void RegisterRepeat(TimeSpan timespan, Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback, Exception exception);
        void RegisterRepeat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback);
        void RegisterRepeat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception);

        /// <summary>Create monitoring interface for the given sub-component</summary>
        /// <param name="name">Sub-component name</param>
        /// <returns>Created new monitoring interface for the given sub-component</returns> 
        IMonitoring CreateMonitoring(string name);

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
