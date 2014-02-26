using System;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring.Core
{
    public class PerformanceCounterNumericFunc : PerformanceCounterBase, IPerformanceCounterNumeric
    {
        private readonly Func<float> _counterValueFunc;

        internal PerformanceCounterNumericFunc(string counterName, Func<float> counterValueFunc, PerformanceCounterType counterType, Severity severity, IMonitoring monitoring)
            : base(true, counterType, counterName, severity, monitoring)
        {
            _counterValueFunc = counterValueFunc ?? (() => 0.0f);
            UpdateInternal();
        }

        internal PerformanceCounterNumericFunc(string counterName, Func<float> counterValueFunc, PerformanceCounterType counterType, Severity severity, IMonitoringState parent)
            : base(true, counterType, counterName, severity, parent)
        {
            _counterValueFunc = counterValueFunc ?? (() => 0.0f);
            UpdateInternal();
        }

        #region Numeric performance counter interface

        public float CounterValue { get; private set; }

        public override void Update()
        {
            UpdateInternal();
        }

        public float GetRawValue()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void SetRawValue(float rawValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void SetRawValue(DateTime rawValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void Increment()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void IncrementBy(float byValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void Decrement()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));   
        }

        public void DecrementBy(float byValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public float GetBaseRawValue()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void SetBaseRawValue(float rawValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void IncrementBase()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void IncrementBaseBy(float byValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void DecrementBase()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void DecrementBaseBy(float byValue)
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