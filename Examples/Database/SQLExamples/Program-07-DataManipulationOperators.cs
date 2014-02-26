using System;
using System.Reflection;

namespace NDepth.Examples.Database.SQLExamples
{
    partial class Program
    {
        static void SqlInsertExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Insert a new category with "values only" form.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO Categories VALUES (NULL, 'Test1', 'Test Category 1', NULL);"));

            // Insert a new category with "columns" form.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("INSERT INTO Categories (CategoryName, Description) VALUES ('Test2', 'Test Category 2');"));

            // Select all records from Categories table.
            db.ExecuteQuery("SELECT * FROM Categories;").PrintToConsole();
        }

        static void SqlUpdateExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Update new category records.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("UPDATE Categories SET CategoryName = 'CoolTest', Description = 'Cool Test Category' WHERE CategoryName LIKE 'Test%';"));

            // Select all records from Categories table.
            db.ExecuteQuery("SELECT * FROM Categories;").PrintToConsole();

            // Update warning: Be careful when updating records. If we had omitted the WHERE clause in the example above
            // all categories will be updated!

            // Update all category records.
            // Console.WriteLine("Query returns: " + db.ExecuteNonQuery("UPDATE Categories SET CategoryName = 'CoolTest', Description = 'Cool Test Category';"));

            // Select all records from Categories table.
            // db.ExecuteQuery("SELECT * FROM Categories;").PrintToConsole();
        }

        static void SqlDeleteExample(Database db)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Delete new category records.
            Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DELETE FROM Categories WHERE CategoryName LIKE 'CoolTest%';"));

            // Select all records from Categories table.
            db.ExecuteQuery("SELECT * FROM Categories;").PrintToConsole();

            // Update warning: Be careful when deleting records. If we had omitted the WHERE clause in the example above
            // all categories will be deleted!

            // Delete all category records.
            // Console.WriteLine("Query returns: " + db.ExecuteNonQuery("DELETE FROM Categories;"));

            // Select all records from Categories table.
            // db.ExecuteQuery("SELECT * FROM Categories;").PrintToConsole();
        }
    }
}
