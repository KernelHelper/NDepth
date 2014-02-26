using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Conversion Operators")]
        [Description("This example uses ToArray to immediately evaluate a sequence into an array.")]
        static void LinqToArray()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

            var sortedDoubles =
                from d in doubles
                orderby d descending
                select d;

            var doublesArray = sortedDoubles.ToArray();

            Console.WriteLine("Every other double from highest to lowest:");
            for (var d = 0; d < doublesArray.Length; d += 2)
            {
                Console.WriteLine(doublesArray[d]);
            }
        }

        [Category("Conversion Operators")]
        [Description("This example uses ToList to immediately evaluate a sequence into a List<T>.")]
        static void LinqToList()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            string[] words = { "cherry", "apple", "blueberry" };

            var sortedWords =
                from w in words
                orderby w
                select w;

            var wordList = sortedWords.ToList();

            Console.WriteLine("The sorted word list:");
            foreach (var w in wordList)
            {
                Console.WriteLine(w);
            }
        }

        [Category("Conversion Operators")]
        [Description("This example uses ToDictionary to immediately evaluate a sequence and a " +
                     "related key expression into a dictionary.")]
        static void LinqToDictionary1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var scoreRecords = new [] 
            { 
                new {Name = "Alice", Score = 50},
                new {Name = "Bob"  , Score = 40},
                new {Name = "Cathy", Score = 45}
            };

            var scoreRecordsDict = scoreRecords.ToDictionary(sr => sr.Name);

            Console.WriteLine("Bob's score: {0}", scoreRecordsDict["Bob"]);
        }

        [Category("Conversion Operators")]
        [Description("This example uses ToDictionary to immediately evaluate a sequence and a " +
                     "related key expression into a dictionary.")]
        static void LinqToDictionary2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var scoreRecords = new[] 
            { 
                new {Name = "Alice", Score = 50},
                new {Name = "Bob"  , Score = 40},
                new {Name = "Cathy", Score = 45}
            };

            var scoreRecordsDict = scoreRecords.ToDictionary(sr => sr.Name, sr => sr.Score);

            Console.WriteLine("Bob's score: {0}", scoreRecordsDict["Bob"]);
        }

        [Category("Conversion Operators")]
        [Description("This example uses ToLookup to immediately evaluate a sequence and a " +
                     "related key expression into a lookup.")]
        static void LinqToLookup1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var scoreRecords = new[] 
            { 
                new {Name = "Alice", Score = 50},
                new {Name = "Alice", Score = 51},
                new {Name = "Bob"  , Score = 40},
                new {Name = "Bob"  , Score = 41},
                new {Name = "Bob"  , Score = 42},
                new {Name = "Cathy", Score = 45}
            };

            var scoreRecordsLookup = scoreRecords.ToLookup(sr => sr.Name);

            Console.WriteLine("Bob's score: ");
            foreach (var s in scoreRecordsLookup["Bob"])
            {
                Console.WriteLine(s);
            }
        }

        [Category("Conversion Operators")]
        [Description("This example uses ToLookup to immediately evaluate a sequence and a " +
                     "related key expression into a lookup.")]
        static void LinqToLookup2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var scoreRecords = new[] 
            { 
                new {Name = "Alice", Score = 50},
                new {Name = "Alice", Score = 51},
                new {Name = "Bob"  , Score = 40},
                new {Name = "Bob"  , Score = 41},
                new {Name = "Bob"  , Score = 42},
                new {Name = "Cathy", Score = 45}
            };

            var scoreRecordsLookup = scoreRecords.ToLookup(sr => sr.Name, sr => sr.Score);

            Console.WriteLine("Bob's score: ");
            foreach (var s in scoreRecordsLookup["Bob"])
            {
                Console.WriteLine(s);
            }
        }

        [Category("Conversion Operators")]
        [Description("This example uses OfType to return only the elements of the array that are of type double.")]
        static void LinqOfType()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            object[] numbers = { null, 1.0, "two", 3, "four", 5, "six", 7.0 };

            var doubles = numbers.OfType<double>();

            Console.WriteLine("Numbers stored as doubles:");
            foreach (var d in doubles)
            {
                Console.WriteLine(d);
            }
        }

        [Category("Conversion Operators")]
        [Description("This example uses Cast to ensure and return all the elements of the array that are of type double.")]
        static void LinqCast()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            object[] numbers1 = { 0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0 };
            object[] numbers2 = { null, 1.0, "two", 3, "four", 5, "six", 7.0 };

            var doubles1 = numbers1.Cast<double>();
            var doubles2 = numbers2.Cast<double>();

            Console.WriteLine("Numbers stored as doubles:");
            foreach (var d in doubles1)
            {
                Console.WriteLine(d);
            }
            // This code can generate NullReferenceException or InvalidCastException.
            try
            {
                foreach (var d in doubles2)
                {
                    Console.WriteLine(d);
                }
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine(ex);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex);
            }
        }

        class Clump<T> : List<T>
        {
            // Custom implementation of Where(). 
            public IEnumerable<T> Where(Func<T, bool> predicate)
            {
                Console.WriteLine("In Clump's implementation of Where().");
                return Enumerable.Where(this, predicate);
            }
        }

        [Category("Conversion Operators")]
        [Description("The following code example demonstrates how to use AsEnumerable to hide a type's custom Where method " +
                     "when the standard query operator implementation is desired.")]
        static void LinqAsEnumerable()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Create a new Clump<T> object.
            var fruitClump = new Clump<string> { "apple", "passionfruit", "banana", "mango", "orange", "blueberry", "grape", "strawberry" };

            // First call to Where(): 
            // Call Clump's Where() method with a predicate.
            var query1 = fruitClump.Where(fruit => fruit.Contains("o"));

            Console.WriteLine("query1 has been created.");
            foreach (var f in query1)
            {
                Console.WriteLine(f);
            }

            // Second call to Where(): 
            // First call AsEnumerable() to hide Clump's Where() method and thereby 
            // force System.Linq.Enumerable's Where() method to be called.
            var query2 = fruitClump.AsEnumerable().Where(fruit => fruit.Contains("o"));

            // Display the output.
            Console.WriteLine("query2 has been created.");
            foreach (var f in query2)
            {
                Console.WriteLine(f);
            }
        }
    }
}
