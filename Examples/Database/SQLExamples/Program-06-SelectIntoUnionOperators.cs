using System;
using System.Reflection;

namespace NDepth.Examples.Database.SQLExamples
{
    partial class Program
    {
        static void SqlSelectIntoExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create temp table and select all customers into it.
            // Console.WriteLine("Query returns: " + db.ExecuteNonQuery("CREATE TABLE CustomersTemp AS SELECT CompanyName, Country FROM Customers;"));

            // Create temp table for customers.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("CREATE TABLE CustomersTemp (Name TEXT, Country TEXT);"));

            // Select all customers into the temp table.
            // db.ExecuteQuery("SELECT CompanyName, Country INTO CustomersTemp FROM Customers;").PrintToConsole();

            // Select all customers into the temp table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO CustomersTemp SELECT CompanyName, Country FROM Customers;"));

            // Select all records from CustomersTemp table.
            db.ExecuteQuery("SELECT * FROM CustomersTemp;").PrintToConsole();

            // Drop CustomersTemp table.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DROP TABLE CustomersTemp;"));
        }

        static void SqlUnionExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Union customers and employees from 'UK'.
            db.ExecuteQuery("SELECT ContactName, 'Customer', Country FROM Customers WHERE Country = 'UK' " +
                            "UNION " +
                            "SELECT FirstName, 'Employee', Country FROM Employees WHERE Country = 'UK';").PrintToConsole();

            // Union all customers and employees from 'UK'.
            db.ExecuteQuery("SELECT ContactName, 'Customer', Country FROM Customers WHERE Country = 'UK' " +
                            "UNION ALL " +
                            "SELECT FirstName, 'Employee', Country FROM Employees WHERE Country = 'UK';").PrintToConsole();
        }
    }
}
