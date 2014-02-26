using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("Query Execution")]
        [Description("The following example shows how query execution is deferred until the query is " +
                     "enumerated at a foreach statement.")]
        static void LinqDeferredExecution()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Sequence operators form first-class queries that 
            // are not executed until you enumerate over them. 

            var numbers = new [] { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var i = 0;
            var q = from n in numbers
                    select ++i;

            // Note, the local variable 'i' is not incremented 
            // until each element is evaluated (as a side-effect): 
            foreach (var v in q)
            {
                Console.WriteLine("v = {0}, i = {1}", v, i);
            }
        }

        [Category("Query Execution")]
        [Description("The following example shows how queries can be executed immediately, and their results " +
                     "stored in memory, with methods such as ToList.")]
        static void LinqImmediateExecution()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Methods like ToList() cause the query to be 
            // executed immediately, caching the results. 

            var numbers = new [] { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var i = 0;
            var q = (from n in numbers
                     select ++i).ToList();

            // The local variable i has already been fully 
            // incremented before we iterate the results: 
            foreach (var v in q)
            {
                Console.WriteLine("v = {0}, i = {1}", v, i);
            } 
        }

        [Category("Query Execution")]
        [Description("The following example shows how, because of deferred execution, queries can be used " +
                     "again after data changes and will then operate on the new data.")]
        static void LinqQueryReuse()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            // Deferred execution lets us define a query once 
            // and then reuse it later after data changes. 

            var numbers = new [] { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var lowNumbers = from n in numbers
                             where n <= 3
                             select n;

            Console.WriteLine("First run numbers <= 3:");
            foreach (var n in lowNumbers)
            {
                Console.WriteLine(n);
            }

            // Modify sequence.
            for (var i = 0; i < 10; i++)
            {
                numbers[i] = -numbers[i];
            }

            // During this second run, the same query object, 
            // lowNumbers, will be iterating over the new state 
            // of numbers[], producing different results: 
            Console.WriteLine("Second run numbers <= 3:");
            foreach (var n in lowNumbers)
            {
                Console.WriteLine(n);
            } 
        }
    }
}
