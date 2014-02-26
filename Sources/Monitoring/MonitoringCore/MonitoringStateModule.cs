using NDepth.Business.BusinessObjects.Common;
using System;
using System.Threading;

namespace NDepth.Monitoring.Core
{
    public class MonitoringStateModule : MonitoringStateComponent, IMonitoringStateModule 
    {
        private readonly CancellationTokenSource _updaterCancellationTokenSource = new CancellationTokenSource();
        private readonly Timer _updaterTimer;

        public MonitoringStateModule(string machineName, string moduleName)
            : base(moduleName, Severity.None, new MonitoringForModule(machineName, moduleName))
        {
            // Start monitoring update timer.
            _updaterTimer = new Timer(UpdateMonitoring, _updaterCancellationTokenSource, 0, 1000);
        }

        private void UpdateMonitoring(object state)
        {
            var cancellation = state as CancellationTokenSource;
            if ((cancellation == null) || (cancellation.Token.IsCancellationRequested))
                return;

            // Perform update starting from the current module state.
            UpdateState();
        }

        #region IDisposable implementation

        // Dispose flags.
        private bool _disposed;

        // Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposingManagedResources)
        {
            if (!_disposed)
            {
                if (disposingManagedResources)
                {
                    // Perform cancellation.
                    _updaterCancellationTokenSource.Cancel();

                    // Dispose updater timer.
                    _updaterTimer.Dispose();

                    // Try to dispose monitoring for module.
                    var disposable = Monitoring as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }                
        }

        // Use C# destructor syntax for finalization code.
        ~MonitoringStateModule()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion
    }
}
