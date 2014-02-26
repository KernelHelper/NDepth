using System.IO;
using System.Reflection;
using Common.Logging;
using Common.Logging.Simple;
using NDepth.Monitoring;
using NDepth.Monitoring.Test;

namespace NDepth.Module.Test
{
    /// <summary>
    /// Module test class configures and starts module for monitoring testing purposes.
    /// It has empty logging system and testing monitoring and notification systems.
    /// </summary>
    public sealed class ModuleBootstrapTestMonitoring<T> : ModuleBootstrap<T>
        where T : ModuleBase
    {
        public ModuleBootstrapTestMonitoring()
        {
            Bootstrap();
        }

        #region Configuration files

        protected override string ModulePath { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "")); } }

        #endregion

        #region Logging system

        protected override void SetupLoggingSystem()
        {
            LogManager.Adapter = new NoOpLoggerFactoryAdapter();
        }

        #endregion

        #region DB core system

        protected override void SetupDBCore(string connectionString, string connectionType)
        {
            // Do nothing here...
        }

        protected override void ShutdownDBCore()
        {
            // Do nothing here...
        }

        #endregion

        #region Monitoring system

        protected override void SetupMonitoringSystem()
        {
            // Register monitoring storage.
            ModuleContainer.Container.RegisterSingle<IMonitoringStorage, TestMonitoringStorage>();

            // Register monitoring notify.
            ModuleContainer.Container.RegisterSingle<IMonitoringNotify, TestMonitoringNotify>();
            ModuleContainer.Container.RegisterSingle<IMonitoringNotifyEmail, TestMonitoringNotifyEmail>();
            ModuleContainer.Container.RegisterSingle<IMonitoringNotifySms, TestMonitoringNotifySms>();
        }

        #endregion

        #region Module service interface

        /// <summary>Start module handler. Can be implemented to define module startup functionality.</summary>
        /// <param name="args">Command line arguments</param>
        public void OnStart(string[] args)
        {
            Module.OnStart(args);
        }

        /// <summary>Run module handler. Can be implemented to define module run functionality.</summary>
        public void OnRun()
        {
            Module.OnRun();
        }

        /// <summary>Stop module handler. Can be implemented to define module stop functionality.</summary>
        public void OnStop()
        {
            Module.OnStop();
        }

        #endregion
    }
}
