namespace NDepth.Monitoring
{
    /// <summary>Performance counter type</summary>
    public enum PerformanceCounterType
    {
        /// <summary>Unknown</summary>
        Unknown,
        /// <summary>Count of items</summary>
        CountOfItems,
        /// <summary>Count per second</summary>
        CountPerSecond,
        /// <summary>Average value</summary>
        AverageValue,
        /// <summary>Delta value</summary>
        DeltaValue,
        /// <summary>Percent value</summary>
        PercentValue,
        /// <summary>Elapsed time in seconds</summary>
        ElapsedTime,
        /// <summary>String value</summary>
        StringValue
    }
}
