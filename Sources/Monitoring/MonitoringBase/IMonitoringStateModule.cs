using System;

namespace NDepth.Monitoring
{
    /// <summary>Monitoring module state interface provides functionality to handle root module component and track its monitoring states</summary>
    public interface IMonitoringStateModule : IMonitoringStateComponent, IDisposable
    {
    }
}
