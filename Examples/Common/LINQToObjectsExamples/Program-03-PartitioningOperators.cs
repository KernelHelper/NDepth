using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Partitioning Operators")]
        [Description("This example uses Take to get only the first 3 elements of the array.")]
        static void LinqTakeSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var first3Numbers = numbers.Take(3);

            Console.WriteLine("First 3 numbers:");
            foreach (var n in first3Numbers)
            {
                Console.WriteLine(n);
            }
        }

        [Category("Partitioning Operators")]
        [Description("This example uses Take to get the first 3 orders from customers in Washington.")]
        static void LinqTakeNested(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var first3WaOrders = (
                from c in storage.Customers
                from o in c.Orders
                where c.Region == "WA"
                select new { c.CustomerId, o.OrderId, o.OrderDate }).Take(3);

            Console.WriteLine("First 3 orders in WA:");
            foreach (var order in first3WaOrders)
            {
                ObjectDumper.Write(order);
            } 
        }

        [Category("Partitioning Operators")]
        [Description("This example uses Skip to get all but the first four elements of the array.")]
        static void LinqSkipSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var allButFirst4Numbers = numbers.Skip(4);

            Console.WriteLine("All but first 4 numbers:");
            foreach (var n in allButFirst4Numbers)
            {
                Console.WriteLine(n);
            }
        }

        [Category("Partitioning Operators")]
        [Description("This example uses Take to get all but the first 2 orders from customers in Washington.")]
        static void LinqSkipNested(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var waOrders =
                from cust in storage.Customers
                from order in cust.Orders
                where cust.Region == "WA"
                select new { cust.CustomerId, order.OrderId, order.OrderDate };

            var allButFirst2Orders = waOrders.Skip(2);

            Console.WriteLine("All but first 2 orders in WA:");
            foreach (var order in allButFirst2Orders)
            {
                ObjectDumper.Write(order);
            }
        }

        [Category("Partitioning Operators")]
        [Description("This example uses TakeWhile to return elements starting from the beginning of the array " +
                     "until a number is read whose value is not less than 6.")]
        static void LinqTakeWhileSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var firstNumbersLessThan6 = numbers.TakeWhile(n => n < 6);

            Console.WriteLine("First numbers less than 6:");
            foreach (var num in firstNumbersLessThan6)
            {
                Console.WriteLine(num);
            }
        }

        [Category("Partitioning Operators")]
        [Description("This example uses TakeWhile to return elements starting from the beginning of the array " +
                     "until a number is hit that is less than its position in the array.")]
        static void LinqTakeWhileIndexed()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var firstSmallNumbers = numbers.TakeWhile((n, index) => n >= index);

            Console.WriteLine("First numbers not less than their position:");
            foreach (var n in firstSmallNumbers)
            {
                Console.WriteLine(n);
            }
        }

        [Category("Partitioning Operators")]
        [Description("This example uses SkipWhile to get the elements of the array starting from the first " +
                     "element divisible by 3.")]
        static void LinqSkipWhileSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            // In the lambda expression, 'n' is the input parameter that identifies each
            // element in the collection in succession. It is is inferred to be
            // of type int because numbers is an int array.
            var allButFirst3Numbers = numbers.SkipWhile(n => n % 3 != 0);

            Console.WriteLine("All elements starting from first element divisible by 3:");
            foreach (var n in allButFirst3Numbers)
            {
                Console.WriteLine(n);
            }
        }

        [Category("Partitioning Operators")]
        [Description("This example uses SkipWhile to get the elements of the array starting from the first " +
                     "element less than its position.")]
        static void LinqSkipWhileIndexed()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var laterNumbers = numbers.SkipWhile((n, index) => n >= index);

            Console.WriteLine("All elements starting from first element less than its position:");
            foreach (var n in laterNumbers)
            {
                Console.WriteLine(n);
            }
        }
    }
}
