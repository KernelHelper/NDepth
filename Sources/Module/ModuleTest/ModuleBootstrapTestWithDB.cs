using System;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using Common.Logging;
using Common.Logging.Simple;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.Sqlite;
using NDepth.Database.Hibernate;
using NDepth.Database.Migrations;
using NDepth.Monitoring;
using NDepth.Monitoring.Hibernate;
using NDepth.Monitoring.Null;

namespace NDepth.Module.Test
{
    /// <summary>
    /// Module test class configures and starts module for testing purposes with in memory DB core.
    /// It has empty logging, in memory DB monitoring and notification systems, enabled DB core with 
    /// valid DB schema and initial testing data.
    /// </summary>
    public sealed class ModuleBootstrapTestWithDB<T> : ModuleBootstrap<T>
        where T : ModuleBase
    {
        public ModuleBootstrapTestWithDB()
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
            const string inMemoryConnectionString = "FullUri=file:memory:?mode=memory&cache=shared;Version=3;Foreign Keys=true;";

            // Create in memory SQLite database.
            CreateConnectionInMemory(inMemoryConnectionString);
            CreateDatabaseSchemaInMemory(inMemoryConnectionString);

            // Register Hibernate core.
            ModuleContainer.Container.RegisterSingle(() => new HibernateCore(inMemoryConnectionString, "Sqlite"));
        }

        protected override void ShutdownDBCore()
        {
            // Dispose Hibernate core.
            var hibernateCore = ModuleContainer.Container.GetInstance<HibernateCore>();
            if (hibernateCore != null)
                hibernateCore.Dispose();
        }

        #endregion

        #region Monitoring system

        protected override void SetupMonitoringSystem()
        {
            // Register monitoring storage.
            ModuleContainer.Container.RegisterSingle<IMonitoringStorage>(() => new HibernateMonitoringStorage(ModuleContainer.Container.GetInstance<HibernateCore>()));

            // Register monitoring notify.
            ModuleContainer.Container.RegisterSingle<IMonitoringNotify, NullMonitoringNotify>();
            ModuleContainer.Container.RegisterSingle<IMonitoringNotifyEmail, NullMonitoringNotifyEmail>();
            ModuleContainer.Container.RegisterSingle<IMonitoringNotifySms, NullMonitoringNotifySms>();
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

        #region Utility methods

        private SQLiteConnection _connection;

        private void CreateConnectionInMemory(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");

            // Create and open first SQLite connection to keep in memory database.
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
        }

        private void CreateDatabaseSchemaInMemory(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");

            // Enable testing profile in FluentMigrator.
            Environment.SetEnvironmentVariable("TestingProfile", "1", EnvironmentVariableTarget.Process);

            // Create migration announcer.
            var announcer = new ConsoleAnnouncer();

            // Create migration context.
            var migrationContext = new RunnerContext(announcer);

            // Create migration options, factory and runner.
            var options = new ProcessorOptions
            {
                PreviewOnly = false,
                Timeout = 60
            };
            var factory = new SqliteProcessorFactory();
            var processor = factory.Create(connectionString, announcer, options);
            var runner = new MigrationRunner(Assembly.GetAssembly(typeof(MigrationExtensions)), migrationContext, processor);

            // Update database.
            runner.MigrateUp(long.MaxValue, true);
        }

        #endregion
    }
}
