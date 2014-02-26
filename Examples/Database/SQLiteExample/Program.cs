using System;
using System.Collections.Generic;
using System.Data;

namespace NDepth.Examples.Database.SQLiteExample
{
    class Program
    {
        static void Main()
        {
            using (var db = new Database())
            {
                // Create database schema.
                db.CreateSchema();

                // Fill database with sample data.
                db.FillWithData();

                // Insert new customer.
                var customer = new Dictionary<string, string>
                {
                    {"FirstName", "Bjørn"},
                    {"LastName", "Hansen"},
                    {"Address", "Ullevålsveien 14, Oslo, Norway, 0171"},
                    {"Email", "bjorn.hansen@yahoo.no"},
                    {"Phone", ""}
                };
                db.InsertRecord("Customer", customer);

                // Update new customer.
                customer = new Dictionary<string, string>
                {
                    {"Phone", "+47 22 44 22 22"}
                };
                db.UpdateRecord("Customer", customer, "FirstName = 'Bjørn'");

                // Get all of customers.
                var customers = db.ExecuteQuery("SELECT * FROM Customer");
                foreach (DataRow row in customers.Rows)
                {
                    foreach (var column in row.ItemArray)
                        Console.Write(" {0}", column);
                    Console.WriteLine();
                }

                // Delete new customer.
                db.DeleteRecord("Customer", "FirstName = 'Bjørn'");

                // Get count of customers.
                var count = db.ExecuteScalar("SELECT COUNT(*) FROM Customer");
                Console.WriteLine("Customers count = " + count);

                // Clear customers table.
                db.ClearTable("Customer");

                // Clear DB.
                db.ClearDb();
            }
        }
    }
}
