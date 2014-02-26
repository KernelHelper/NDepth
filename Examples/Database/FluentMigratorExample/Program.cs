using System;
using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.Sqlite;

namespace NDepth.Examples.Database.FluentMigratorExample
{
    class Program
    {
        const string ConnectionString = "Data Source=NHibernateSQLite.db3;Version=3;Foreign Keys=true;";

        static void Main()
        {
            // Create migration announcer.
            var announcer = new TextWriterAnnouncer(Console.WriteLine);
            announcer.ShowSql = true;

            // Create migration context.
            var migrationContext = new RunnerContext(announcer);

            // Create migration options, factory and runner.
            var options = new ProcessorOptions
            {
                PreviewOnly = false,
                Timeout = 60
            };
            var factory = new SqliteProcessorFactory();
            var processor = factory.Create(ConnectionString, announcer, options);
            var runner = new MigrationRunner(Assembly.GetExecutingAssembly(), migrationContext, processor);

            // Update database.
            runner.MigrateUp(long.MaxValue, true);

            // Rollback database.
            runner.MigrateDown(long.MinValue, true);
        }
    }
}
