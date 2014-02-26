using System;
using System.Reflection;

namespace NDepth.Examples.Database.SQLExamples
{
    partial class Program
    {
        static void SqlInnerJoinExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select count of employees and customers from each city.
            db.ExecuteQuery("SELECT COUNT(DISTINCT e.EmployeeID) AS numEmployees, e.City AS EmployeesCity, c.City AS CustomersCity, COUNT(DISTINCT c.CustomerID) AS numCustomers " +
                            "FROM Employees e " +
                                "INNER JOIN Customers c ON (e.City = c.City) " +
                            "GROUP BY e.City, c.City " +
                            "ORDER BY e.City;").PrintToConsole();
        }

        static void SqlLeftJoinExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Select count of employees and customers from each employee city.
            db.ExecuteQuery("SELECT COUNT(DISTINCT e.EmployeeID) AS numEmployees, e.City AS EmployeesCity, c.City AS CustomersCity, COUNT(DISTINCT c.CustomerID) AS numCustomers " +
                            "FROM Employees e " +
                                "LEFT JOIN Customers c ON (e.City = c.City) " +
                            "GROUP BY e.City, c.City " +
                            "ORDER BY e.City;").PrintToConsole();
        }

        /*
        static void SqlRightJoinExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");
          
            // SQLite doesn't support right join.

            // Select count of employees and customers from each employee city.
            db.ExecuteQuery("SELECT COUNT(DISTINCT e.EmployeeID) AS numEmployees, e.City AS EmployeesCity, c.City AS CustomersCity, COUNT(DISTINCT c.CustomerID) AS numCustomers " +
                            "FROM Employees e " +
                                "RIGHT JOIN Customers c ON (e.City = c.City) " +
                            "GROUP BY e.City, c.City " +
                            "ORDER BY e.City;").PrintToConsole();
        }

        static void SqlFullOuterJoinExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");
          
            // SQLite doesn't support full outer join.

            // Select count of employees and customers from each city.
            db.ExecuteQuery("SELECT COUNT(DISTINCT e.EmployeeID) AS numEmployees, e.City AS EmployeesCity, c.City AS CustomersCity, COUNT(DISTINCT c.CustomerID) AS numCustomers " +
                            "FROM Employees e " +
                                "FULL OUTER JOIN Customers c ON (e.City = c.City) " +
                            "GROUP BY e.City, c.City " +
                            "ORDER BY e.City;").PrintToConsole();
        }
        */
    }
}
