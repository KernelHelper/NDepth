using System;
using System.Collections.Generic;
using System.Text;

namespace NDepth.Common.LinqEx
{
    /// <summary>
    /// More Linq extension methods for strings.
    /// </summary>
    public static class StringEx
    {
        /// <summary>Build a pretty string from the enumerable sequence using comma as a delimiter</summary>
        /// <param name="source">Source sequence</param>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="source"/> is <c>null</c>.
        /// </exception>
        /// <returns>Result string</returns>
        public static string ToStringPretty<T>(this IEnumerable<T> source)
        {
            return ToStringPretty(source, ",");
        }

        /// <summary>Build a pretty string from the enumerable sequence using given delimiter</summary>
        /// <param name="source">Source sequence</param>
        /// <param name="delimiter">String delimiter</param>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="source"/> is <c>null</c>.
        /// </exception>
        /// <returns>Result string</returns>
        public static string ToStringPretty<T>(this IEnumerable<T> source, string delimiter)
        {
            return ToStringPretty(source, "", delimiter, "");
        }

        /// <summary>Build a pretty string from the enumerable sequence using given prefix, suffix and delimiter</summary>
        /// <param name="source">Source sequence</param>
        /// <param name="before">Before string prefix</param>
        /// <param name="delimiter">String delimiter</param>
        /// <param name="after">After string suffix</param>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="source"/> is <c>null</c>.
        /// </exception>
        /// <returns>Result string</returns>
        public static string ToStringPretty<T>(this IEnumerable<T> source, string before, string delimiter, string after)
        {
            if (source == null) 
                throw new ArgumentNullException("source");

            var result = new StringBuilder();
            result.Append(before);

            var firstElement = true;
            foreach (var elem in source)
            {
                if (firstElement) 
                    firstElement = false;
                else 
                    result.Append(delimiter);
                result.Append(elem);
            }

            result.Append(after);
            return result.ToString();
        }
    }
}
