using System;
using System.Reflection;

namespace NDepth.Examples.Database.SQLExamples
{
    partial class Program
    {
        static void SqlConstraintsExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create new MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery(
                "CREATE TABLE MyTable (" +
                    "Id                 INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "Login              INTEGER NOT NULL UNIQUE CHECK (Login > 0), " +
                    "Address            TEXT, " +
                    "Description        TEXT DEFAULT(''), " +
                    "CONSTRAINT chk_Address CHECK ((Address LIKE 'USA,%') AND (LENGTH(Description) > 5))" +
                ");"));

            // Add some rows to the MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyTable VALUES (1, 100, 'USA, NY', 'New Login 1');"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyTable VALUES (2, 101, 'USA, CA', 'New Login 2');"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyTable VALUES (3, 102, 'USA, FL', 'New Login 3');"));

            // Select all records from MyTable table.
            db.ExecuteQuery("SELECT * FROM MyTable;").PrintToConsole();

            // Drop new MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP TABLE MyTable;"));
        }

        static void SqlForeignKeyExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create new MyParentTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery(
                "CREATE TABLE MyParentTable (" +
                    "Id                 INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "ArtistName         TEXT" +
                ");"));

            // Create new MyChildTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery(
                "CREATE TABLE MyChildTable (" +
                    "Id                 INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "TrackName          TEXT, " +
                    "TrackArtistFK      INTEGER NOT NULL, " +
                 // "FOREIGN KEY (TrackArtistFK) REFERENCES MyParentTable(Id)" +
                 // "FOREIGN KEY (TrackArtistFK) REFERENCES MyParentTable(Id) ON UPDATE NO ACTION ON DELETE NO ACTION" +
                 // "FOREIGN KEY (TrackArtistFK) REFERENCES MyParentTable(Id) ON UPDATE RESTRICT ON DELETE RESTRICT" +
                 // "FOREIGN KEY (TrackArtistFK) REFERENCES MyParentTable(Id) ON UPDATE SET NULL ON DELETE SET NULL" +
                 // "FOREIGN KEY (TrackArtistFK) REFERENCES MyParentTable(Id) ON UPDATE SET DEFAULT ON DELETE SET DEFAULT" +
                    "FOREIGN KEY (TrackArtistFK) REFERENCES MyParentTable(Id) ON UPDATE CASCADE ON DELETE CASCADE" +
                ");"));

            // Add some rows to the MyParentTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyParentTable (ArtistName) VALUES ('The Beatles');"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyParentTable (ArtistName) VALUES ('The Doors');"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyParentTable (ArtistName) VALUES ('ZZ Top');"));

            // Add some rows to the MyChildTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyChildTable (TrackName, TrackArtistFK) SELECT 'A Hard Day’s Night', Id FROM MyParentTable WHERE ArtistName = 'The Beatles' LIMIT 1;"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyChildTable (TrackName, TrackArtistFK) SELECT 'Help!', Id FROM MyParentTable WHERE ArtistName = 'The Beatles' LIMIT 1;"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyChildTable (TrackName, TrackArtistFK) SELECT 'Yesterday', Id FROM MyParentTable WHERE ArtistName = 'The Beatles' LIMIT 1;"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyChildTable (TrackName, TrackArtistFK) SELECT 'Strange Days', Id FROM MyParentTable WHERE ArtistName = 'The Doors' LIMIT 1;"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyChildTable (TrackName, TrackArtistFK) SELECT 'Riders on the Storm', Id FROM MyParentTable WHERE ArtistName = 'The Doors' LIMIT 1;"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyChildTable (TrackName, TrackArtistFK) SELECT 'Sharp Dressed Man', Id FROM MyParentTable WHERE ArtistName = 'ZZ Top' LIMIT 1;"));

            // Select all records from MyParentTable & MyChildTable tables.
            db.ExecuteQuery("SELECT * FROM MyParentTable;").PrintToConsole();
            db.ExecuteQuery("SELECT * FROM MyChildTable;").PrintToConsole();

            // Delete some rows from the MyParentTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DELETE FROM MyParentTable WHERE ArtistName = 'The Doors';"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DELETE FROM MyParentTable WHERE ArtistName = 'ZZ Top';"));

            // Select all records from MyParentTable & MyChildTable tables.
            db.ExecuteQuery("SELECT * FROM MyParentTable;").PrintToConsole();
            db.ExecuteQuery("SELECT * FROM MyChildTable;").PrintToConsole();

            // Drop new MyChildTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP TABLE MyChildTable;"));

            // Drop new MyParentTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP TABLE MyParentTable;"));
        }

        static void SqlIndexExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create new MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery(
                "CREATE TABLE MyTable (" +
                    "Id                 INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "Login              INTEGER NOT NULL CHECK (Login > 0), " +
                    "ZipCode            INT, " +
                    "Address            TEXT, " +
                    "Timestamp          DATETIME DEFAULT(DATETIME('now'))" +
                ");"));

            // Create unique index to the Login column of the MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("CREATE UNIQUE INDEX idxLogin ON MyTable (Login);"));

            // Create index to the Timestamp column of the MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("CREATE INDEX idxTimestamp ON MyTable (Timestamp);"));

            // Create combined index to the Timestamp column of the MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("CREATE INDEX idxZipCodeAddress ON MyTable (ZipCode, Address);"));

            // Add some rows to the MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyTable (Login, ZipCode, Address) VALUES (100, 10453, 'USA, NY');"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyTable (Login, ZipCode, Address) VALUES (101, 90001, 'USA, CA');"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyTable (Login, ZipCode, Address) VALUES (102, 32034, 'USA, FL');"));

            // Select all records from MyTable table.
            db.ExecuteQuery("SELECT * FROM MyTable;").PrintToConsole();

            // Drop indexes from MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP INDEX idxLogin;"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP INDEX idxTimestamp;"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP INDEX IF EXISTS idxZipCodeAddress;"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP INDEX IF EXISTS idxTemp;"));

            // Drop new MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP TABLE MyTable;"));
        }

        static void SqlViewExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create a view using some SQL select expression.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("CREATE VIEW MyView AS SELECT ProductName, LENGTH(ProductName) AS Length, RANDOM() AS Random FROM Products;"));

            // Select all records from MyView view.
            db.ExecuteQuery("SELECT * FROM MyView;").PrintToConsole();

            // Drop new MyView view.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP VIEW MyView;"));
        }

        static void SqlTriggerExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create new MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery(
                "CREATE TABLE MyTable (" +
                    "Id                 INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                    "Login              INTEGER NOT NULL CHECK (Login > 0), " +
                    "ZipCode            INT, " +
                    "Address            TEXT" +
                ");"));

            // Create new MyHistoryTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery(
                "CREATE TABLE MyHistoryTable (" +
                    "Id                 INTEGER NOT NULL PRIMARY KEY, " +
                    "Created            DATETIME, " +
                    "Updated            DATETIME, " +
                    "Deleted            DATETIME" +
                ");"));

            // Create insert trigger.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery(
                "CREATE TRIGGER MyInsertTrigger AFTER INSERT ON MyTable " +
                "BEGIN " +
                    "INSERT INTO MyHistoryTable (Id, Created) VALUES (NEW.Id, DATETIME('now')); " +
                "END;"));

            // Create update trigger.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery(
                "CREATE TRIGGER MyUpdateTrigger AFTER UPDATE ON MyTable " +
                "BEGIN " +
                    "UPDATE MyHistoryTable SET Id = NEW.Id, Updated = DATETIME('now') WHERE Id = OLD.Id; " +
                "END;"));

            // Create delete trigger.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery(
                "CREATE TRIGGER MyDeleteTrigger AFTER DELETE ON MyTable " +
                "BEGIN " +
                    "UPDATE MyHistoryTable SET Deleted = DATETIME('now') WHERE Id = OLD.Id; " +
                "END;"));

            // Generate some data.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyTable (Login, ZipCode, Address) VALUES (100, '1234567', 'Some address');"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyTable (Login, ZipCode, Address) VALUES (101, '7654321', 'Another address');"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("UPDATE MyTable SET Address = 'Some address modified' WHERE Login = 100;"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DELETE FROM MyTable WHERE Login = 101;"));

            // Select all records from MyHistoryTable view.
            db.ExecuteQuery("SELECT * FROM MyHistoryTable;").PrintToConsole();

            // Drop new MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP TABLE MyTable;"));
            // Drop new MyHistoryTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP TABLE MyHistoryTable;"));
        }
    }
}
