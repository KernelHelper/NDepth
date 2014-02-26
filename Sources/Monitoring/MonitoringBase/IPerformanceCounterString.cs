namespace NDepth.Monitoring
{
    /// <summary>String performance counter interface provides functionality to get the value of custom string performance counters</summary>
    public interface IPerformanceCounterString : IPerformanceCounter
    {
        /// <summary>Performance counter value</summary>
        string CounterValue { get; }

        /// <summary>Get raw value of the performance counter</summary>
        /// <returns>Raw value of the performance counter</returns>
        string GetRawValue();
        /// <summary>Set raw value of the performance counter</summary>
        /// <param name="rawValue">Raw value of the performance counter</param>
        /// <exception cref="System.InvalidOperationException">Thrown when performance counter is readonly</exception>
        void SetRawValue(string rawValue);
    }
}
