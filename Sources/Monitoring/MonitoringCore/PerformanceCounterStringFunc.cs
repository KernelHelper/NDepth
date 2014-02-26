using System;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring.Core
{
    public class PerformanceCounterStringFunc : PerformanceCounterBase, IPerformanceCounterString
    {
        private readonly Func<string> _counterValueFunc;

        internal PerformanceCounterStringFunc(string counterName, Func<string> counterValueFunc, Severity severity, IMonitoring monitoring)
            : base(true, PerformanceCounterType.StringValue, counterName, severity, monitoring)
        {
            _counterValueFunc = counterValueFunc ?? (() => string.Empty);
            UpdateInternal();
        }

        internal PerformanceCounterStringFunc(string counterName, Func<string> counterValueFunc, Severity severity, IMonitoringState parent)
            : base(true, PerformanceCounterType.StringValue, counterName, severity, parent)
        {
            _counterValueFunc = counterValueFunc ?? (() => string.Empty);
            UpdateInternal();
        }

        #region String performance counter interface

        public string CounterValue { get; private set; }

        public override void Update()
        {
            UpdateInternal();
        }

        public string GetRawValue()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void SetRawValue(string rawValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        #endregion

        #region Utility methods

        private void UpdateInternal()
        {
            // Update state of the performance counter value.
            CounterValue = _counterValueFunc();            
        }

        internal override void UpdateState()
        {
            // Call the base method.
            base.UpdateState();

            UpdateInternal();
        }

        #endregion
    }
}