namespace NDepth.Monitoring
{
    /// <summary>Performance counter interface provides basic functionality to get performance counter type and properties</summary>
    public interface IPerformanceCounter : IMonitoringState
    {
        /// <summary>Is performance counter readonly?</summary>
        bool IsReadonly { get; }

        /// <summary>Performance counter type</summary>
        PerformanceCounterType CounterType { get; }

        /// <summary>Update performance counter</summary>
        void Update();
    }
}
