using System;
using System.Reflection;

namespace NDepth.Examples.Database.SQLExamples
{
    partial class Program
    {
        static void SqlCreateTableExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create new MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery(
                "CREATE TABLE MyTable (" +
                    "CharType          CHAR(10), " +
                    "VarCharType       VCHAR(256), " +
                    "TextType          TEXT, " +

                    "NCharType          NCHAR(10) COLLATE BINARY, " +
                    "NVarCharType       NVCHAR(256) COLLATE RTRIM, " +
                    "NTextType          NTEXT COLLATE NOCASE, " +

                    "BoolType           BOOLEAN, " +
                    "BlobType           BLOB, " +

                    "TinyIntType        TINYINT, " +
                    "SmallIntType       SMALLINT, " +
                    "IntType            INT, " +
                    "BigIntType         BIGINT, " +

                    "UTinyIntType       UNSIGNED TINYINT, " +
                    "USmallIntType      UNSIGNED SMALLINT, " +
                    "UIntType           UNSIGNED INT, " +
                    "UBigIntType        UNSIGNED BIGINT, " +

                    "RealType           REAL, " +
                    "FloatType          FLOAT, " +
                    "DoubleType         DOUBLE, " +

                    "NumericType        NUMERIC, " +
                    "DecimalType        DECIMAL(10, 5), " +

                    "TimeType           TIME, " +
                    "DateType           DATE, " +
                    "DateTimeType       DATETIME" +
                ");"));
        }

        static void SqlDropTableExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Drop new MyTable table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP TABLE MyTable;"));
        }

        static void SqlAlterTableExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create new MyTable table and fill it with some data.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("CREATE TABLE MyTable (Id INTEGER, Name TEXT);"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyTable VALUES (1, 'Value 1');"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyTable VALUES (2, 'Value 2');"));
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO MyTable VALUES (3, 'Value 3');"));

            // Select all records from MyTable table.
            db.ExecuteQuery("SELECT * FROM MyTable;").PrintToConsole();

            // Alter the MyTable table and add new column.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("ALTER TABLE MyTable ADD COLUMN Temp TEXT DEFAULT('foobar');"));

            // Select all records from MyTable table.
            db.ExecuteQuery("SELECT * FROM MyTable;").PrintToConsole();

            // Alter the MyTable table and modify new column.
            // Console.WriteLine("Query returns: " + db.ExecuteNonQuery("ALTER TABLE MyTable ALTER COLUMN Temp VARCHAR(10);"));

            // Alter the MyTable table and drop new column.
            // Console.WriteLine("Query returns: " + db.ExecuteNonQuery("ALTER TABLE MyTable DROP COLUMN Temp;"));

            // Select all records from MyTable table.
            db.ExecuteQuery("SELECT * FROM MyTable;").PrintToConsole();

            // Alter the MyTable table and rename it.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("ALTER TABLE MyTable RENAME TO MyTableEx;"));

            // Drop new MyTableEx table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP TABLE MyTableEx;"));
        }
    }
}
