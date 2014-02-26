using System;
using System.Collections.Generic;
using System.Linq;

namespace NDepth.Common.LinqEx
{
    /// <summary>
    /// More Linq extension methods for IEnumerable interface.
    /// </summary>
    public static class EnumerableEx
    {
        #region Generating sequences

        /// <summary>Generate sequence using custom generator</summary>
        /// <param name="seed">Initial seed value for generator</param>
        /// <param name="generator">Generator function</param>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="generator"/> is <c>null</c>.
        /// </exception>
        /// <returns>Enumerable sequence with the generated values</returns>
        public static IEnumerable<T> Generate<T>(T seed, Func<T, T> generator)
        {
            if (generator == null) 
                throw new ArgumentNullException("generator");

            T current = seed;
            while (true)
            {
                current = generator(current);
                yield return current;
            }
        }

        /// <summary>Get enumerable sequence for the one wrapped vale</summary>
        /// <returns>Enumerable sequence with the one wrapped value</returns>
        public static IEnumerable<T> Single<T>(T value)
        {
            return Enumerable.Repeat(value, 1);
        }

        #endregion

        #region Side effects

        /// <summary>Perform some action on each element in sequence and return it (lazy execution)</summary>
        /// <param name="source">Source sequence</param>
        /// <param name="action">Action to perform</param>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="source"/> is <c>null</c>.
        ///   if <paramref name="action"/> is <c>null</c>.
        /// </exception>
        /// <returns>Enumerable sequence</returns>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) 
                throw new ArgumentNullException("source");
            if (action == null) 
                throw new ArgumentNullException("action");

            foreach (var elem in source)
            {
                action(elem);
                yield return elem;
            }
        }

        /// <summary>Perform some action on each element in sequence (immediate execution)</summary>
        /// <param name="source">Source sequence</param>
        /// <param name="action">Action to perform</param>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="source"/> is <c>null</c>.
        ///   if <paramref name="action"/> is <c>null</c>.
        /// </exception>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) 
                throw new ArgumentNullException("source");
            if (action == null) 
                throw new ArgumentNullException("action");

            foreach (var elem in source)
            {
                action(elem);
            }
        }

        #endregion

        #region Other stuff

        /// <summary>Combine two sequences using the given function</summary>
        /// <param name="in1">Source sequence 1</param>
        /// <param name="in2">Source sequence 2</param>
        /// <param name="func">Combining function</param>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="in1"/> is <c>null</c>.
        ///   if <paramref name="in2"/> is <c>null</c>.
        ///   if <paramref name="func"/> is <c>null</c>.         
        /// </exception>
        /// <returns>Enumerable sequence</returns>
        public static IEnumerable<TOut> Combine<TIn1, TIn2, TOut>(this IEnumerable<TIn1> in1, IEnumerable<TIn2> in2, Func<TIn1, TIn2, TOut> func)
        {
            if (in1 == null) 
                throw new ArgumentNullException("in1");
            if (in2 == null) 
                throw new ArgumentNullException("in2");
            if (func == null) 
                throw new ArgumentNullException("func");

            using (var e1 = in1.GetEnumerator())
            using (var e2 = in2.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    yield return func(e1.Current, e2.Current);
                }
            }
        }

        /// <summary>Shuffle elements in the given sequence</summary>
        /// <param name="source">Source sequence</param>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="source"/> is <c>null</c>.
        /// </exception>
        /// <returns>Enumerable sequence</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return Shuffle(source, new Random());
        }

        /// <summary>Shuffle elements in the given sequence using the given random generator</summary>
        /// <param name="source">Source sequence</param>
        /// <param name="random">Random generator</param>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="source"/> is <c>null</c>.
        ///   if <paramref name="random"/> is <c>null</c>.
        /// </exception>
        /// <returns>Enumerable sequence</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
        {
            if (source == null) 
                throw new ArgumentNullException("source");
            if (random == null) 
                throw new ArgumentNullException("random");

            var array = source.ToArray();

            for (var i = 0; i < array.Length; i++)
            {
                var r = random.Next(i + 1);
                var tmp = array[r];
                array[r] = array[i];
                array[i] = tmp;
            }

            return array;
        }
        
        #endregion
    }
}
