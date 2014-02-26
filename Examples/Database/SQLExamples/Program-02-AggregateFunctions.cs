using System;
using System.Reflection;

namespace NDepth.Examples.Database.SQLExamples
{
    partial class Program
    {
        static void SqlAvgExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select average unit price of all products.
            db.ExecuteQuery("SELECT AVG(UnitPrice) FROM Products;").PrintToConsole();

            // Select all products with unit price greater than average unit price.
            db.ExecuteQuery("SELECT ProductName, UnitPrice FROM Products WHERE UnitPrice > (SELECT AVG(UnitPrice) FROM Products);").PrintToConsole();
        }

        static void SqlCountExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select the count of all customers.
            db.ExecuteQuery("SELECT COUNT(*) FROM Customers;").PrintToConsole();

            // Select the count of all customers with not NULL country.
            db.ExecuteQuery("SELECT COUNT(Country) FROM Customers;").PrintToConsole();

            // Select the count of different countries for all customers.
            db.ExecuteQuery("SELECT COUNT(DISTINCT Country) FROM Customers;").PrintToConsole();
        }

        static void SqlFirstExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select the first available country from Customers table.
            // db.ExecuteQuery("SELECT FIRST(Country) AS FirstCountry FROM Customers;").PrintToConsole();

            // Select the first available country from Customers table.
            db.ExecuteQuery("SELECT Country FROM Customers ORDER BY CustomerID LIMIT 1;").PrintToConsole();
        }

        static void SqlLastExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select the last available country from Customers table.
            // db.ExecuteQuery("SELECT LAST(Country) AS FirstCountry FROM Customers;").PrintToConsole();

            // Select the last available country from Customers table.
            db.ExecuteQuery("SELECT Country FROM Customers ORDER BY CustomerID DESC LIMIT 1;").PrintToConsole();
        }

        static void SqlMaxExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select max unit price of all products.
            db.ExecuteQuery("SELECT MAX(UnitPrice) FROM Products;").PrintToConsole();

            // Select all products with max unit price.
            db.ExecuteQuery("SELECT ProductName, UnitPrice FROM Products WHERE UnitPrice = (SELECT MAX(UnitPrice) FROM Products);").PrintToConsole();
        }

        static void SqlMinExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select min unit price of all products.
            db.ExecuteQuery("SELECT MIN(UnitPrice) FROM Products;").PrintToConsole();

            // Select all products with min unit price.
            db.ExecuteQuery("SELECT ProductName, UnitPrice FROM Products WHERE UnitPrice = (SELECT MIN(UnitPrice) FROM Products);").PrintToConsole();
        }

        static void SqlSumExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select total count of products in stock.
            db.ExecuteQuery("SELECT SUM(UnitsInStock) FROM Products;").PrintToConsole();
        }

        static void SqlIfNullExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select total price of products ignoring NULL.
            db.ExecuteQuery("SELECT ProductName, UnitPrice * (UnitsInStock + IFNULL(UnitsOnOrder, 0)) FROM Products;").PrintToConsole();

            // Select price of products in orders.
            db.ExecuteQuery("SELECT ProductName, UnitPrice * (UnitsInStock + NULLIF(UnitsOnOrder, 0)) FROM Products;").PrintToConsole();
        }
    }
}
