using System;
using System.Collections.Generic;
using Common.Logging;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Business.BusinessObjects.Domain;

namespace NDepth.Monitoring.Core
{
    internal class MonitoringForComponent : IMonitoring
    {
        private readonly MonitoringForModule _monitoring;

        public MonitoringForComponent(MonitoringForModule monitoring, string componentName)
        {
            if (monitoring == null)
                throw new ArgumentNullException("monitoring");
            if (string.IsNullOrEmpty(componentName))
                throw new ArgumentNullException("componentName");

            _monitoring = monitoring;
            ComponentName = componentName;
        }

        #region Monitoring interface

        public int FlushQueueSize { get { return _monitoring.FlushQueueSize; } }

        public string MachineName { get { return _monitoring.MachineName; } }
        public string ModuleName { get { return _monitoring.ModuleName; } }
        public string ComponentName { get; private set; }

        public void Register(Severity severity, string title, object message) { _monitoring.RegisterToQueue(new MonitoringForModule.MonitoringItem(this, severity, title, message.ToString())); }
        public void Register(Severity severity, string title, object message, Exception exception) { _monitoring.RegisterToQueue(new MonitoringForModule.MonitoringItem(this, severity, title, message.ToString(), exception)); }
        public void RegisterFormat(Severity severity, string title, string format, params object[] args) { _monitoring.RegisterToQueue(new MonitoringForModule.MonitoringItem(this, severity, title, string.Format(format, args))); }
        public void RegisterFormat(Severity severity, string title, string format, Exception exception, params object[] args) { _monitoring.RegisterToQueue(new MonitoringForModule.MonitoringItem(this, severity, title, string.Format(format, args), exception)); }
        public void RegisterFormat(Severity severity, string title, IFormatProvider formatProvider, string format, params object[] args) { _monitoring.RegisterToQueue(new MonitoringForModule.MonitoringItem(this, severity, title, string.Format(formatProvider, format, args))); }
        public void RegisterFormat(Severity severity, string title, IFormatProvider formatProvider, string format, Exception exception, params object[] args) { _monitoring.RegisterToQueue(new MonitoringForModule.MonitoringItem(this, severity, title, string.Format(formatProvider, format, args), exception)); }
        public void Register(Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback) { _monitoring.RegisterToQueue(new MonitoringForModule.MonitoringItem(this, severity, title, new MonitoringForModule.FormatMessageCallbackFormattedMessage(formatMessageCallback).ToString())); }
        public void Register(Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { _monitoring.RegisterToQueue(new MonitoringForModule.MonitoringItem(this, severity, title, new MonitoringForModule.FormatMessageCallbackFormattedMessage(formatMessageCallback).ToString(), exception)); }
        public void Register(Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback) { _monitoring.RegisterToQueue(new MonitoringForModule.MonitoringItem(this, severity, title, new MonitoringForModule.FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback).ToString())); }
        public void Register(Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { _monitoring.RegisterToQueue(new MonitoringForModule.MonitoringItem(this, severity, title, new MonitoringForModule.FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback).ToString(), exception)); }

        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, object message) { _monitoring.RegisterToCache(new MonitoringForModule.MonitoringItem(this, severity, title, message.ToString()), timespan); }
        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, object message, Exception exception) { _monitoring.RegisterToCache(new MonitoringForModule.MonitoringItem(this, severity, title, message.ToString(), exception), timespan); }
        public void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, string format, params object[] args) { _monitoring.RegisterToCache(new MonitoringForModule.MonitoringItem(this, severity, title, string.Format(format, args)), timespan); }
        public void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, string format, Exception exception, params object[] args) { _monitoring.RegisterToCache(new MonitoringForModule.MonitoringItem(this, severity, title, string.Format(format, args), exception), timespan); }
        public void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, string format, params object[] args) { _monitoring.RegisterToCache(new MonitoringForModule.MonitoringItem(this, severity, title, string.Format(formatProvider, format, args)), timespan); }
        public void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, string format, Exception exception, params object[] args) { _monitoring.RegisterToCache(new MonitoringForModule.MonitoringItem(this, severity, title, string.Format(formatProvider, format, args), exception), timespan); }
        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback) { _monitoring.RegisterToCache(new MonitoringForModule.MonitoringItem(this, severity, title, new MonitoringForModule.FormatMessageCallbackFormattedMessage(formatMessageCallback).ToString()), timespan); }
        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { _monitoring.RegisterToCache(new MonitoringForModule.MonitoringItem(this, severity, title, new MonitoringForModule.FormatMessageCallbackFormattedMessage(formatMessageCallback).ToString(), exception), timespan); }
        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback) { _monitoring.RegisterToCache(new MonitoringForModule.MonitoringItem(this, severity, title, new MonitoringForModule.FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback).ToString()), timespan); }
        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { _monitoring.RegisterToCache(new MonitoringForModule.MonitoringItem(this, severity, title, new MonitoringForModule.FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback).ToString(), exception), timespan); }

        public IMonitoring CreateMonitoring(string name)
        {
            return new MonitoringForComponent(_monitoring, name);
        }

        public IList<MonitoringEvent> Fetch(DateTime fromTimestamp, DateTime toTimestamp, string machine, string module, string component, long? pageId, int pageSize, bool pageForward)
        {
            return _monitoring.Fetch(fromTimestamp, toTimestamp, machine, module, component, pageId, pageSize, pageForward);
        }

        #endregion
    }
}
