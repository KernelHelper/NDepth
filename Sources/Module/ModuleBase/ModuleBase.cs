using System;
using System.Runtime.CompilerServices;
using Common.Logging;
using NDepth.Monitoring;
using NDepth.Monitoring.Core;
using SimpleInjector;

[assembly: InternalsVisibleTo("NDepth.Module.Test")]

namespace NDepth.Module
{
    /// <summary>
    /// Base module class contains common module configurations, properties and start/stop module handlers.
    /// </summary>
    public abstract class ModuleBase : IDisposable 
    {
        /// <summary>Dependency injection container</summary>
        public Container Container { get { return ModuleContainer.Container; } }

        /// <summary>Module logger</summary>
        public readonly ILog Logger;
        /// <summary>Module monitoring</summary>
        public readonly IMonitoring Monitoring;
        /// <summary>Module state</summary>
        public readonly IMonitoringStateModule State;

        /// <summary>Machine name</summary>
        public readonly string MachineName;
        /// <summary>Machine description</summary>
        public readonly string MachineDescription;
        /// <summary>Machine address</summary>
        public readonly string MachineAddress;
        /// <summary>Module name</summary>
        public readonly string ModuleName;
        /// <summary>Module description</summary>
        public readonly string ModuleDescription;
        /// <summary>Module version</summary>
        public readonly string ModuleVersion;
        /// <summary>Module connection string</summary>
        public readonly string ConnectionString;
        /// <summary>Module connection type</summary>
        public readonly string ConnectionType;

        protected ModuleBase()
        {
            // Get module configuration.
            var configuration = Container.GetInstance<ModuleConfiguration>();

            // Resolve predefined values.
            MachineName = configuration.MachineName;
            MachineDescription = configuration.MachineDescription;
            MachineAddress = configuration.MachineAddress;
            ModuleName = configuration.ModuleName;
            ModuleDescription = configuration.ModuleDescription;
            ModuleVersion = configuration.ModuleVersion;
            ConnectionString = configuration.ConnectionString;
            ConnectionType = configuration.ConnectionType;

            // Create module state.
            State = new MonitoringStateModule(MachineName, ModuleName);

            // Get module logger and monitoring.
            Logger = State.Logger;
            Monitoring = State.Monitoring;
        }

        #region Module interface

        /// <summary>Start module handler. Can be implemented to define module startup functionality.</summary>
        /// <param name="args">Command line arguments</param>
        protected internal virtual void OnStart(string[] args) { }
        /// <summary>Run module handler. Can be implemented to define module run functionality.</summary>
        protected internal virtual void OnRun() { } 
        /// <summary>Stop module handler. Can be implemented to define module stop functionality.</summary>
        protected internal virtual void OnStop() { } 

        #endregion

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
                    State.Dispose();
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }                
        }

        // Use C# destructor syntax for finalization code.
        ~ModuleBase()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion
    }
}
