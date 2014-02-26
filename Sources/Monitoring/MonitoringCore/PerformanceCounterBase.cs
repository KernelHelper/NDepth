using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring.Core
{
    public abstract class PerformanceCounterBase : MonitoringStateBase, IPerformanceCounter
    {
        internal PerformanceCounterBase(bool isReadonly, PerformanceCounterType type, string name, Severity severity, IMonitoring monitoring)
            : base(name, severity, monitoring)
        {
            IsReadonly = isReadonly;
            CounterType = type;
        }

        internal PerformanceCounterBase(bool isReadonly, PerformanceCounterType type, string name, Severity severity, IMonitoringState parent) 
            : base(name, severity, parent)
        {
            IsReadonly = isReadonly;
            CounterType = type;
        }

        #region Performance counter interface

        public bool IsReadonly { get; private set; }

        public PerformanceCounterType CounterType { get; private set; }

        public abstract void Update();

        #endregion
    }
}