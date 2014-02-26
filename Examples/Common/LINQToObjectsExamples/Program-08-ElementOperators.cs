using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Element Operators")]
        [Description("This example uses First to return the first matching element " +
                     "as a Product, instead of as a sequence containing a Product.")]
        static void LinqFirstSimple(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var product12 = (
                from p in storage.Products
                where p.ProductId == 12
                select p).First();

            ObjectDumper.Write(product12);
        }

        [Category("Element Operators")]
        [Description("This example uses First to find the first element in the array that starts with 'o'.")]
        static void LinqFirstCondition()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var startsWithO = strings.First(s => s[0] == 'o');

            Console.WriteLine("A string starting with 'o': {0}", startsWithO); 
        }

        [Category("Element Operators")]
        [Description("This example uses FirstOrDefault to try to return the first element of the sequence, " +
                     "unless there are no elements, in which case the default value for that type " +
                     "is returned. FirstOrDefault is useful for creating outer joins.")]
        static void LinqFirstOrDefaultSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { };

            var firstNumOrDefault = numbers.FirstOrDefault();

            Console.WriteLine(firstNumOrDefault); 
        }

        [Category("Element Operators")]
        [Description("This example uses FirstOrDefault to return the first product whose ProductID is 789 " +
                     "as a single Product object, unless there is no match, in which case null is returned.")]
        static void LinqFirstOrDefaultCondition(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var product789 = storage.Products.FirstOrDefault(p => p.ProductId == 789);

            Console.WriteLine("Product 789 exists: {0}", product789 != null); 
        }

        [Category("Element Operators")]
        [Description("This example uses Last to return the last matching element " +
                     "as a Product, instead of as a sequence containing a Product.")]
        static void LinqLastSimple(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var product12 = (
                from p in storage.Products
                where p.ProductId == 12
                select p).Last();

            ObjectDumper.Write(product12);
        }

        [Category("Element Operators")]
        [Description("This example uses Last to find the last element in the array that starts with 'o'.")]
        static void LinqLastCondition()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var startsWithO = strings.Last(s => s[0] == 'o');

            Console.WriteLine("A string starting with 'o': {0}", startsWithO);
        }

        [Category("Element Operators")]
        [Description("This example uses LastOrDefault to try to return the last element of the sequence, " +
                     "unless there are no elements, in which case the default value for that type " +
                     "is returned. LastOrDefault is useful for creating outer joins.")]
        static void LinqLastOrDefaultSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { };

            var lastNumOrDefault = numbers.LastOrDefault();

            Console.WriteLine(lastNumOrDefault);
        }

        [Category("Element Operators")]
        [Description("This example uses LastOrDefault to return the last product whose ProductID is 789 " +
                     "as a single Product object, unless there is no match, in which case null is returned.")]
        static void LinqLastOrDefaultCondition(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var product789 = storage.Products.LastOrDefault(p => p.ProductId == 789);

            Console.WriteLine("Product 789 exists: {0}", product789 != null);
        }

        [Category("Element Operators")]
        [Description("This example uses ElementAt to retrieve the second number greater than 5 from an array.")]
        static void LinqElementAt()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var fourthLowNum = (
                from n in numbers
                where n > 5
                select n).ElementAt(1); // Second number is index 1 because sequences use 0-based indexing.

            Console.WriteLine("Second number > 5: {0}", fourthLowNum);
        }

        [Category("Element Operators")]
        [Description("This example uses ElementAt to retrieve some out of bounds item from an array.")]
        static void LinqElementAtOrDefault()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var outOfBoundsItem = numbers.ElementAtOrDefault(100);

            Console.WriteLine("Out of bounds item: {0}", outOfBoundsItem);
        }

        [Category("Element Operators")]
        [Description("This example uses Single to retrieve only one single element from a sequence. " +
                     "If the sequence is empty or has more than one elements InvalidOperationException will be thrown.")]
        static void LinqSingle()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers1 = { 5 };
            int[] numbers2 = { 5, 6, 7, 8 };

            var single = numbers1.Single();
            Console.WriteLine("Single number is {0}", single);

            // The following expression will throw InvalidOperationException.
            try
            {
                var notsingle = numbers2.Single();
                Console.WriteLine("Will never be printed! Single number is {0}", notsingle);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex);                
            }
        }

        [Category("Element Operators")]
        [Description("This example uses SingleOrDefault to retrieve only one single element from a sequence. " +
                     "If the sequence is empty default value will be returned. " +
                     "If the sequence has more than one elements InvalidOperationException will be thrown.")]
        static void LinqSingleOrDefault()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers1 = { };
            int[] numbers2 = { 5 };
            int[] numbers3 = { 5, 6, 7, 8 };

            var single1 = numbers1.SingleOrDefault();
            Console.WriteLine("Default number is {0}", single1);

            var single2 = numbers2.SingleOrDefault();
            Console.WriteLine("Single number is {0}", single2);

            // The following expression will throw InvalidOperationException.
            try
            {
                var notsingle = numbers3.SingleOrDefault();
                Console.WriteLine("Will never be printed! Single number is {0}", notsingle);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
