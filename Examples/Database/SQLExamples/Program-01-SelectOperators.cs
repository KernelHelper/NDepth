using System;
using System.Reflection;

namespace NDepth.Examples.Database.SQLExamples
{
    partial class Program
    {
        static void SqlSelectExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select all records from Categories table.
            db.ExecuteQuery("SELECT * FROM Categories;").PrintToConsole();

            // Select some columns in all records from Customers table.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers;").PrintToConsole();
        }

        static void SqlSelectDistinctExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select unique countries from Customers table.
            db.ExecuteQuery("SELECT DISTINCT Country " +
                            "FROM Customers;").PrintToConsole();
        }

        static void SqlSelectWhereExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select all customers from USA.
            db.ExecuteQuery("SELECT CompanyName, ContactName, Country " +
                            "FROM Customers " +
                            "WHERE Country = 'USA';").PrintToConsole();
            
            // Select all customers not from USA.
            db.ExecuteQuery("SELECT CompanyName, ContactName, Country " +
                            "FROM Customers " +
                            "WHERE Country <> 'USA';").PrintToConsole();

            // Select different categories.
            db.ExecuteQuery("SELECT * FROM Categories WHERE CategoryId < 4;").PrintToConsole();
            db.ExecuteQuery("SELECT * FROM Categories WHERE CategoryId <= 4;").PrintToConsole();
            db.ExecuteQuery("SELECT * FROM Categories WHERE CategoryId > 5;").PrintToConsole();
            db.ExecuteQuery("SELECT * FROM Categories WHERE CategoryId >= 5;").PrintToConsole();
            db.ExecuteQuery("SELECT * FROM Categories WHERE CategoryId BETWEEN 2 AND 5;").PrintToConsole();
            db.ExecuteQuery("SELECT * FROM Categories WHERE CategoryId IN (2, 4, 6);").PrintToConsole();
            db.ExecuteQuery("SELECT * FROM Categories WHERE CategoryName LIKE 'Con%';").PrintToConsole();
        }

        static void SqlSelectWhereAndOrExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select all company owners from USA.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "WHERE ContactTitle = 'Owner' AND Country = 'USA';").PrintToConsole();

            // Select all company owners or sales.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "WHERE ContactTitle = 'Owner' OR ContactTitle LIKE 'Sales%';").PrintToConsole();

            // Select all company owners or sales from USA.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "WHERE (ContactTitle = 'Owner' OR ContactTitle LIKE 'Sales%') AND Country = 'USA';").PrintToConsole();
        }

        static void SqlSelectWhereIsNullExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select all company owners from NULL country.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "WHERE Country IS NULL;").PrintToConsole();

            // Select all company owners from NOT NULL country.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "WHERE Country IS NOT NULL;").PrintToConsole();
        }

        static void SqlSelectWhereLikeExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select all customers using wildcard matching.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, City " +
                            "FROM Customers " +
                            "WHERE City LIKE 'San %';").PrintToConsole();
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, City " +
                            "FROM Customers " +
                            "WHERE City LIKE 'Sa_ %';").PrintToConsole();
        }

        static void SqlSelectWhereInExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select all customers from 'USA', 'Canada', 'UK'.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "WHERE Country IN ('USA', 'Canada', 'UK');").PrintToConsole();

            // Select all customers from countries with name length <= 4.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "WHERE Country IN (SELECT Country FROM Customers WHERE length(Country) <= 4);").PrintToConsole();
        }

        static void SqlSelectWhereBetweenExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select all customers from country between 'Canada' and 'France'.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "WHERE Country BETWEEN 'Canada' AND 'France';").PrintToConsole();

            // Select all customers from country not between 'Canada' and 'France'.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "WHERE Country NOT BETWEEN 'Canada' AND 'France';").PrintToConsole();
        }

        static void SqlSelectGroupByExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select regions with count of customers in each.
            db.ExecuteQuery("SELECT Region, COUNT(CustomerId) AS CustomersCount " +
                            "FROM Customers " +
                            "GROUP BY Region;").PrintToConsole();

            // Select not NULL regions with count of customers more than 2 in each.
            db.ExecuteQuery("SELECT Region, COUNT(CustomerId) AS CustomersCount " +
                            "FROM Customers " +
                            "GROUP BY Region " +
                            "HAVING (Region NOT NULL) AND (COUNT(CustomerId) > 2);").PrintToConsole();
        }

        static void SqlSelectOrderByExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select all customers ordered by country (ascending order by default).
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "ORDER BY Country;").PrintToConsole();

            // Select all customers ordered by country ascending order.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "ORDER BY Country ASC;").PrintToConsole();

            // Select all customers ordered by country descending order.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers " +
                            "ORDER BY Country DESC;").PrintToConsole();

            // Select all customers ordered by country than by contact title.
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country " +
                            "FROM Customers ORDER BY Country DESC, ContactTitle ASC;").PrintToConsole();
        }

        static void SqlSelectAliasExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select top 10 orders from 'UK' customers handled by 'USA' employees.
            db.ExecuteQuery("SELECT o.OrderId, c.ContactName, c.Country, e.FirstName, e.Country " +
                            "FROM Orders as o, Customers as c, Employees as e " +
                            "WHERE c.Country = 'UK' AND e.Country = 'USA' " +
                            "LIMIT 10;").PrintToConsole();
        }

        static void SqlSelectTopExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select top 10 customers (TOP syntax).
            // db.ExecuteQuery("SELECT TOP 10 CompanyName, ContactName, ContactTitle, Country FROM Customers;").PrintToConsole();

            // Select top 50% customers (TOP syntax).
            // db.ExecuteQuery("SELECT TOP 50 PERCENT CompanyName, ContactName, ContactTitle, Country FROM Customers;").PrintToConsole();

            // Select top 10 customers (LIMIT syntax).
            db.ExecuteQuery("SELECT CompanyName, ContactName, ContactTitle, Country FROM Customers LIMIT 10;").PrintToConsole();
        }

        static void SqlSelectCaseExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select customers with ordered <unknown>/USA/Worldwide location.
            db.ExecuteQuery("SELECT CompanyName, ContactName, Country, " +
                            "(CASE " +
                                "WHEN c.Country IS NULL THEN '<unknown>' " +
                                "WHEN c.Country = 'USA' THEN 'USA' " +
                                "ELSE 'Worldwide' " +
                            "END) AS Location " +
                            "FROM Customers c " +
                            "ORDER BY 4;").PrintToConsole();
        }
    }
}
