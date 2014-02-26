using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Restriction Operators")]
        [Description("This example uses the where clause to find all elements of an array with a value less than 5.")]
        static void LinqWhereSimple1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            
            var lowNums =
                from num in numbers
                where (num < 5)
                select num;

            // Fluent expression equivalent.
            // var lowNums = numbers.Where(num => num < 5);

            Console.WriteLine("Numbers < 5:");
            foreach (var x in lowNums)
            {
                Console.WriteLine(x);
            }
        }

        [Category("Restriction Operators")]
        [Description("This example uses the where clause to find all products that are out of stock.")]
        static void LinqWhereSimple2(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var soldOutProducts =
                from prod in storage.Products
                where (prod.UnitsInStock == 0)
                select prod;

            // Fluent expression equivalent.
            // var soldOutProducts = storage.Products.Where(prod => prod.UnitsInStock == 0);

            Console.WriteLine("Sold out products:");
            foreach (var product in soldOutProducts)
            {
                Console.WriteLine("{0} is sold out!", product.ProductName);
            }
        }

        [Category("Restriction Operators")]
        [Description("This example uses the where clause to find all products that are in stock and cost more than 3.00 per unit.")]
        static void LinqWhereSimple3(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var expensiveInStockProducts =
                from prod in storage.Products
                where ((prod.UnitsInStock > 0) && (prod.UnitPrice > 3.00M))
                select prod;

            // Fluent expression equivalent.
            // var expensiveInStockProducts = storage.Products.Where(prod => (prod.UnitsInStock > 0) && (prod.UnitPrice > 3.00M));

            Console.WriteLine("In-stock products that cost more than 3.00:");
            foreach (var product in expensiveInStockProducts)
            {
                Console.WriteLine("{0} is in stock and costs more than 3.00.", product.ProductName);
            }
        }

        [Category("Restriction Operators")]
        [Description("This example uses the where clause to find all customers in Washington and then it uses a foreach loop " +
                     "to iterate over the orders collection that belongs to each customer.")]
        static void LinqWhereDrilldown(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var waCustomers =
                from cust in storage.Customers
                where (cust.Region == "WA")
                select cust;

            // Fluent expression equivalent.
            // var waCustomers = storage.Customers.Where(cust => cust.Region == "WA");

            Console.WriteLine("Customers from Washington and their orders:");
            foreach (var customer in waCustomers)
            {
                Console.WriteLine("Customer {0}: {1}", customer.CustomerId, customer.CompanyName);
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine("  Order {0}: {1}", order.OrderId, order.OrderDate);
                }
            }
        }

        [Category("Restriction Operators")]
        [Description("This example demonstrates an indexed where clause that returns digits whose name is shorter " +
                     "than their value.")]
        static void LinqWhereIndexed()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            // Fluent expression only.
            var shortDigits = digits.Where((digit, index) => digit.Length < index);

            Console.WriteLine("Short digits:");
            foreach (var d in shortDigits)
            {
                Console.WriteLine("The word {0} is shorter than its value.", d);
            }
        }
    }
}
