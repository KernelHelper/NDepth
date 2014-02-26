using System;

namespace NDepth.Monitoring
{
    /// <summary>Numeric performance counter interface provides functionality to get the value of numeric performance counters</summary>
    public interface IPerformanceCounterNumeric : IPerformanceCounter
    {
        /// <summary>Performance counter value</summary>
        float CounterValue { get; }

        /// <summary>Get raw value of the performance counter</summary>
        /// <returns>Raw value of the performance counter</returns>
        float GetRawValue();
        /// <summary>Set raw value of the performance counter</summary>
        /// <param name="rawValue">Raw value of the performance counter</param>
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly</exception>
        void SetRawValue(float rawValue);
        /// <summary>Set raw value of the performance counter</summary>
        /// <param name="rawValue">Raw value of the performance counter</param>
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly</exception>
        void SetRawValue(DateTime rawValue);

        /// <summary>Increment performance counter</summary>
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly</exception>
        void Increment();
        /// <summary>Increment performance counter by the given value</summary>
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly</exception>
        void IncrementBy(float byValue);
        /// <summary>Decrement performance counter</summary>
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly</exception>
        void Decrement();
        /// <summary>Decrement performance counter by the given value</summary>
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly</exception>
        void DecrementBy(float byValue);

        /// <summary>Get base raw value of the performance counter</summary>
        /// <remarks>Only supported for performance counter with types: PerformanceCounterType.AverageValue, PerformanceCounterType.PercentValue</remarks> 
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter type does not support base value</exception>
        /// <returns>Base raw value of the performance counter</returns>
        float GetBaseRawValue();
        /// <summary>Set base raw value of the performance counter</summary>
        /// <remarks>Only supported for performance counter with types: PerformanceCounterType.AverageValue, PerformanceCounterType.PercentValue</remarks> 
        /// <param name="rawValue">Base raw value of the performance counter</param>
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly or does not support base value</exception>
        void SetBaseRawValue(float rawValue);

        /// <summary>Increment performance counter base</summary>
        /// <remarks>Only supported for performance counter with types: PerformanceCounterType.AverageValue, PerformanceCounterType.PercentValue</remarks> 
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly or does not support base value</exception>
        void IncrementBase();
        /// <summary>Increment performance counter base by the given value</summary>
        /// <remarks>Only supported for performance counter with types: PerformanceCounterType.AverageValue, PerformanceCounterType.PercentValue</remarks> 
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly or does not support base value</exception>
        void IncrementBaseBy(float byValue);
        /// <summary>Decrement performance counter base</summary>
        /// <remarks>Only supported for performance counter with types: PerformanceCounterType.AverageValue, PerformanceCounterType.PercentValue</remarks> 
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly or does not support base value</exception>
        void DecrementBase();
        /// <summary>Decrement performance counter base by the given value</summary>
        /// <remarks>Only supported for performance counter with types: PerformanceCounterType.AverageValue, PerformanceCounterType.PercentValue</remarks> 
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly</exception>
        void DecrementBaseBy(float byValue);
    }
}
