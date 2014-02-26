using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Aggregate Operators")]
        [Description("This example uses Count to get the number of unique prime factors of 300.")]
        static void LinqCountSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] factorsOf300 = { 2, 2, 3, 5, 5 };

            var uniqueFactors = factorsOf300.Distinct().Count();

            Console.WriteLine("There are {0} unique factors of 300.", uniqueFactors);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Count to get the number of odd ints in the array.")]
        static void LinqCountConditional()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var oddNumbers = numbers.Count(n => n % 2 == 1);

            Console.WriteLine("There are {0} odd numbers in the list.", oddNumbers); 
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Count to return a list of customers and how many orders each has.")]
        static void LinqCountNested(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var orderCounts =
                from c in storage.Customers
                select new { c.CustomerId, OrderCount = c.Orders.Count() };

            // Fluent expression equivalent.
            // var orderCounts = storage.Customers.Select(c => new { c.CustomerId, OrderCount = c.Orders.Count() });

            ObjectDumper.Write(orderCounts); 
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Count to return a list of categories and how many products each has.")]
        static void LinqCountGrouped(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var categoryCounts =
                from p in storage.Products
                group p by p.Category into g
                select new { Category = g.Key, ProductCount = g.Count() };

            // Fluent expression equivalent.
            // var categoryCounts = storage.Products.GroupBy(p => p.Category, (k, g) => new { Category = k, ProductCount = g.Count() });

            ObjectDumper.Write(categoryCounts);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses LongCount to get the number elements in a huge generated sequence.")]
        static void LinqLongCountSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var hugeSequence = Enumerable.Range(0, int.MaxValue).Concat(Enumerable.Range(0, int.MaxValue));

            var longCount = hugeSequence.LongCount();

            Console.WriteLine("There are {0} elements in the huge sequence.", longCount);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses LongCount to get the number of odd ints in a huge generated sequence.")]
        static void LinqLongCountConditional()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var hugeSequence = Enumerable.Range(0, int.MaxValue).Concat(Enumerable.Range(0, int.MaxValue));

            var longCount = hugeSequence.LongCount(n => n % 2 == 1);

            Console.WriteLine("There are {0} odd numbers in the huge sequence.", longCount);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Sum to add all the numbers in an array.")]
        static void LinqSumSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var numSum = numbers.Sum();

            Console.WriteLine("The sum of the numbers is {0}.", numSum);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Sum to get the total number of characters of all words in the array.")]
        static void LinqSumProjection()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "cherry", "apple", "blueberry" };

            var totalChars = words.Sum(w => w.Length);

            Console.WriteLine("There are a total of {0} characters in these words.", totalChars);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Sum to get the total units in stock for each product category.")]
        static void LinqSumGrouped(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var categories =
                from p in storage.Products
                group p by p.Category into g
                select new { Category = g.Key, TotalUnitsInStock = g.Sum(p => p.UnitsInStock) };

            // Fluent expression equivalent.
            // var categories = storage.Products.GroupBy(p => p.Category, (k, g) => new { Category = k, TotalUnitsInStock = g.Sum(p => p.UnitsInStock) });

            ObjectDumper.Write(categories);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Min to get the lowest number in an array.")]
        static void LinqMinSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var minNum = numbers.Min();

            Console.WriteLine("The minimum number is {0}.", minNum); 
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Min to get the length of the shortest word in an array.")]
        static void LinqMinProjection()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "cherry", "apple", "blueberry" };

            var shortestWord = words.Min(w => w.Length);

            Console.WriteLine("The shortest word is {0} characters long.", shortestWord);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Min to get the cheapest price among each category's products.")]
        static void LinqMinGrouped(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var categories =
                from p in storage.Products
                group p by p.Category into g
                select new { Category = g.Key, CheapestPrice = g.Min(p => p.UnitPrice) };

            // Fluent expression equivalent.
            // var categories = storage.Products.GroupBy(p => p.Category, (k, g) => new { Category = k, CheapestPrice = g.Min(p => p.UnitPrice) });

            ObjectDumper.Write(categories); 
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Min to get the products with the lowest price in each category.")]
        static void LinqMinElements(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var categories =
                from p in storage.Products
                group p by p.Category into g
                let minPrice = g.Min(p => p.UnitPrice)
                select new { Category = g.Key, CheapestProducts = g.Where(p => p.UnitPrice == minPrice) };

            ObjectDumper.Write(categories, 1);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Max to get the highest number in an array. Note that the method returns a single value.")]
        static void LinqMaxSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var maxNum = numbers.Max();

            Console.WriteLine("The maximum number is {0}.", maxNum); 
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Max to get the length of the longest word in an array.")]
        static void LinqMaxProjection()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "cherry", "apple", "blueberry" };

            var longestWord = words.Max(w => w.Length);

            Console.WriteLine("The longest word is {0} characters long.", longestWord);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Max to get the most expensive price among each category's products.")]
        static void LinqMaxGrouped(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var categories =
                from p in storage.Products
                group p by p.Category into g
                select new { Category = g.Key, MostExpensivePrice = g.Max(p => p.UnitPrice) };

            // Fluent expression equivalent.
            // var categories = storage.Products.GroupBy(p => p.Category, (k, g) => new { Category = k, MostExpensivePrice = g.Max(p => p.UnitPrice) });

            ObjectDumper.Write(categories); 
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Max to get the products with the most expensive price in each category.")]
        static void LinqMaxElements(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var categories =
                from p in storage.Products
                group p by p.Category into g
                let maxPrice = g.Max(p => p.UnitPrice)
                select new { Category = g.Key, MostExpensiveProducts = g.Where(p => p.UnitPrice == maxPrice) };

            ObjectDumper.Write(categories, 1);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Average to get the average of all numbers in an array.")]
        static void LinqAverageSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var averageNum = numbers.Average();

            Console.WriteLine("The average number is {0}.", averageNum); 
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Average to get the average length of the words in the array.")]
        static void LinqAverageProjection()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "cherry", "apple", "blueberry" };

            var averageLength = words.Average(w => w.Length);

            Console.WriteLine("The average word length is {0} characters.", averageLength); 
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Average to get the average price of each category's products.")]
        static void LinqAverageGrouped(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var categories =
                from p in storage.Products
                group p by p.Category into g
                select new { Category = g.Key, AveragePrice = g.Average(p => p.UnitPrice) };

            // Fluent expression equivalent.
            // var categories = storage.Products.GroupBy(p => p.Category, (k, g) => new { Category = k, AveragePrice = g.Average(p => p.UnitPrice) });

            ObjectDumper.Write(categories);
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Aggregate to create a running product on the array that " +
                     "calculates the total product of all elements.")]
        static void LinqAggregateSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

            var product = doubles.Aggregate((runningProduct, nextFactor) => runningProduct * nextFactor);

            Console.WriteLine("Total product of all numbers: {0}", product); 
        }

        [Category("Aggregate Operators")]
        [Description("This example uses Aggregate to create a running account balance that " +
                     "subtracts each withdrawal from the initial balance of 100, as long as " +
                     "the balance never drops below 0.")]
        static void LinqAggregateSeed()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            const double startBalance = 100.0;

            int[] attemptedWithdrawals = { 20, 10, 40, 50, 10, 70, 30 };

            var endBalance =
                attemptedWithdrawals.Aggregate(startBalance,
                    (balance, nextWithdrawal) =>
                        ((nextWithdrawal <= balance) ? (balance - nextWithdrawal) : balance));

            Console.WriteLine("Ending balance: {0}", endBalance); 
        }
    }
}
