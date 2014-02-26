using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Common.Logging;
using Common.Logging.Log4Net;
using Common.Logging.NLog;
using Common.Logging.Simple;
using NDepth.Database.Hibernate;
using NDepth.Monitoring;
using NDepth.Monitoring.Hibernate;
using NDepth.Monitoring.Logger;

namespace NDepth.Module
{
    /// <summary>
    /// Module bootstrap class configures and starts module with the following steps:
    /// 1) Module searches for machine.xml file in current and parent directories. 
    /// 2) Then parses machine.xml to define the current machine name, description, 
    ///    connection string and logging settings. Machine connection string and 
    ///    logging settings will override the current ones from the configuration.
    /// 3) Module searches for module.xml file in current and parent directories. 
    /// 4) Then parses module.xml to define the current module name, description, 
    ///    connection string and logging settings. Module connection string and 
    ///    logging settings will override the current ones.
    /// 5) Initialize logging system.
    /// 6) Bootstrap module with registering all dependencies.
    /// 7) Create module instance.
    /// 8) Setup exception handler for the current application domain to catch
    ///    and log all unhandled exceptions. 
    /// </summary>
    public class ModuleBootstrap<T> : IDisposable
        where T : ModuleBase
    {
        private ModuleBase _module;
        private readonly ModuleConfiguration _moduleConfiguration = new ModuleConfiguration();

        /// <summary>Module instance property</summary>
        public T Module { get { return _module as T; } }

        #region Bootstrap

        protected virtual void Bootstrap()
        {
            // Setup module configuration.
            var configResult = SetupModuleConfiguration(ModulePath);

            // Setup logging system.
            SetupLoggingSystem();

            // Setup DB core.
            SetupDBCore(_moduleConfiguration.ConnectionString, _moduleConfiguration.ConnectionType);            

            // Setup monitoring system.
            SetupMonitoringSystem();

            // Setup dependency injection.
            SetupModuleContainer();

            // Create module instance.
            _module = ModuleContainer.Container.GetInstance<ModuleBase>();

            // Show module configuration results.
            ShowModuleConfigurationResults(Module, configResult);

            // Setup unhandled exceptions handler.
            SetupUnhandledExceptionsHandler();            
        }

        #endregion

        #region Configuration files

        protected virtual string ModulePath { get { return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location); } }

        protected virtual Tuple<bool, bool, bool> SetupModuleConfiguration(string modulePath)
        {
            // Get the current module path.
            if (string.IsNullOrEmpty(modulePath) || !Directory.Exists(modulePath))
                throw new ArgumentException(Resources.Strings.CurrentModulePathException);

            // Parse machine.xml file under the current module path.
            bool result1 = ParseMachineConfiguration(modulePath, "machine.xml");
            // Parse machine.xml file under the parent module path.
            bool result2 = ParseMachineConfiguration(modulePath + Path.DirectorySeparatorChar + "..", "machine.xml");
            // Parse module.xml file under the current module path.
            bool result3 = ParseModuleConfiguration(modulePath, "module.xml");

            return Tuple.Create(result1, result2, result3);
        }

        protected virtual void ShowModuleConfigurationResults(ModuleBase module, Tuple<bool, bool, bool> results)
        {
            if (results.Item1)
                module.Logger.InfoFormat(Resources.Strings.ModuleInitializedFromLocalMachineXml, module.MachineName, module.ModuleName);
            if (results.Item2)
                module.Logger.InfoFormat(Resources.Strings.ModuleInitializedFromParentMachineXml, module.MachineName, module.ModuleName);
            if (results.Item3)
                module.Logger.InfoFormat(Resources.Strings.ModuleInitializedFromModuleXml, module.MachineName, module.ModuleName);
        }

