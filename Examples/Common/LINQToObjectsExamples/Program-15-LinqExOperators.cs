using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using NDepth.Common.LinqEx;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("LinqEx Operators")]
        [Description("This example shows the usage of custom sequence generator.")]
        static void LinqGenerate()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var random = new Random();

            var generated =
                from v in EnumerableEx.Generate(0, i => random.Next()).Take(5)
                select v;

            Console.WriteLine("Generated values:");
            foreach (var v in generated)
            {
                Console.WriteLine(v);
            }
        }

        [Category("LinqEx Operators")]
        [Description("This example shows how to convert IEnumerator<T> to IEnumerable<T> in a simple way.")]
        static void LinqToEnumerable()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var items = new [] { 1, 2, 3, 4, 5 };

            // Get enumerable interface.
            var enumerable = items as IEnumerable<int>;

            // Get enumerator interface and lost enumerable one.
            var enumerator = enumerable.GetEnumerator();

            Console.WriteLine("Values from enumerator:");
            foreach (var v in enumerator.ToEnumerable())
            {
                Console.WriteLine(v);
            }
        }

        [Category("LinqEx Operators")]
        [Description("This example shows how to wrap single value to IEnumerable<T>.")]
        static void LinqSingleWrapper()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var single = EnumerableEx.Single(100);

            Console.WriteLine("Single value:");
            foreach (var v in single)
            {
                Console.WriteLine(v);
            }
        }

        [Category("LinqEx Operators")]
        [Description("This example shows how to perform some side effect action for each sequence element in a lazy way.")]
        static void LinqDo()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var items = new[] { 1, 2, 3, 4, 5 };

            Console.WriteLine("Items: ");
            foreach (var i in items)
            {
                Console.Write("=" + i + " ");
            }
            Console.WriteLine();
        }

        [Category("LinqEx Operators")]
        [Description("This example shows how to perform some side effect action for each sequence element in an immediate way.")]
        static void LinqForEach()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var items = new[] { 1, 2, 3, 4, 5 };

            Console.WriteLine("Items: ");
            items.ForEach(Console.Write);
            Console.WriteLine();
        }

        [Category("LinqEx Operators")]
        [Description("This example shows how to serialize enumerable sequence to pretty string.")]
        static void LinqToStringPretty()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var items = new[] { 1, 2, 3, 4, 5 };

            Console.WriteLine(items.ToStringPretty());
            Console.WriteLine(items.ToStringPretty(", "));
            Console.WriteLine(items.ToStringPretty("Items: ", ", ", "."));
        }

        [Category("LinqEx Operators")]
        [Description("This example shows how to combine two sequences.")]
        static void LinqCombine()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var items1 = new[] { 1, 2, 3, 4, 5 };
            var items2 = new[] { 10, 20, 30, 40, 50 };

            var combination = items1.Combine(items2, (i1, i2) => i1 + i2);
            
            Console.WriteLine("Combination: ");
            foreach (var i in combination)
            {
                Console.WriteLine(i);
            }
        }

        [Category("LinqEx Operators")]
        [Description("This example shows how to random shuffle a sequence.")]
        static void LinqShuffle()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var items = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var shuffle = items.Shuffle();

            Console.WriteLine("Shuffle: ");
            foreach (var i in shuffle)
            {
                Console.WriteLine(i);
            }
        }
    }
}
