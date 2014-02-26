using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    static class CustomSequenceOperators
    {
        public static IEnumerable<TResult> CustomCombine<T1, T2, TResult>(this IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, TResult> func)
        {
            using (var e1 = first.GetEnumerator())
            using (var e2 = second.GetEnumerator()) 
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    yield return func(e1.Current, e2.Current);
                }
            }
        }

        public static bool CustomSequencesEquivalent<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer = null)
        {
            return first.Intersect(second, comparer).Count() == first.Count();
        }
    }

    partial class Program
    {
        [Category("Custom Operators")]
        [Description("This example uses a user-created sequence operator, Combine, to calculate the dot product of two vectors.")]
        static void LinqCustomCombine()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            int[] vectorA = { 0, 2, 4, 5, 6 };
            int[] vectorB = { 1, 3, 5, 7, 8 };

            var dotProduct = vectorA.CustomCombine(vectorB, (a, b) => a * b).Sum();

            Console.WriteLine("Dot product: {0}", dotProduct); 
        }

        [Category("Custom Operators")]
        [Description("This example uses a user-created sequence operator, SequencesEquivalent, to check if two sequences are equivalent.")]
        static void LinqCustomSequenceEquivalent()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var wordsA = new [] { "cherry", "apple", "blueberry" };
            var wordsB = new [] { "apple", "blueberry", "cherry" };

            var equivalent = wordsA.CustomSequencesEquivalent(wordsB);

            Console.WriteLine("The sequences equivalent: {0}", equivalent);
        }
    }
}
