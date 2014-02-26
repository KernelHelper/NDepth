using System;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring.Core
{
    public class PerformanceCounterNumeric : PerformanceCounterBase, IPerformanceCounterNumeric
    {
        private readonly bool _supportBase;
        private float _counterValue;
        private float _counterDelta;
        private float _counterBaseValue = 1.0f;
        private DateTime _timestamp = DateTime.MinValue;

        internal PerformanceCounterNumeric(string counterName, PerformanceCounterType counterType, Severity severity, IMonitoring monitoring)
            : base(false, counterType, counterName, severity, monitoring)
        {
            // Setup support base value flag.
            switch (counterType)
            {
                case PerformanceCounterType.AverageValue:
                case PerformanceCounterType.PercentValue:
                    _supportBase = true;
                    break;
            }
            UpdateInternal();
        }

        internal PerformanceCounterNumeric(string counterName, PerformanceCounterType counterType, Severity severity, IMonitoringState parent)
            : base(false, counterType, counterName, severity, parent)
        {
            // Setup support base value flag.
            switch (counterType)
            {
                case PerformanceCounterType.AverageValue:
                case PerformanceCounterType.PercentValue:
                    _supportBase = true;
                    break;
            }
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
            return _counterValue;
        }

        public void SetRawValue(float rawValue)
        {
            switch (CounterType)
            {
                case PerformanceCounterType.CountOfItems:
                    _counterValue = rawValue;
                    break;
                case PerformanceCounterType.CountPerSecond:
                    _timestamp = TimestampSeconds();
                    _counterValue = rawValue;
                    break;
                case PerformanceCounterType.AverageValue:
                    _counterValue = rawValue;
                    break;
                case PerformanceCounterType.DeltaValue:
                    _counterValue = rawValue;
                    _counterDelta = rawValue;
                    break;
                case PerformanceCounterType.PercentValue:
                    _counterValue = rawValue;
                    break;
                case PerformanceCounterType.ElapsedTime:
                    throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterInvalidOperation, ComponentName, CounterType));
            }
        }

        public void SetRawValue(DateTime rawValue)
        {
            switch (CounterType)
            {
                case PerformanceCounterType.CountOfItems:
                case PerformanceCounterType.CountPerSecond:
                case PerformanceCounterType.AverageValue:
                case PerformanceCounterType.DeltaValue:
                case PerformanceCounterType.PercentValue:
                    throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterInvalidOperation, ComponentName, CounterType));
                case PerformanceCounterType.ElapsedTime:
                    _counterValue = rawValue.Ticks;
                    _timestamp = rawValue;
                    break;                    
            }
        }

        public void Increment()
        {
            switch (CounterType)
            {
                case PerformanceCounterType.CountOfItems:
                    _counterValue++;
                    break;
                case PerformanceCounterType.CountPerSecond:
                    DateTime now = TimestampSeconds();
                    if (_timestamp != now)
                    {
                        _timestamp = now;
                        _counterValue = 0;
                    }
                    _counterValue++;
                    if (_counterValue > 1000)
                    {
                        _counterValue = 0;
                    }
                    break;
                case PerformanceCounterType.AverageValue:
                    _counterValue++;
                    break;
                case PerformanceCounterType.DeltaValue:
                    _counterValue++;
                    _counterDelta = 1.0f;
                    break;
                case PerformanceCounterType.PercentValue:
                    _counterValue++;
                    break;
                case PerformanceCounterType.ElapsedTime:
                    throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterInvalidOperation, ComponentName, CounterType));
            }
        }

        public void IncrementBy(float byValue)
        {
            switch (CounterType)
            {
                case PerformanceCounterType.CountOfItems:
                    _counterValue += byValue;
                    break;
                case PerformanceCounterType.CountPerSecond:
                    DateTime now = TimestampSeconds();
                    if (_timestamp != now)
                    {
                        _timestamp = now;
                        _counterValue = 0;
                    }
                    _counterValue += byValue;
                    break;
                case PerformanceCounterType.AverageValue:
                    _counterValue += byValue;
                    break;
                case PerformanceCounterType.DeltaValue:
                    _counterValue += byValue;
                    _counterDelta = byValue;
                    break;
                case PerformanceCounterType.PercentValue:
                    _counterValue += byValue;
                    break;
                case PerformanceCounterType.ElapsedTime:
                    throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterInvalidOperation, ComponentName, CounterType));
            }
        }

        public void Decrement()
        {
            switch (CounterType)
            {
                case PerformanceCounterType.CountOfItems:
                    _counterValue--;
                    break;
                case PerformanceCounterType.CountPerSecond:
                    DateTime now = TimestampSeconds();
                    if (_timestamp != now)
                    {
                        _timestamp = now;
                        _counterValue = 0;
                    }
                    _counterValue--;
                    break;
                case PerformanceCounterType.AverageValue:
                    _counterValue--;
                    break;
                case PerformanceCounterType.DeltaValue:
                    _counterValue--;
                    _counterDelta = -1.0f;
                    break;
                case PerformanceCounterType.PercentValue:
                    _counterValue--;
                    break;
                case PerformanceCounterType.ElapsedTime:
                    throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterInvalidOperation, ComponentName, CounterType));
            }
        }

        public void DecrementBy(float byValue)
        {
            switch (CounterType)
            {
                case PerformanceCounterType.CountOfItems:
                    _counterValue -= byValue;
                    break;
                case PerformanceCounterType.CountPerSecond:
                    DateTime now = TimestampSeconds();
                    if (_timestamp != now)
                    {
                        _timestamp = now;
                        _counterValue = 0;
                    }
                    _counterValue -= byValue;
                    break;
                case PerformanceCounterType.AverageValue:
                    _counterValue -= byValue;
                    break;
                case PerformanceCounterType.DeltaValue:
                    _counterValue -= byValue;
                    _counterDelta = -byValue;
                    break;
                case PerformanceCounterType.PercentValue:
                    _counterValue -= byValue;
                    break;
                case PerformanceCounterType.ElapsedTime:
                    throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterInvalidOperation, ComponentName, CounterType));
            }
        }

        public float GetBaseRawValue()
        {
            if (!_supportBase)
                throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterWithoutBase, ComponentName, CounterType));

            return _counterBaseValue;
        }

        public void SetBaseRawValue(float rawValue)
        {
            if (!_supportBase)
                throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterWithoutBase, ComponentName, CounterType));

            _counterBaseValue = rawValue;
        }

        public void IncrementBase()
        {
            if (!_supportBase)
                throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterWithoutBase, ComponentName, CounterType));

            _counterBaseValue++;
        }

        public void IncrementBaseBy(float byValue)
        {
            if (!_supportBase)
                throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterWithoutBase, ComponentName, CounterType));

            _counterBaseValue += byValue;
        }

        public void DecrementBase()
        {
            if (!_supportBase)
                throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterWithoutBase, ComponentName, CounterType));

            _counterBaseValue--;
        }

        public void DecrementBaseBy(float byValue)
        {
            if (!_supportBase)
                throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterWithoutBase, ComponentName, CounterType));

            _counterBaseValue -= byValue;
        }

        #endregion

        #region Utility methods

        private void UpdateInternal()
        {
            // Update state of the performance counter value.
            switch (CounterType)
            {
                case PerformanceCounterType.CountOfItems:
                    CounterValue = _counterValue;
                    break;
                case PerformanceCounterType.CountPerSecond:
                    CounterValue = _counterValue;
                    break;
                case PerformanceCounterType.AverageValue:
                    CounterValue = _counterValue / _counterBaseValue;
                    break;
                case PerformanceCounterType.DeltaValue:
                    CounterValue = _counterDelta;
                    break;
                case PerformanceCounterType.PercentValue:
                    CounterValue = (_counterValue / _counterBaseValue) * 100.0f;
                    break;
                case PerformanceCounterType.ElapsedTime:
                    CounterValue = (float)(DateTime.UtcNow - _timestamp).TotalSeconds;
                    break;
            }                        
        }

        internal override void UpdateState()
        {
            // Call the base method.
            base.UpdateState();

            UpdateInternal();
        }

        private DateTime TimestampSeconds()
        {
            var utcnow = DateTime.UtcNow;
            return new DateTime(utcnow.Year, utcnow.Month, utcnow.Day, utcnow.Hour, utcnow.Minute, utcnow.Second, DateTimeKind.Utc);
        }

        #endregion
    }
}