using System;
using System.Runtime.CompilerServices;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

[assembly: InternalsVisibleTo("NDepth.Module")]
[assembly: InternalsVisibleTo("NDepth.Module.Test")]

namespace NDepth.Database.Hibernate
{
    /// <summary>
    /// Hibernate database core class is used to initialize DB layer, configure NHibernlate with ISessionFactory and open new NHibernate sessions.
    /// </summary>
    public class HibernateCore : IDisposable
    {
        #region Constructors

        /// <summary>Create in memory NHibernate session factory based on SQLite database</summary>
        public HibernateCore()
        {
            // Create NHebirnate session factory.
            SessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.InMemory().FormatSql())
                .Mappings(m => m.AutoMappings.Add(HibernateAutoMapping.CreateAutoMappingModel()))
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
                .BuildSessionFactory();
        }

        /// <summary>Create NHibernate session factory based on the given connection string and connection type</summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="connectionType">Connection type</param>
        /// <exception cref="ArgumentException">
        ///   if <paramref name="connectionType"/> is unsupported connection type.
        /// </exception>
        public HibernateCore(string connectionString, string connectionType)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(string.Format(Resources.Strings.ArgumentIsNullOrEmpty, "connectionString"));
            if (string.IsNullOrEmpty(connectionType))
                throw new ArgumentException(string.Format(Resources.Strings.ArgumentIsNullOrEmpty, "connectionType"));

            // Get DB dependent NHibernate configuration.
            IPersistenceConfigurer config;
            switch (connectionType)
            {
                case "Sqlite":
                    config = SQLiteConfiguration.Standard.ConnectionString(connectionString).FormatSql();
                    break;
                case "SqlServer":
                    config = MsSqlConfiguration.MsSql2008.ConnectionString(connectionString).FormatSql();
                    break;
                case "SqlServerCe":
                    config = MsSqlCeConfiguration.Standard.ConnectionString(connectionString).FormatSql();
                    break;
                default:
                    throw new ArgumentException(string.Format(Resources.Strings.ConnectionTypeUnsupported, connectionType));
            }

            // Create NHebirnate session factory.
            SessionFactory = Fluently.Configure().Database(config)
                .Mappings(m => m.AutoMappings.Add(HibernateAutoMapping.CreateAutoMappingModel()))
                .BuildSessionFactory();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Hibernate session factory
        /// </summary>
        public ISessionFactory SessionFactory { get; private set; }

        #endregion

        #region Public methods

        /// <summary>Open new NHibernate session</summary>
        /// <returns>NHibernate session (can be null)</returns>
        public ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        /// <summary>Perform transaction without returning value</summary>
        /// <param name="transBody">Transaction body</param>
        public void PerformTransaction(Action<ISession> transBody)
        {
            // Create NHibernate session.
            using (var session = OpenSession())
            {
                // Setup flush mode.
                session.FlushMode = FlushMode.Commit;

                // Begin new transaction.
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Perform transaction.
                        transBody(session);

                        // Commit the transaction.
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Rollback transaction.
                        if (transaction != null)
                            transaction.Rollback();

                        // Rethrow exception.
                        throw;
                    }
                }
            }
        }

        /// <summary>Perform transaction with returning value</summary>
        /// <param name="transBody">Transaction body</param>
        public T PerformTransaction<T>(Func<ISession, T> transBody)
        {
            // Create NHibernate session.
            using (var session = OpenSession())
            {
                // Setup flush mode.
                session.FlushMode = FlushMode.Commit;

                // Begin new transaction.
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Perform transaction.
                        T result = transBody(session);

                        // Commit the transaction.
                        transaction.Commit();

                        return result;
                    }
                    catch (Exception)
                    {
                        // Rollback transaction.
                        if (transaction != null)
                            transaction.Rollback();

                        // Rethrow exception.
                        throw;
                    }
                }
            }
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
                    SessionFactory.Dispose();
                    SessionFactory = null;
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }                
        }

        // Use C# destructor syntax for finalization code.
        ~HibernateCore()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion
    }
}
