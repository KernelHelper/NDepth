using System;
using System.Reflection;

namespace NDepth.Examples.Database.SQLExamples
{
    partial class Program
    {
        static void SqlDateTimeNowExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select current date & time.
            // db.ExecuteQuery("SELECT ProductName, NOW() AS Timestamp FROM Products;").PrintToConsole();

            // Select current date.
            db.ExecuteQuery("SELECT ProductName, DATE('now') AS Timestamp FROM Products;").PrintToConsole();

            // Select current local time.
            db.ExecuteQuery("SELECT ProductName, TIME('now', 'localtime') AS Timestamp FROM Products;").PrintToConsole();

            // Select current utc date & time.
            db.ExecuteQuery("SELECT ProductName, DATETIME('now', 'utc') AS Timestamp FROM Products;").PrintToConsole();
        }

        static void SqlDateTimeFormatExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // %d - day of month: 00
            // %f - fractional seconds: SS.SSS
            // %H - hour: 00-24
            // %j - day of year: 001-366
            // %J - Julian day number
            // %m - month: 01-12
            // %M - minute: 00-59
            // %s - seconds since 1970-01-01
            // %S - seconds: 00-59
            // %w - day of week 0-6 with Sunday==0
            // %W - week of year: 00-53
            // %Y - year: 0000-9999
            // %% - %

            // Select current formatted date & time.
            db.ExecuteQuery("SELECT ProductName, STRFTIME('%Y.%m.%d %H:%M:%S', 'now') AS Timestamp FROM Products;").PrintToConsole();
        }
    }
}
