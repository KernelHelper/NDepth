using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Set Operators")]
        [Description("This example uses Distinct to remove duplicate elements in a sequence of factors of 300.")]
        static void LinqDistinct1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] factorsOf300 = { 2, 2, 3, 5, 5 };

            var uniqueFactors = factorsOf300.Distinct();

            Console.WriteLine("Prime factors of 300:");
            foreach (var f in uniqueFactors)
            {
                Console.WriteLine(f);
            }
        }

        [Category("Set Operators")]
        [Description("This example uses Distinct to find the unique Category names.")]
        static void LinqDistinct2(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var categoryNames = (
                from p in storage.Products
                select p.Category)
                .Distinct();

            Console.WriteLine("Category names:");
            foreach (var n in categoryNames)
            {
                Console.WriteLine(n);
            } 
        }

        [Category("Set Operators")]
        [Description("This example uses Union to create one sequence that contains the unique values " +
                     "from both arrays.")]
        static void LinqUnion1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            var uniqueNumbers = numbersA.Union(numbersB);

            Console.WriteLine("Unique numbers from both arrays:");
            foreach (var n in uniqueNumbers)
            {
                Console.WriteLine(n);
            }
        }

        [Category("Set Operators")]
        [Description("This example uses the Union method to create one sequence that contains the unique first letter " +
                     "from both product and customer names. Union is only available through method syntax.")]
        static void LinqUnion2(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var productFirstChars =
                from p in storage.Products
                select p.ProductName[0];
            var customerFirstChars =
                from c in storage.Customers
                select c.CompanyName[0];

            var uniqueFirstChars = productFirstChars.Union(customerFirstChars);

            Console.WriteLine("Unique first letters from Product names and Customer names:");
            foreach (var ch in uniqueFirstChars)
            {
                Console.WriteLine(ch);
            }
        }

        [Category("Set Operators")]
        [Description("This example uses Intersect to create one sequence that contains the common values " +
                     "shared by both arrays.")]
        static void LinqIntersect1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            var commonNumbers = numbersA.Intersect(numbersB);

            Console.WriteLine("Common numbers shared by both arrays:");
            foreach (var n in commonNumbers)
            {
                Console.WriteLine(n);
            }
        }

        [Category("Set Operators")]
        [Description("This example uses Intersect to create one sequence that contains the common first letter " +
                     "from both product and customer names.")]
        static void LinqIntersect2(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var productFirstChars =
                from p in storage.Products
                select p.ProductName[0];
            var customerFirstChars =
                from c in storage.Customers
                select c.CompanyName[0];

            var commonFirstChars = productFirstChars.Intersect(customerFirstChars);

            Console.WriteLine("Common first letters from Product names and Customer names:");
            foreach (var ch in commonFirstChars)
            {
                Console.WriteLine(ch);
            } 
        }

        [Category("Set Operators")]
        [Description("This example uses Except to create a sequence that contains the values from numbersA" +
                     "that are not also in numbersB.")]
        static void LinqExcept1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            var aOnlyNumbers = numbersA.Except(numbersB);

            Console.WriteLine("Numbers in first array but not second array:");
            foreach (var n in aOnlyNumbers)
            {
                Console.WriteLine(n);
            }             
        }

        [Category("Set Operators")]
        [Description("This example uses Except to create one sequence that contains the first letters " +
                     "of product names that are not also first letters of customer names.")]
        static void LinqExcept2(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var productFirstChars =
                from p in storage.Products
                select p.ProductName[0];
            var customerFirstChars =
                from c in storage.Customers
                select c.CompanyName[0];

            var productOnlyFirstChars = productFirstChars.Except(customerFirstChars);

            Console.WriteLine("First letters from Product names, but not from Customer names:");
            foreach (var ch in productOnlyFirstChars)
            {
                Console.WriteLine(ch);
            }
        }
    }
}
