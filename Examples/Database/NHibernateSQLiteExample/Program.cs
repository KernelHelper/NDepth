using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.Log4Net;

namespace NDepth.Examples.Database.NHibernateSQLiteExample
{
    class Program
    {
        static void Main()
        {
            // Create logger factory.
            LogManager.Adapter = new Log4NetLoggerFactoryAdapter(new NameValueCollection { { "configType", "FILE-WATCH" }, { "configFile", "~/log4net.config" } });

            using (var db = new Database())
            {
                // Create database schema.
                db.CreateSchema();

                // Fill database with sample data.
                db.FillWithData();
            }
        }
    }
}
