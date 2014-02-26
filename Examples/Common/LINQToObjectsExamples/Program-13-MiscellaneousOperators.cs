using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Miscellaneous Operators")]
        [Description("This example uses DefaultIfEmpty to get default item in case the collection is empty.")]
        static void LinqDefaultIfEmpty()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbersA = { 1, 2, 3, 4, 5 };
            int[] numbersB = { };
            int[] numbersC = { };

            Console.WriteLine("Numbers from the first array:");
            foreach (var n in numbersA.DefaultIfEmpty())
            {
                Console.WriteLine(n);
            }

            Console.WriteLine("Numbers from the second array:");
            foreach (var n in numbersB.DefaultIfEmpty())
            {
                Console.WriteLine(n);
            }

            Console.WriteLine("Numbers from the third array:");
            foreach (var n in numbersC.DefaultIfEmpty(100))
            {
                Console.WriteLine(n);
            }
        }

        [Category("Miscellaneous Operators")]
        [Description("This example uses Concat to create one sequence that contains each array's " +
                     "values, one after the other.")]
        static void LinqConcat1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            var allNumbers = numbersA.Concat(numbersB);

            Console.WriteLine("All numbers from both arrays:");
            foreach (var n in allNumbers)
            {
                Console.WriteLine(n);
            }
        }

        [Category("Miscellaneous Operators")]
        [Description("This example uses Concat to create one sequence that contains the names of " +
                     "all customers and products, including any duplicates.")]
        static void LinqConcat2(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var customerNames =
                from c in storage.Customers
                select c.CompanyName;
            var productNames =
                from p in storage.Products
                select p.ProductName;

            var allNames = customerNames.Concat(productNames);

            Console.WriteLine("Customer and product names:");
            foreach (var n in allNames)
            {
                Console.WriteLine(n);
            } 
        }

        [Category("Miscellaneous Operators")]
        [Description("This example uses SequenceEquals to see if two sequences match on all elements " +
                     "in the same order.")]
        static void LinqSequenceEqual1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var wordsA = new [] { "cherry", "apple", "blueberry" };
            var wordsB = new [] { "cherry", "apple", "blueberry" };

            var match = wordsA.SequenceEqual(wordsB);

            Console.WriteLine("The sequences match: {0}", match); 
        }

        [Category("Miscellaneous Operators")]
        [Description("This example uses SequenceEqual to see if two sequences match on all elements " +
                     "in the same order.")]
        static void LinqSequenceEqual2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var wordsA = new [] { "cherry", "apple", "blueberry" };
            var wordsB = new [] { "apple", "blueberry", "cherry" };

            var match = wordsA.SequenceEqual(wordsB);

            Console.WriteLine("The sequences match: {0}", match);
        }

        [Category("Miscellaneous Operators")]
        [Description("The following code example demonstrates how to use the Zip method to merge two sequences.")]
        static void LinqZip()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var numbers = new [] { 1, 2, 3, 4 };
            var words = new [] { "one", "two", "three" };

            var numbersAndWords = numbers.Zip(words, (first, second) => first + " is " + second);

            Console.WriteLine("Zipped collection is:");
            foreach (var n in numbersAndWords)
            {
                Console.WriteLine(n);
            }
        }
    }
}
