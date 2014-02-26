using System;
using System.Reflection;

namespace NDepth.Examples.Database.SQLExamples
{
    partial class Program
    {
        static void SqlUpperExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select product names in uppercase.
            db.ExecuteQuery("SELECT UPPER(ProductName) FROM Products;").PrintToConsole();
        }

        static void SqlLowerExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select product names in lowercase.
            db.ExecuteQuery("SELECT LOWER(ProductName) FROM Products;").PrintToConsole();
        }

        static void SqlSubstrExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select left trim substrings from product names.
            db.ExecuteQuery("SELECT SUBSTR(ProductName, 3) FROM Products;").PrintToConsole();

            // Select right trim substrings from product names.
            db.ExecuteQuery("SELECT SUBSTR(ProductName, -3) FROM Products;").PrintToConsole();

            // Select middle 3 symbols substrings from product names.
            db.ExecuteQuery("SELECT SUBSTR(ProductName, 3, 3) FROM Products;").PrintToConsole();
        }

        static void SqlLengthExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select length of all product names.
            db.ExecuteQuery("SELECT ProductName, LENGTH(ProductName) AS Length FROM Products;").PrintToConsole();
        }

        static void SqlRoundExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select product unit price rounded to the given digits.
            db.ExecuteQuery("SELECT ProductName, ROUND(UnitPrice, 2) AS Price FROM Products;").PrintToConsole();
        }

        static void SqlRandomExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select product marked by random value.
            db.ExecuteQuery("SELECT ProductName, RANDOM() AS Random FROM Products;").PrintToConsole();
        }
    }
}
