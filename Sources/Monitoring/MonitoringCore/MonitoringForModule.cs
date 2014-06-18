using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Business.BusinessObjects.Domain;
using NDepth.Common.Concurrent;
using NDepth.Module;

namespace NDepth.Monitoring.Core
{
    internal class MonitoringForModule : IMonitoring, IDisposable
    {
        private static int _asyncMonitoringCount;

        private readonly IMonitoringStorage _storage;
        private readonly IMonitoringNotify _notify;
        private readonly IMonitoringNotifyEmail _notifyEmail;
        private readonly IMonitoringNotifySms _notifySms;

        public MonitoringForModule(string machineName, string moduleName)
        {
            if (string.IsNullOrEmpty(machineName))
                throw new ArgumentNullException("machineName");
            if (string.IsNullOrEmpty(moduleName))
                throw new ArgumentNullException("moduleName");

            MachineName = machineName;
            ModuleName = moduleName;
            ComponentName = moduleName;

            // Get monitoring storage and notifies.
            _storage = ModuleContainer.Container.GetInstance<IMonitoringStorage>();
            _notify = ModuleContainer.Container.GetInstance<IMonitoringNotify>();
            _notifyEmail = ModuleContainer.Container.GetInstance<IMonitoringNotifyEmail>();
            _notifySms = ModuleContainer.Container.GetInstance<IMonitoringNotifySms>();

            // Prepare monitoring name.
            var name = "Monitoring-" + Interlocked.Increment(ref _asyncMonitoringCount);

            // Create and start monitoring processing queue.
            _monitoringQueue = new AsyncQueueBlocking<MonitoringItem>(name, BlockType.Grow);
            _monitoringQueue.HandleItem += Store;
            _monitoringQueue.StartProcessing();
        }

        #region Monitoring interface

        public int FlushQueueSize { get { return _monitoringQueue.AsyncQueueSize; } }

        public string MachineName { get; private set; }
        public string ModuleName { get; private set; }
        public string ComponentName { get; private set; }

