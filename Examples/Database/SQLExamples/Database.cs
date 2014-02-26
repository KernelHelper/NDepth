using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;

namespace NDepth.Examples.Database.SQLExamples
{
    public class Database : IDisposable
    {
        private const string DbName = "Northwind.Sqlite3.db3";
        private const string ConnectionString = "Data Source=" + DbName + ";Version=3;Foreign Keys=true;";
        private readonly SQLiteFactory _connectionFactory;

        public Database()
        {
            // Create connection factory.
            _connectionFactory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
        }

        public DataTable ExecuteQuery(string sqlQuery)
        {
            var table = new DataTable();

            // Create a new DB connection.
            using (var connection = (SQLiteConnection)_connectionFactory.CreateConnection())
            {
                if (connection == null)
                    return table;

                // Open the DB connection.
                connection.ConnectionString = ConnectionString;
                connection.Open();

                // Execute SQL command.
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sqlQuery;
                    using (var reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }
            }

            return table;
        }

        public int ExecuteNonQuery(string sqlQuery)
        {
            // Create a new DB connection.
            using (var connection = (SQLiteConnection)_connectionFactory.CreateConnection())
            {
                if (connection == null)
                    return 0;

                // Open the DB connection.
                connection.ConnectionString = ConnectionString;
                connection.Open();

                // Execute SQL command.
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sqlQuery;
                    return command.ExecuteNonQuery();
                }
            }
        }

        public string ExecuteScalar(string sqlQuery)
        {
            // Create a new DB connection.
            using (var connection = (SQLiteConnection)_connectionFactory.CreateConnection())
            {
                if (connection == null)
                    return null;

                // Open the DB connection.
                connection.ConnectionString = ConnectionString;
                connection.Open();

                // Execute SQL command.
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sqlQuery;
                    return command.ExecuteScalar().ToString();
                }
            }
        }

        public void InsertRecord(String tableName, Dictionary<String, String> recordData)
        {
            if (recordData.Count < 1)
                return;

            var columns = "";
            var values = "";

            foreach (var record in recordData)
            {
                columns += String.Format(" {0},", record.Key);
                values += String.Format(" '{0}',", record.Value);
            }

            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);

            ExecuteNonQuery(String.Format("INSERT INTO {0} ({1}) VALUES ({2});", tableName, columns, values));
        }

        public void UpdateRecord(String tableName, Dictionary<String, String> recordData, string where)
        {
            if (recordData.Count < 1)
                return;

            var values = recordData.Aggregate("", (current, record) => current + String.Format(" {0} = '{1}',", record.Key, record.Value));

            values = values.Substring(0, values.Length - 1);

            ExecuteNonQuery(String.Format("UPDATE {0} SET {1} WHERE {2};", tableName, values, where));
        }

        public void DeleteRecord(String tableName, string where)
        {
            ExecuteNonQuery(String.Format("DELETE FROM {0} WHERE {1};", tableName, where));
        }

        public void ClearTable(String tableName)
        {
            ExecuteNonQuery(String.Format("DELETE FROM {0};", tableName));
        }

        public void ClearDb()
        {
            DataTable tables = ExecuteQuery("SELECT NAME FROM SQLITE_MASTER WHERE type='table' order by NAME;");
            foreach (DataRow table in tables.Rows)
                ClearTable(table["NAME"].ToString());
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
                    _connectionFactory.Dispose();
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
