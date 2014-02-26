using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Common.Logging;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Common.DisposableLock;

namespace NDepth.Monitoring.Core
{
    public class MonitoringStateBase : IMonitoringState
    {
        protected ReaderWriterLockSlim Lock { get; private set; }

        private IMonitoringState _parent;
        private Severity _severity;

        private readonly List<IMonitoringState> _child = new List<IMonitoringState>();
        private readonly List<MonitoringStateComponent> _childComponents = new List<MonitoringStateComponent>();
        private readonly List<PerformanceCounterBase> _childCounters = new List<PerformanceCounterBase>();

        private MonitoringStateBase(string name, Severity severity)
        {
            Lock = new ReaderWriterLockSlim();

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            // Setup severity.
            _severity = severity;

            ComponentName = name;
            Logger = LogManager.GetLogger(name);
        }

        protected MonitoringStateBase(string name, Severity severity, IMonitoring monitoring)
            : this(name, severity)
        {
            if (monitoring == null)
                throw new ArgumentNullException("monitoring");

            // Setup parent component.
            _parent = null;

            Monitoring = monitoring;
        }

        protected MonitoringStateBase(string name, Severity severity, IMonitoringState parent)
            : this(name, severity)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            // Setup parent component.
            _parent = parent;

            Monitoring = parent.Monitoring.CreateMonitoring(name);
        }

        #region Monitoring state interface

        public string MachineName { get { return Monitoring.MachineName; } }
        public string ModuleName { get { return Monitoring.ModuleName; } }
        public string ComponentName { get; private set; }

        public Severity Severity { get { using (new ReadLock(Lock)) { return _severity; } } set { using (new WriteLock(Lock)) { _severity = value; } } }

        public ILog Logger { get; private set; }
        public IMonitoring Monitoring { get; private set; }

        public IMonitoringState Parent { get { using (new ReadLock(Lock)) { return _parent; } } }
        public IEnumerable<IMonitoringState> Child { get { using (new ReadLock(Lock)) { return new List<IMonitoringState>(_child); } } }

        public IMonitoringStateComponent AttachComponent(string name)
        {
            var state = new MonitoringStateComponent(name, Severity.None, this);

            // Add new monitoring state component into the child collection.
            using (new WriteLock(Lock))
            {
                _child.Add(state);
                _childComponents.Add(state);
            }

            return state;
        }

        public IMonitoringStateComponent AttachComponent(string name, Severity severity)
        {
            var state = new MonitoringStateComponent(name, severity, this);

            // Add new monitoring state component into the child collection.
            using (new WriteLock(Lock))
            {
                _child.Add(state);
                _childComponents.Add(state);
            }

            return state;
        }

        public IPerformanceCounterNumeric AttachNumericCounter(string counterName, PerformanceCounterType counterType = PerformanceCounterType.CountOfItems)
        {
            // Check numeric counter type.
            switch (counterType)
            {
                case PerformanceCounterType.CountOfItems:
                case PerformanceCounterType.CountPerSecond:
                case PerformanceCounterType.AverageValue:
                case PerformanceCounterType.DeltaValue:
                case PerformanceCounterType.PercentValue:
                case PerformanceCounterType.ElapsedTime:
                    break;
                default:
                    throw new ArgumentException(string.Format(Resources.Strings.InvalidNumericCounterType, counterType));
            }

            var counter = new PerformanceCounterNumeric(counterName, counterType, Severity.None, this);

            // Add new performance counter into the child collection.
            using (new WriteLock(Lock))
            {
                _child.Add(counter);
                _childCounters.Add(counter);
            }

            return counter;                        
        }

        public IPerformanceCounterNumeric AttachNumericCounter(string counterName, float counterValue, PerformanceCounterType counterType = PerformanceCounterType.CountOfItems)
        {
            // Check numeric counter type.
            switch (counterType)
            {
                case PerformanceCounterType.CountOfItems:
                case PerformanceCounterType.CountPerSecond:
                case PerformanceCounterType.AverageValue:
                case PerformanceCounterType.DeltaValue:
                case PerformanceCounterType.PercentValue:
                case PerformanceCounterType.ElapsedTime:
                    break;
                default:
                    throw new ArgumentException(string.Format(Resources.Strings.InvalidNumericCounterType, counterType));
            }

            var counter = new PerformanceCounterNumericFunc(counterName, () => counterValue, counterType, Severity.None, this);

            // Add new performance counter into the child collection.
            using (new WriteLock(Lock))
            {
                _child.Add(counter);
                _childCounters.Add(counter);
            }

            return counter;                        
        }

