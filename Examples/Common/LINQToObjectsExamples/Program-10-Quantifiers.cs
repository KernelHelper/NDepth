using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Quantifiers")]
        [Description("This example uses Contains to determine if the array contains word 'cool'.")]
        static void LinqContains()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "believe", "relief", "cool", "receipt", "field" };

            var contains = words.Contains("cool");

            Console.WriteLine("Is there the word 'cool': {0}", contains);
        }

        [Category("Quantifiers")]
        [Description("This example uses Any to determine if any of the words in the array " +
                     "contain the substring 'ei'.")]
        static void LinqAnySimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "believe", "relief", "receipt", "field" };

            var iAfterE = words.Any(w => w.Contains("ei"));

            Console.WriteLine("There is a word that contains in the list that contains 'ei': {0}", iAfterE);
        }

        [Category("Quantifiers")]
        [Description("This example uses Any to return a grouped a list of products only for categories " +
                     "that have at least one product that is out of stock.")]
        static void LinqAnyGrouped(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var productGroups =
                from p in storage.Products
                group p by p.Category into g
                where g.Any(p => p.UnitsInStock == 0)
                select new { Category = g.Key, Products = g };

            // Fluent expression equivalent.
            // var productGroups = storage.Products.GroupBy(p => p.Category, (k, g) => new { Category = k, Products = g }).Where(g => g.Products.Any(p => p.UnitsInStock == 0));

            ObjectDumper.Write(productGroups, 1);
        }

        [Category("Quantifiers")]
        [Description("This example uses All to determine whether an array contains only odd numbers.")]
        static void LinqAllSimple()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbers = { 1, 11, 3, 19, 41, 65, 19 };

            var onlyOdd = numbers.All(n => n % 2 == 1);

            Console.WriteLine("The list contains only odd numbers: {0}", onlyOdd); 
        }

        [Category("Quantifiers")]
        [Description("This example uses All to return a grouped a list of products only for categories " +
                     "that have all of their products in stock.")]
        static void LinqAllGrouped(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var productGroups =
                from p in storage.Products
                group p by p.Category into g
                where g.All(p => p.UnitsInStock > 0)
                select new { Category = g.Key, Products = g };

            // Fluent expression equivalent.
            // var productGroups = storage.Products.GroupBy(p => p.Category, (k, g) => new { Category = k, Products = g }).Where(g => g.Products.All(p => p.UnitsInStock > 0));

            ObjectDumper.Write(productGroups, 1); 
        }
    }
}