        private bool ParseMachineConfiguration(string path, string machine)
        {
            try
            {
                // Parse machine.xml
                XDocument machineDoc = XDocument.Load(path + Path.DirectorySeparatorChar + machine);

                // Get the root element.
                XElement root = machineDoc.Element("Machine");
                if (root == null)
                    return false;

                // Try to get machine name, machine description and connection string.
                XElement name = root.Element("Name");
                _moduleConfiguration.MachineName = (name != null) ? name.Value : _moduleConfiguration.MachineName;
                XElement description = root.Element("Description");
                _moduleConfiguration.MachineDescription = (description != null) ? description.Value : _moduleConfiguration.MachineDescription;
                XElement address = root.Element("Address");
                _moduleConfiguration.MachineAddress = (address != null) ? address.Value : _moduleConfiguration.MachineAddress;
                XElement connectionString = root.Element("ConnectionString");
                _moduleConfiguration.ConnectionString = ((connectionString != null) && !string.IsNullOrEmpty(connectionString.Value)) ? connectionString.Value : _moduleConfiguration.ConnectionString;
                XElement connectionType = root.Element("ConnectionType");
                _moduleConfiguration.ConnectionType = ((connectionType != null) && !string.IsNullOrEmpty(connectionType.Value)) ? connectionType.Value : _moduleConfiguration.ConnectionType;

                // Try to get logging configuration.
                if (!ParseLoggingConfiguration(path, root.Element("Logging")))
                    return false;

                return true;
            }
            catch
            {
                // Catch all exceptions.
                return false;
            }
        }

        private bool ParseModuleConfiguration(string path, string module)
        {
            try
            {
                // Parse mdule.xml
                XDocument moduleDoc = XDocument.Load(path + Path.DirectorySeparatorChar + module);

                // Get the root element.
                XElement root = moduleDoc.Element("Module");
                if (root == null)
                    return false;

                // Try to get module name, module description and connection string.
                XElement name = root.Element("Name");
                _moduleConfiguration.ModuleName = (name != null) ? name.Value : _moduleConfiguration.ModuleName;
                XElement description = root.Element("Description");
                _moduleConfiguration.ModuleDescription = (description != null) ? description.Value : _moduleConfiguration.ModuleDescription;
                XElement version = root.Element("Version");
                _moduleConfiguration.ModuleVersion = (version != null) ? version.Value : _moduleConfiguration.ModuleVersion;
                XElement connectionString = root.Element("ConnectionString");
                _moduleConfiguration.ConnectionString = ((connectionString != null) && !string.IsNullOrEmpty(connectionString.Value)) ? connectionString.Value : _moduleConfiguration.ConnectionString;
                XElement connectionType = root.Element("ConnectionType");
                _moduleConfiguration.ConnectionType = ((connectionType != null) && !string.IsNullOrEmpty(connectionType.Value)) ? connectionType.Value : _moduleConfiguration.ConnectionType;

                // Try to get logging configuration.
                if (!ParseLoggingConfiguration(path, root.Element("Logging")))
                    return false;

                return true;
            }
            catch
            {
                // Catch all exceptions.
                return false;
            }
        }

        private bool ParseLoggingConfiguration(string path, XElement logging)
        {
            if (logging == null)
                return true;

            XElement logType = logging.Element("LoggingType");

            // Try to get logging type.
            LoggingType logTypeValue;
            if ((logType != null) && (Enum.TryParse(logType.Value, true, out logTypeValue)))
            {
                _loggingType = logTypeValue;

                // Try to get logging configuration.
                XElement logConfig = logging.Element("LoggingConfig");
                if (logConfig != null)
                {
                    var collection = new NameValueCollection();
                    var query = from el in logConfig.Elements() select el;
                    foreach (var el in query)
                        collection.Add(el.Name.LocalName, el.Value);

                    // Try to get 'configFile' logging property.
                    string configFile = collection["configFile"];
                    if (!string.IsNullOrEmpty(configFile))
                    {
                        // Update the content file name.
                        collection["configFile"] = configFile.Replace("~", path);
                    }

                    _loggingConfig = collection;
                }

                // Try to get logging content.
                XElement logContent = logging.Element("LoggingContent");
                if (logContent != null)
                {
                    _loggingContent = logContent.FirstNode as XElement;
                }

                return true;
            }

            return false;
        }

        #endregion

        #region Logging system

        private enum LoggingType
        {
            Null,
            Console,
            Trace,
            log4net,
            NLog
        }

        private LoggingType _loggingType = LoggingType.Null;
        private NameValueCollection _loggingConfig = new NameValueCollection();
        private XElement _loggingContent;