        public void Register(Severity severity, string title, object message) { RegisterToQueue(new MonitoringItem(this, severity, title, message.ToString())); }
        public void Register(Severity severity, string title, object message, Exception exception) { RegisterToQueue(new MonitoringItem(this, severity, title, message.ToString(), exception)); }
        public void RegisterFormat(Severity severity, string title, string format, params object[] args) { RegisterToQueue(new MonitoringItem(this, severity, title, string.Format(format, args))); }
        public void RegisterFormat(Severity severity, string title, string format, Exception exception, params object[] args) { RegisterToQueue(new MonitoringItem(this, severity, title, string.Format(format, args), exception)); }
        public void RegisterFormat(Severity severity, string title, IFormatProvider formatProvider, string format, params object[] args) { RegisterToQueue(new MonitoringItem(this, severity, title, string.Format(formatProvider, format, args))); }
        public void RegisterFormat(Severity severity, string title, IFormatProvider formatProvider, string format, Exception exception, params object[] args) { RegisterToQueue(new MonitoringItem(this, severity, title, string.Format(formatProvider, format, args), exception)); }
        public void Register(Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback) { RegisterToQueue(new MonitoringItem(this, severity, title, new FormatMessageCallbackFormattedMessage(formatMessageCallback).ToString())); }
        public void Register(Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { RegisterToQueue(new MonitoringItem(this, severity, title, new FormatMessageCallbackFormattedMessage(formatMessageCallback).ToString(), exception)); }
        public void Register(Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback) { RegisterToQueue(new MonitoringItem(this, severity, title, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback).ToString())); }
        public void Register(Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { RegisterToQueue(new MonitoringItem(this, severity, title, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback).ToString(), exception)); }

        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, object message) { RegisterToCache(new MonitoringItem(this, severity, title, message.ToString()), timespan); }
        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, object message, Exception exception) { RegisterToCache(new MonitoringItem(this, severity, title, message.ToString(), exception), timespan); }
        public void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, string format, params object[] args) { RegisterToCache(new MonitoringItem(this, severity, title, string.Format(format, args)), timespan); }
        public void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, string format, Exception exception, params object[] args) { RegisterToCache(new MonitoringItem(this, severity, title, string.Format(format, args), exception), timespan); }
        public void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, string format, params object[] args) { RegisterToCache(new MonitoringItem(this, severity, title, string.Format(formatProvider, format, args)), timespan); }
        public void RegisterRepeatFormat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, string format, Exception exception, params object[] args) { RegisterToCache(new MonitoringItem(this, severity, title, string.Format(formatProvider, format, args), exception), timespan); }
        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback) { RegisterToCache(new MonitoringItem(this, severity, title, new FormatMessageCallbackFormattedMessage(formatMessageCallback).ToString()), timespan); }
        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { RegisterToCache(new MonitoringItem(this, severity, title, new FormatMessageCallbackFormattedMessage(formatMessageCallback).ToString(), exception), timespan); }
        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback) { RegisterToCache(new MonitoringItem(this, severity, title, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback).ToString()), timespan); }
        public void RegisterRepeat(TimeSpan timespan, Severity severity, string title, IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception) { RegisterToCache(new MonitoringItem(this, severity, title, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback).ToString(), exception), timespan); }

        public IMonitoring CreateMonitoring(string name)
        {
            return new MonitoringForComponent(this, name);
        }

        public IList<MonitoringEvent> Fetch(DateTime fromTimestamp, DateTime toTimestamp, string machine, string module, string component, long? pageId, int pageSize, bool pageForward)
        {
            return _storage.Fetch(fromTimestamp, toTimestamp, machine, module, component, pageId, pageSize, pageForward);
        }

        #endregion

        #region Monitoring queue

        internal class MonitoringItem
        {
            public readonly string Key;

            public DateTime Timestamp;
            public string MachineName;
            public string ModuleName;
            public string ComponentName;
            public Severity Severity;
            public string Title;
            public string Description;
            public Exception Exception;

            public int Count;
            public TimeSpan? Timespan;

            public MonitoringItem(IMonitoring monitoring, Severity severity, string title, string description, Exception exception = null)
            {
                Timestamp = DateTime.UtcNow;
                MachineName = monitoring.MachineName;
                ModuleName = monitoring.ModuleName;
                ComponentName = monitoring.ComponentName;
                Severity = severity;
                Title = title;
                Description = description;
                Exception = exception;

                Count = 0;

                Key = MachineName + ":" + ModuleName + ":" + ComponentName + ":" + title;
            }

            public MonitoringItem(MonitoringItem item)
            {
                Key = item.Key;

                Timestamp = item.Timestamp;
                MachineName = item.MachineName;
                ModuleName = item.ModuleName;
                ComponentName = item.ComponentName;
                Severity = item.Severity;
                Title = item.Title;
                Description = item.Description;
                Exception = item.Exception;

                Count = item.Count;
                Timespan = item.Timespan;
            }

            public void Update(MonitoringItem item)
            {
                Timestamp = item.Timestamp;
                MachineName = item.MachineName;
                ModuleName = item.ModuleName;
                ComponentName = item.ComponentName;
                Severity = item.Severity;
                Title = item.Title;
                Description = item.Description;
                Exception = item.Exception;

                Count++;
            }
        }

        // Monitoring queue resources.
        private readonly object _monitoringLock = new object();
        private readonly CancellationTokenSource _monitoringCancellationTokenSource = new CancellationTokenSource();
        private readonly Dictionary<string, MonitoringItem> _monitoringCache = new Dictionary<string, MonitoringItem>();
        private readonly AsyncQueueBlocking<MonitoringItem> _monitoringQueue;

        internal void RegisterToCache(MonitoringItem item, TimeSpan timespan)
        {
            // Lock monitoring queue resources.
            lock (_monitoringLock)
            {
                // Check disposed flag.
                if (_disposed)
                    return;

                MonitoringItem current;
                if (_monitoringCache.TryGetValue(item.Key, out current))
                {
                    // UpdateState item in the monitoring cache.
                    current.Update(item);
                }
                else
                {
                    // Add item to the monitoring queue.
                    _monitoringQueue.AddItem(item);

                    // Create clone monitoring item and setup its parameters.
                    var clone = new MonitoringItem(item) { Timespan = timespan };

                    // Create delayed task to check monitoring cache.
                    Task.Delay(timespan, _monitoringCancellationTokenSource.Token).ContinueWith(task => RegisterCacheToQueue(clone), _monitoringCancellationTokenSource.Token);

                    // Add clone item to the monitoring cache.
                    _monitoringCache.Add(clone.Key, clone);
                }
            }
        }

        private void RegisterCacheToQueue(MonitoringItem item)
        {
            // Lock monitoring queue resources.
            lock (_monitoringLock)
            {
                // Check disposed flag.
                if (_disposed)
                    return;

                MonitoringItem current;
                if (_monitoringCache.TryGetValue(item.Key, out current))
                {
                    if (current.Count > 0)
                    {
                        // Create clone monitoring item.
                        var clone = new MonitoringItem(current);

                        // Add clone item to the monitoring queue.
                        _monitoringQueue.AddItem(clone);

                        // Reset monitoring item in cache.
                        current.Count = 0;

                        // Check for delayed task.
                        if (current.Timespan.HasValue)
                        {
                            // Recreate delayed task to check monitoring cache.
                            Task.Delay(current.Timespan.Value, _monitoringCancellationTokenSource.Token).ContinueWith(task => RegisterCacheToQueue(current), _monitoringCancellationTokenSource.Token);
                        }
                        else
                        {
                            // Remove item from the monitoring cache.
                            _monitoringCache.Remove(item.Key);                                                    
                        }
                    }
                    else
                    {
                        // Remove item from the monitoring cache.
                        _monitoringCache.Remove(item.Key);                        
                    }
                }
            }
        }

        internal void RegisterToQueue(MonitoringItem item)
        {
            // Lock monitoring queue resources.
            lock (_monitoringLock)
            {
                // Check disposed flag.
                if (_disposed)
                    return;

                // Add item to the monitoring queue.
                _monitoringQueue.AddItem(item);
            }
        }

        #endregion

        #region Storage

        private void Store(MonitoringItem item)
        {
            var times = (item.Timespan.HasValue && (item.Count > 0)) ? string.Format(Resources.Strings.MonitoringTimesInSecondsFormat, item.Count, item.Timespan.Value) : string.Empty;
            var exception = (item.Exception != null) ? Environment.NewLine + item.Exception : string.Empty;
            var description = item.Description + times + exception;

            // Store monitoring event.
            _storage.Store(item.Timestamp, item.MachineName, item.ModuleName, item.ComponentName, item.Severity, item.Title, description);

            // Notify monitoring event.
            if ((item.Severity >= Severity.Notify) && (item.Severity < Severity.NotifyWithEmail))
            {
                _notify.Notify(item.Timestamp, item.MachineName, item.ModuleName, item.ComponentName, item.Severity, item.Title, description);
            }
            else if (((item.Severity >= Severity.ErrorWithEmail) && (item.Severity < Severity.ErrorWithSms)) || ((item.Severity >= Severity.NotifyWithEmail) && (item.Severity < Severity.NotifyWithSms)))
            {
                _notify.Notify(item.Timestamp, item.MachineName, item.ModuleName, item.ComponentName, item.Severity, item.Title, description);
                _notifyEmail.NotifyWithEmail(item.Timestamp, item.MachineName, item.ModuleName, item.ComponentName, item.Severity, item.Title, description);
            }
            else if (((item.Severity >= Severity.ErrorWithSms) && (item.Severity < Severity.Fatal)) || (item.Severity >= Severity.NotifyWithSms))
            {
                _notify.Notify(item.Timestamp, item.MachineName, item.ModuleName, item.ComponentName, item.Severity, item.Title, description);
                _notifySms.NotifyWithSms(item.Timestamp, item.MachineName, item.ModuleName, item.ComponentName, item.Severity, item.Title, description);
            }
            else if ((item.Severity >= Severity.Fatal) && (item.Severity < Severity.Notify))
            {
                _notify.Notify(item.Timestamp, item.MachineName, item.ModuleName, item.ComponentName, item.Severity, item.Title, description);
                _notifyEmail.NotifyWithEmail(item.Timestamp, item.MachineName, item.ModuleName, item.ComponentName, item.Severity, item.Title, description);
                _notifySms.NotifyWithSms(item.Timestamp, item.MachineName, item.ModuleName, item.ComponentName, item.Severity, item.Title, description);
            }
        }

        #endregion 

        #region IDisposable implementation

        // Dispose flags.
        private bool _disposed;

        // Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposingManagedResources)
        {
            // Lock monitoring queue resources.
            lock (_monitoringLock)
            {
                if (!_disposed)
                {
                    if (disposingManagedResources)
                    {
                        // Perform cancellation.
                        _monitoringCancellationTokenSource.Cancel();

                        // Move all valid items from the monitoring cache to the monitoring queue.
                        foreach (var item in _monitoringCache.Values.Where(item => item.Count > 0))
                            _monitoringQueue.AddItem(item);

                        // Clear the monitoring cache.
                        _monitoringCache.Clear();

                        // Dispose managed resources here...
                        _monitoringQueue.Dispose();
                        _monitoringCancellationTokenSource.Dispose();
                    }

                    // Dispose unmanaged resources here...

                    // Set large fields to null here...

                    // Mark as disposed.
                    _disposed = true;
                }
            }
        }

        // Use C# destructor syntax for finalization code.
        ~MonitoringForModule()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion

        #region Utilities

        internal class FormatMessageCallbackFormattedMessage
        {
            private volatile string _cachedMessage;
            private readonly IFormatProvider _formatProvider;
            private readonly Action<FormatMessageHandler> _formatMessageCallback;

            public FormatMessageCallbackFormattedMessage(Action<FormatMessageHandler> formatMessageCallback)
            {
                _formatMessageCallback = formatMessageCallback;
            }

            public FormatMessageCallbackFormattedMessage(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
            {
                _formatProvider = formatProvider;
                _formatMessageCallback = formatMessageCallback;
            }

            public override string ToString()
            {
                if ((_cachedMessage == null) && (_formatMessageCallback != null))
                    _formatMessageCallback(FormatMessage);
                return _cachedMessage ?? string.Empty;
            }

            private string FormatMessage(string format, params object[] args)
            {
                _cachedMessage = string.Format(_formatProvider, format, args);
                return _cachedMessage;
            }
        }

        #endregion
    }
}