        public IPerformanceCounterNumeric AttachNumericCounter(string counterName, Func<float> counterValueFunc, PerformanceCounterType counterType = PerformanceCounterType.CountOfItems)
        {
            // Check numeric counter type.
            switch (counterType)
            {
                case PerformanceCounterType.CountOfItems:
                case PerformanceCounterType.CountPerSecond:
                case PerformanceCounterType.AverageValue:
                case PerformanceCounterType.DeltaValue:
                case PerformanceCounterType.PercentValue:
                case PerformanceCounterType.ElapsedTime:
                    break;
                default:
                    throw new ArgumentException(string.Format(Resources.Strings.InvalidNumericCounterType, counterType));
            }

            var counter = new PerformanceCounterNumericFunc(counterName, counterValueFunc, counterType, Severity.None, this);

            // Add new performance counter into the child collection.
            using (new WriteLock(Lock))
            {
                _child.Add(counter);
                _childCounters.Add(counter);
            }

            return counter;
        }

        public IPerformanceCounterString AttachStringCounter(string counterName)
        {
            var counter = new PerformanceCounterString(counterName, Severity.None, this);

            // Add new performance counter into the child collection.
            using (new WriteLock(Lock))
            {
                _child.Add(counter);
                _childCounters.Add(counter);
            }

            return counter;                        
        }

        public IPerformanceCounterString AttachStringCounter(string counterName, string counterValue)
        {
            var counter = new PerformanceCounterStringFunc(counterName, () => counterValue, Severity.None, this);

            // Add new performance counter into the child collection.
            using (new WriteLock(Lock))
            {
                _child.Add(counter);
                _childCounters.Add(counter);
            }

            return counter;
        }

        public IPerformanceCounterString AttachStringCounter(string counterName, Func<string> counterValueFunc)
        {
            var counter = new PerformanceCounterStringFunc(counterName, counterValueFunc, Severity.None, this);

            // Add new performance counter into the child collection.
            using (new WriteLock(Lock))
            {
                _child.Add(counter);
                _childCounters.Add(counter);
            }

            return counter;                                    
        }

        public IPerformanceCounterNumeric AttachSystemCounter(string categoryName, string counterName)
        {
            var counter = new PerformanceCounterSystem(new PerformanceCounter(categoryName, counterName), Severity.None, this);

            // Add new performance counter into the child collection.
            using (new WriteLock(Lock))
            {
                _child.Add(counter);
                _childCounters.Add(counter);
            }

            return counter;            
        }

        public IPerformanceCounterNumeric AttachSystemCounter(string categoryName, string counterName, string instanceName)
        {
            var counter = new PerformanceCounterSystem(new PerformanceCounter(categoryName, counterName, instanceName), Severity.None, this);

            // Add new performance counter into the child collection.
            using (new WriteLock(Lock))
            {
                _child.Add(counter);
                _childCounters.Add(counter);
            }

            return counter;                        
        }

        public IPerformanceCounterNumeric AttachSystemCounter(string machineName, string categoryName, string counterName, string instanceName)
        {
            var counter = new PerformanceCounterSystem(new PerformanceCounter(machineName, categoryName, counterName, instanceName), Severity.None, this);

            // Add new performance counter into the child collection.
            using (new WriteLock(Lock))
            {
                _child.Add(counter);
                _childCounters.Add(counter);
            }

            return counter;                                    
        }

        public void RemoveFromMonitoring()
        {
            var parent = _parent as MonitoringStateBase;
            if (parent != null)
            {
                // Delete the current component in parent child collections.
                using (new WriteLock(parent.Lock))
                {
                    parent._child.Remove(this);

                    var component = this as MonitoringStateComponent;
                    if (component != null)
                        parent._childComponents.Remove(component);

                    var counter = this as PerformanceCounterBase;
                    if (counter != null)
                        parent._childCounters.Remove(counter);
                }
            }

            // Reset the parent reference.
            _parent = null;
        }

        #endregion

        #region Utility methods

        internal virtual void UpdateState()
        {
            // Update state of all children in the monitoring hierarchy.
            using (new WriteLock(Lock))
            {
                // Update state of all components.
                foreach (var component in _childComponents)
                    component.UpdateState();

                // Update state of all performance counters.
                foreach (var counter in _childCounters)
                    counter.UpdateState();
            }
        }

        #endregion
    }
}