        protected virtual void SetupLoggingSystem()
        {
            // In case of logging content is present, update corresponding content file.
            if (_loggingContent != null)
            {
                // Try to get 'configFile' logging property.
                string configFile = _loggingConfig["configFile"];
                if (!string.IsNullOrEmpty(configFile))
                {
                    // Save logging configuration content.
                    _loggingContent.Save(configFile);
                }
            }

            // Setup logging adapter.
            switch (_loggingType)
            {
                case LoggingType.Null:
                    LogManager.Adapter = new NoOpLoggerFactoryAdapter(_loggingConfig);
                    break;
                case LoggingType.Console:
                    LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(_loggingConfig);
                    break;
                case LoggingType.Trace:
                    LogManager.Adapter = new TraceLoggerFactoryAdapter(_loggingConfig);
                    break;
                case LoggingType.log4net:
                    LogManager.Adapter = new Log4NetLoggerFactoryAdapter(_loggingConfig);
                    break;
                case LoggingType.NLog:
                    LogManager.Adapter = new NLogLoggerFactoryAdapter(_loggingConfig);
                    break;
            }
        }

        protected virtual void ShutdownLoggingSystem()
        {
            switch (_loggingType)
            {
                case LoggingType.log4net:
                    log4net.LogManager.Shutdown();
                    break;
                case LoggingType.NLog:
                    NLog.LogManager.Shutdown();
                    break;
            }
        }

        #endregion

        #region DB core system

        protected virtual void SetupDBCore(string connectionString, string connectionType)
        {
            // Register Hibernate core.
            ModuleContainer.Container.RegisterSingle(() => new HibernateCore(connectionString, connectionType));
        }

        protected virtual void ShutdownDBCore()
        {
            // Dispose Hibernate core.
            var hibernateCore = ModuleContainer.Container.GetInstance<HibernateCore>();
            if (hibernateCore != null)
                hibernateCore.Dispose();
        }

        #endregion

        #region Monitoring system

        protected virtual void SetupMonitoringSystem()
        {
            // Register monitoring storage.
            ModuleContainer.Container.RegisterSingle<IMonitoringStorage>(() => new HibernateMonitoringStorage(ModuleContainer.Container.GetInstance<HibernateCore>()));

            // Register monitoring notify.
            ModuleContainer.Container.RegisterSingle<IMonitoringNotify, LoggerMonitoringNotify>();
            ModuleContainer.Container.RegisterSingle<IMonitoringNotifyEmail, LoggerMonitoringNotifyEmail>();
            ModuleContainer.Container.RegisterSingle<IMonitoringNotifySms, LoggerMonitoringNotifySms>();
        }

        protected virtual void ShutdownMonitoringSystem()
        {
            // Dispose monitoring storage.
            var storage = ModuleContainer.Container.GetInstance<IMonitoringStorage>();
            if (storage != null)
                storage.Dispose();

            // Dispose monitoring notify.
            var notify = ModuleContainer.Container.GetInstance<IMonitoringNotify>();
            if (notify != null)
                notify.Dispose();
            var notifyEmail = ModuleContainer.Container.GetInstance<IMonitoringNotifyEmail>();
            if (notifyEmail != null)
                notifyEmail.Dispose();
            var notifySms = ModuleContainer.Container.GetInstance<IMonitoringNotifySms>();
            if (notifySms != null)
                notifySms.Dispose();
        }

        #endregion

        #region Dependency injection

        protected virtual void SetupModuleContainer()
        {
            // Register module configuration.
            ModuleContainer.Container.RegisterSingle(() => _moduleConfiguration);

            // Register module.
            ModuleContainer.Container.RegisterSingle<ModuleBase, T>();

            // Check the configuration for correctness, such as missing dependencies and recursive dependency graphs.
            ModuleContainer.Container.Verify();
        }

        protected virtual void ShutdownModuleContainer()
        {
            // Dispose module at the end.
            var module = ModuleContainer.Container.GetInstance<ModuleBase>();
            if (module != null)
                module.Dispose();
        }

        #endregion

        #region Unhandled exceptions handler

        protected virtual void SetupUnhandledExceptionsHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, ex) => Module.Logger.Fatal(Resources.Strings.UnhandledException, ex.ExceptionObject as Exception);
        }

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
                    // Shutdown dependency injection.
                    ShutdownModuleContainer();

                    // Shutdown monitoring system.
                    ShutdownMonitoringSystem();

                    // Shutdown DB core.
                    ShutdownDBCore();

                    // Shutdown logging system.
                    ShutdownLoggingSystem();
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }                
        }

        // Use C# destructor syntax for finalization code.
        ~ModuleBootstrap()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion
    }
}
