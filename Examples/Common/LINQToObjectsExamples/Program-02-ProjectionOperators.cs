using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Projection Operators")]
        [Description("This example uses select to produce a sequence of ints one higher than those in an existing array of ints.")]
        static void LinqSelectSimple1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var numsPlusOne =
                from n in numbers
                select n + 1;

            // Fluent expression equivalent.
            // var numsPlusOne = numbers.Select(num => num + 1);

            Console.WriteLine("Numbers + 1:");
            foreach (var i in numsPlusOne)
            {
                Console.WriteLine(i);
            }
        }

        [Category("Projection Operators")]
        [Description("This example uses select to return a sequence of just the names of a list of products.")]
        static void LinqSelectSimple2(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var productNames =
                from p in storage.Products
                select p.ProductName;

            // Fluent expression equivalent.
            // var productNames = storage.Products.Select(p => p.ProductName);

            Console.WriteLine("Product Names:");
            foreach (var productName in productNames)
            {
                Console.WriteLine(productName);
            } 
        }

        [Category("Projection Operators")]
        [Description("This example uses select to produce a sequence of strings representing the text version of a sequence of ints.")]
        static void LinqSelectTransformation()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var textNums =
                from n in numbers
                select strings[n];

            // Fluent expression equivalent.
            // var textNums = numbers.Select(n => strings[n]);

            Console.WriteLine("Number strings:");
            foreach (var s in textNums)
            {
                Console.WriteLine(s);
            } 
        }

        [Category("Projection Operators")]
        [Description("This example uses select to produce a sequence of the uppercase and lowercase versions of each word in the original array.")]
        static void LinqSelectAnonymousTypes1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "aPPLE", "BlUeBeRrY", "cHeRry" };

            var upperLowerWords =
                from w in words
                select new { Upper = w.ToUpper(), Lower = w.ToLower() };

            // Fluent expression equivalent.
            // var upperLowerWords = words.Select(w => new { Upper = w.ToUpper(), Lower = w.ToLower() });

            foreach (var ul in upperLowerWords)
            {
                Console.WriteLine("Uppercase: {0}, Lowercase: {1}", ul.Upper, ul.Lower);
            } 
        }

        [Category("Projection Operators")]
        [Description("This example uses select to produce a sequence containing text representations of digits and whether their length is even or odd.")]
        static void LinqSelectAnonymousTypes2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var digitOddEvens =
                from n in numbers
                select new { Digit = strings[n], Even = (n % 2 == 0) };

            // Fluent expression equivalent.
            // var digitOddEvens = numbers.Select(n => new { Digit = strings[n], Even = (n % 2 == 0) });

            foreach (var d in digitOddEvens)
            {
                Console.WriteLine("The digit {0} is {1}.", d.Digit, d.Even ? "even" : "odd");
            }
        }

        [Category("Projection Operators")]
        [Description("This example uses select to produce a sequence containing some properties of Products, " +
                     "including UnitPrice which is renamed to Price in the resulting type.")]
        static void LinqSelectAnonymousTypes3(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var productInfos =
                from p in storage.Products
                select new { p.ProductName, p.Category, Price = p.UnitPrice };

            // Fluent expression equivalent.
            // var productInfos = storage.Products.Select(p => new { p.ProductName, p.Category, Price = p.UnitPrice });

            Console.WriteLine("Product Info:");
            foreach (var productInfo in productInfos)
            {
                Console.WriteLine("{0} is in the category {1} and costs {2} per unit.", productInfo.ProductName, productInfo.Category, productInfo.Price);
            }
        }

        [Category("Projection Operators")]
        [Description("This example uses an indexed Select clause to determine if the value of ints in an array " +
                     "match their position in the array.")]
        static void LinqSelectIndexed()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            // Fluent expression only.
            var numsInPlace = numbers.Select((num, index) => new { Num = num, InPlace = (num == index) });

            Console.WriteLine("Number: In-place?");
            foreach (var n in numsInPlace)
            {
                Console.WriteLine("{0}: {1}", n.Num, n.InPlace);
            } 
        }

        [Category("Projection Operators")]
        [Description("This example combines select and where to make a simple query that returns the text form of " +
                     "each digit less than 5.")]
        static void LinqSelectFiltered()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var lowNums =
                from n in numbers
                where n < 5
                select digits[n];

            // Fluent expression equivalent.
            // var lowNums = numbers.Where(n => n < 5).Select(n => digits[n]);

            Console.WriteLine("Numbers < 5:");
            foreach (var num in lowNums)
            {
                Console.WriteLine(num);
            } 
        }

        [Category("Projection Operators")]
        [Description("This example uses a compound from clause to make a query that returns all pairs of numbers from " +
                     "both arrays such that the number from numbersA is less than the number from numbersB.")]
        static void LinqSelectManyCompoundFrom1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            var pairs =
                from a in numbersA
                from b in numbersB
                where a < b
                select new { a, b };

            // Fluent expression equivalents.
            // var pairs = numbersA.SelectMany(a => numbersB.Where(b => a < b).Select(b => new { a, b }));
            // var pairs = numbersA.SelectMany(a => numbersB.Where(b => a < b), (a, b) => new {a, b});

            Console.WriteLine("Pairs where a < b:");
            foreach (var pair in pairs)
            {
                Console.WriteLine("{0} is less than {1}", pair.a, pair.b);
            } 
        }

        [Category("Projection Operators")]
        [Description("This example uses a compound from clause to select all orders where the order total is less than 500.00.")]
        static void LinqSelectManyCompoundFrom2(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var orders =
                from c in storage.Customers
                from o in c.Orders
                where o.Total < 500.00M
                select new { c.CustomerId, o.OrderId, o.Total };

            // Fluent expression equivalents.
            // var orders = storage.Customers.SelectMany(c => c.Orders.Where(o => o.Total < 500.00M).Select(o => new { c.CustomerId, o.OrderId, o.Total }));
            /*
            var orders = storage.Customers.SelectMany(
                c => c.Orders.Where(o => o.Total < 500.00M), 
                (c, o) => new { c.CustomerId, o.OrderId, o.Total });
            */ 

            ObjectDumper.Write(orders); 
        }

        [Category("Projection Operators")]
        [Description("This example uses a compound from clause to select all orders where the order was made in 1998 or later.")]
        static void LinqSelectManyCompoundFrom3(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var orders =
                from c in storage.Customers
                from o in c.Orders
                where o.OrderDate >= new DateTime(1998, 1, 1)
                select new { c.CustomerId, o.OrderId, o.OrderDate };

            // Fluent expression equivalents.
            // var orders = storage.Customers.SelectMany(c => c.Orders.Where(o => o.OrderDate >= new DateTime(1998, 1, 1)).Select(o => new { c.CustomerId, o.OrderId, o.OrderDate }));
            /*
            var orders = storage.Customers.SelectMany(
                c => c.Orders.Where(o => o.OrderDate >= new DateTime(1998, 1, 1)), 
                (c, o) => new {c.CustomerId, o.OrderId, o.OrderDate});
            */ 

            ObjectDumper.Write(orders);
        }

        [Category("Projection Operators")]
        [Description("This example uses a compound from clause to select all orders where the order total is greater than 2000.00 " +
                     "and uses from assignment to avoid requesting the total twice.")]
        static void LinqSelectManyFromAssignment(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var orders =
                from c in storage.Customers
                from o in c.Orders
                let total = o.Total
                where total >= 2000.0M
                select new { c.CustomerId, o.OrderId, total };

            // Fluent expression equivalents.
            // var orders = storage.Customers.SelectMany(c => c.Orders.Select(o => new { order = o, total = o.Total }).Where(o => o.total < 500.00M).Select(o => new { c.CustomerId, o.order.OrderId, o.total }));
            /*
            var orders = storage.Customers.SelectMany(
                c => c.Orders.Select(o => new { order = o, total = o.Total }).Where(o => o.total < 500.00M), 
                (c, o) => new { c.CustomerId, o.order.OrderId, o.total });
            */ 

            ObjectDumper.Write(orders);
        }

        [Category("Projection Operators")]
        [Description("This example uses multiple from clauses so that filtering on customers can be done before selecting " +
                     "their orders.  This makes the query more efficient by not selecting and then discarding orders for " +
                     "customers outside of Washington.")]
        static void LinqSelectManyMultipleFrom(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var cutoffDate = new DateTime(1997, 1, 1);

            var orders =
                from c in storage.Customers
                where c.Region == "WA"
                from o in c.Orders
                where o.OrderDate >= cutoffDate
                select new { c.CustomerId, o.OrderId };

            // Fluent expression equivalents.
            // var orders = storage.Customers.Where(c => c.Region == "WA").SelectMany(c => c.Orders.Where(o => o.OrderDate >= cutoffDate).Select(o => new { c.CustomerId, o.OrderId }));
            /*
            var orders = storage.Customers.Where(c => c.Region == "WA").SelectMany(
                c => c.Orders.Where(o => o.OrderDate >= cutoffDate),
                (c, o) => new { c.CustomerId, o.OrderId });
            */ 

            ObjectDumper.Write(orders);
        }

        [Category("Projection Operators")]
        [Description("This example uses an indexed SelectMany clause to select all orders, while referring to customers " +
                     "by the order in which they are returned from the query.")]
        static void LinqSelectManyIndexed(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Fluent expression only.
            var customerOrders = storage.Customers.SelectMany((c, cIndex) => c.Orders.Select(o => "Customer #" + (cIndex + 1) + " has an order with OrderID " + o.OrderId));
            /*
            var customerOrders = storage.Customers.SelectMany(
                (c, cIndex) => c.Orders.Select(o => new { index = cIndex, order = o }),
                (c, o) => "Customer #" + (o.index + 1) + " has an order with OrderID " + o.order.OrderId);
            */ 

            ObjectDumper.Write(customerOrders); 
        }
    }
}
