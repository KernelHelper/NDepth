using System;
using System.Collections.Generic;

namespace NDepth.Common.LinqEx
{
    /// <summary>
    /// More Linq extension methods for IEnumerator interface.
    /// </summary>
    public static class EnumeratorEx
    {
        /// <summary>Convert sequence represented with IEnumerator to IEnumerable</summary>
        /// <param name="enumerator">Enumerator</param>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="enumerator"/> is <c>null</c>.
        /// </exception>
        /// <returns>Corresponding enumerable sequence</returns>
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator)
        {
            if (enumerator == null)
                throw new ArgumentNullException("enumerator");

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}
