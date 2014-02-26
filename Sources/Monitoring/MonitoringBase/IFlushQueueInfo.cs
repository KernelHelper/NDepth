namespace NDepth.Monitoring
{
    /// <summary>Flush queue interface provides monitoring information about asynchronous flush queue</summary>
    public interface IFlushQueueInfo
    {
        /// <summary>Flush queue size</summary>
        int FlushQueueSize { get; }
    }
}
