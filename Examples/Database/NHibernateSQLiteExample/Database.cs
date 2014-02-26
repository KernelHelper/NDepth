using System;
using NHibernate;
using NHibernate.Cfg;
using NDepth.Examples.Database.NHibernateSQLiteExample.Domain;
using NHibernate.Tool.hbm2ddl;

namespace NDepth.Examples.Database.NHibernateSQLiteExample
{
    public class Database : IDisposable
    {
        private readonly Configuration _configuration;
        private readonly ISessionFactory _sessionFactory;

        public Database()
        {
            // Initialize NHibernate.
            _configuration = new Configuration();
            _configuration.Configure("SQLite.cfg.xml");
            _configuration.AddAssembly(typeof(Database).Assembly);

            // Create session factory.
            _sessionFactory = _configuration.BuildSessionFactory();
        }

        public void CreateSchema()
        {
            // Get new schema export based on the current configuration.
            var schema = new SchemaExport(_configuration);
            // Create schema for the current domain.
            schema.Create(true, true);
        }

        public void FillWithData()
        {
            // Open new session.
            using (var session = _sessionFactory.OpenSession())
            {
                // Setup flush mode.
                session.FlushMode = FlushMode.Commit;

                // Begin new transaction.
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Create some customers.
                        var customer = new Customer
                        {
                            FirstName = "Luís",
                            LastName = "Gonçalves",
                            Address = "Embraer - Empresa Brasileira de Aeronáutica S.A., Av. Brigadeiro Faria Lima, 2170, São José dos Campos, SP, Brazil, 12227-000",
                            Email = "luisg@embraer.com.br",
                            Phone = "+55 (12) 3923-5566"
                        };
                        session.Save(customer);
                        customer = new Customer
                        {
                            FirstName = "Leonie",
                            LastName = "Köhler",
                            Address = "Theodor-Heuss-Straße 34, Stuttgart, Germany, 70174",
                            Email = "leonekohler@surfeu.de",
                            Phone = "+49 0711 2842222"
                        };
                        session.Save(customer);
                        customer = new Customer
                        {
                            FirstName = "François",
                            LastName = "Tremblay",
                            Address = "1498 rue Bélanger, Montréal, QC, Canada, H2G 1A7",
                            Email = "ftremblay@gmail.com",
                            Phone = "+1 (514) 721-4711"
                        };
                        session.Save(customer);

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

        #region IDisposable implementation

        // Disposed flag.
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
                    // Dispose managed resources here...
                    _sessionFactory.Dispose();
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~Database()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion
    }
}
