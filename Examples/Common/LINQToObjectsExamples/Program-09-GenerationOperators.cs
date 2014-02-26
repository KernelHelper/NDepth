using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Generation Operators")]
        [Description("This example uses Empty to generate a sequence with no elements.")]
        static void LinqEmpty()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var empty = Enumerable.Empty<int>();

            Console.WriteLine("Empty sequence: ");
            foreach (var n in empty)
            {
                Console.WriteLine(n);
            }
        }

        [Category("Generation Operators")]
        [Description("This example uses Range to generate a sequence of numbers from 100 to 149 " +
                     "that is used to find which numbers in that range are odd and even.")]
        static void LinqRange()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var numbers =
                from n in Enumerable.Range(100, 50)
                select new { Number = n, OddEven = n % 2 == 1 ? "odd" : "even" };

            foreach (var n in numbers)
            {
                Console.WriteLine("The number {0} is {1}.", n.Number, n.OddEven);
            }
        }

        [Category("Generation Operators")]
        [Description("This example uses Repeat to generate a sequence that contains the number 7 ten times.")]
        static void LinqRepeat()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var numbers = Enumerable.Repeat(7, 10);

            foreach (var n in numbers)
            {
                Console.WriteLine(n);
            }
        }
    }
}
