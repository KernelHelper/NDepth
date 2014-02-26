using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring.Core
{
    public class PerformanceCounterString : PerformanceCounterBase, IPerformanceCounterString
    {
        internal PerformanceCounterString(string counterName, Severity severity, IMonitoring monitoring)
            : base(false, PerformanceCounterType.StringValue, counterName, severity, monitoring)
        {
            CounterValue = string.Empty;
        }

        internal PerformanceCounterString(string counterName, Severity severity, IMonitoringState parent)
            : base(false, PerformanceCounterType.StringValue, counterName, severity, parent)
        {
            CounterValue = string.Empty;
        }

        #region String performance counter interface

        public string CounterValue { get; private set; }

        public override void Update() { }

        public string GetRawValue()
        {
            return CounterValue;
        }

        public void SetRawValue(string rawValue)
        {
            CounterValue = rawValue;
        }

        #endregion
    }
}