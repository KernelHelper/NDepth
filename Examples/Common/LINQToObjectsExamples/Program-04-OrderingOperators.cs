using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Ordering Operators")]
        [Description("This example uses orderby to sort a list of words alphabetically.")]
        static void LinqOrderBySimple1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "cherry", "apple", "blueberry" };

            var sortedWords =
                from word in words
                orderby word
                select word;

            // Fluent expression equivalent.
            // var sortedWords = words.OrderBy(word => word);

            Console.WriteLine("The sorted list of words:");
            foreach (var w in sortedWords)
            {
                Console.WriteLine(w);
            }
        }

        [Category("Ordering Operators")]
        [Description("This example uses orderby to sort a list of words by length.")]
        static void LinqOrderBySimple2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "cherry", "apple", "blueberry" };

            var sortedWords =
                from word in words
                orderby word.Length
                select word;

            // Fluent expression equivalent.
            // var sortedWords = words.OrderBy(word => word.Length);

            Console.WriteLine("The sorted list of words (by length):");
            foreach (var w in sortedWords)
            {
                Console.WriteLine(w);
            }
        }

        [Category("Ordering Operators")]
        [Description("This example uses orderby to sort a list of products by name. Use the 'descending' keyword " +
                     "at the end of the clause to perform a reverse ordering.")]
        static void LinqOrderBySimple3(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var sortedProducts =
                from prod in storage.Products
                orderby prod.ProductName
                select prod;

            // Fluent expression equivalent.
            // var sortedProducts = storage.Products.OrderBy(prod => prod.ProductName);

            ObjectDumper.Write(sortedProducts);
        }

        // Custom comparer for use with ordering operators
        private class CaseInsensitiveComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Category("Ordering Operators")]
        [Description("This example uses an OrderBy clause with a custom comparer to do a case-insensitive sort of the words in an array.")]
        static void LinqOrderByComparer()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            // Fluent expression only.
            var sortedWords = words.OrderBy(word => word, new CaseInsensitiveComparer());

            ObjectDumper.Write(sortedWords);
        }

        [Category("Ordering Operators")]
        [Description("This example uses orderby and descending to sort a list of doubles from highest to lowest.")]
        static void LinqOrderByDescendingSimple1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

            var sortedDoubles =
                from d in doubles
                orderby d descending
                select d;

            // Fluent expression equivalent.
            // var sortedDoubles = doubles.OrderByDescending(d => d);

            Console.WriteLine("The doubles from highest to lowest:");
            foreach (var d in sortedDoubles)
            {
                Console.WriteLine(d);
            }
        }

        [Category("Ordering Operators")]
        [Description("This example uses orderby to sort a list of products by units in stock from highest to lowest.")]
        static void LinqOrderByDescendingSimple2(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var sortedProducts =
                from prod in storage.Products
                orderby prod.UnitsInStock descending
                select prod;

            // Fluent expression equivalent.
            // var sortedProducts = storage.Products.OrderByDescending(prod => prod.UnitsInStock);

            ObjectDumper.Write(sortedProducts);
        }

        [Category("Ordering Operators")]
        [Description("This example uses method syntax to call OrderByDescending because it enables you to use a custom comparer.")]
        static void LinqOrderByDescendingComparer()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            // Fluent expression only.
            var sortedWords = words.OrderByDescending(word => word, new CaseInsensitiveComparer());

            ObjectDumper.Write(sortedWords);
        }

        [Category("Ordering Operators")]
        [Description("This example uses a compound orderby to sort a list of digits, first by length of their name, " +
                     "and then alphabetically by the name itself.")]
        static void LinqThenBySimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var sortedDigits =
                from digit in digits
                orderby digit.Length, digit
                select digit;

            // Fluent expression equivalent.
            // var sortedDigits = digits.OrderBy(digit => digit.Length).ThenBy(digit => digit);

            Console.WriteLine("Sorted digits:");
            foreach (var d in sortedDigits)
            {
                Console.WriteLine(d);
            }
        }

        [Category("Ordering Operators")]
        [Description("The first query in this example uses method syntax to call OrderBy and ThenBy with a custom comparer to " +
                     "sort first by word length and then by a case-insensitive sort of the words in an array. " +
                     "The second two queries show another way to perform the same task.")]
        static void LinqThenByComparer()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            // Fluent expression only.
            var sortedWords = words.OrderBy(word => word.Length).ThenBy(word => word, new CaseInsensitiveComparer());

            ObjectDumper.Write(sortedWords);
        }

        [Category("Ordering Operators")]
        [Description("This example uses a compound orderby to sort a list of products, " +
                     "first by category, and then by unit price, from highest to lowest.")]
        static void LinqThenByDescendingSimple(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var sortedProducts =
                from prod in storage.Products
                orderby prod.Category, prod.UnitPrice descending
                select prod;

            // Fluent expression equivalent.
            // var sortedProducts = storage.Products.OrderBy(prod => prod.Category).ThenByDescending(prod => prod.UnitPrice);

            ObjectDumper.Write(sortedProducts);
        }

        [Category("Ordering Operators")]
        [Description("This example uses an OrderBy and a ThenBy clause with a custom comparer to " +
                     "sort first by word length and then by a case-insensitive descending sort " +
                     "of the words in an array.")]
        static void LinqThenByDescendingComparer()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            // Fluent expression only.
            var sortedWords = words.OrderBy(word => word.Length).ThenByDescending(word => word, new CaseInsensitiveComparer());

            ObjectDumper.Write(sortedWords);
        }

        [Category("Ordering Operators")]
        [Description("This example uses Reverse to create a list of all digits in the array whose " +
                     "second letter is 'i' that is reversed from the order in the original array.")]
        static void LinqReverse()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var reversedIDigits = (
                from digit in digits
                where digit[1] == 'i'
                select digit).Reverse();

            Console.WriteLine("A backwards list of the digits with a second character of 'i':");
            foreach (var d in reversedIDigits)
            {
                Console.WriteLine(d);
            }
        }
    }
}
