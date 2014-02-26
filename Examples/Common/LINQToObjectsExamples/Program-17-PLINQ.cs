using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        [Category("PLINQ")]
        [Description("The following example shows AsParallel() extension.")]
        static void PLinqAsParallel(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel = 
                from p in storage.Products.AsParallel()
                select new { p.ProductId, p.ProductName, Thread.CurrentThread.ManagedThreadId };

            foreach (var p in parallel)
            {
                Console.WriteLine("Id = {0}, Product = {1}, ThreadId = {2}", p.ProductId, p.ProductName, p.ManagedThreadId);
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows AsSequential() extension.")]
        static void PLinqAsSequential(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel =
                (from p in storage.Products.AsParallel()
                 select new { p.ProductId, p.ProductName, Thread.CurrentThread.ManagedThreadId }).AsSequential().Take(10);

            foreach (var p in parallel)
            {
                Console.WriteLine("Id = {0}, Product = {1}, ThreadId = {2}", p.ProductId, p.ProductName, p.ManagedThreadId);
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows AsEnumerable() extension.")]
        static void PLinqAsEnumerable(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel =
                (from p in storage.Products.AsParallel()
                 select new { p.ProductId, p.ProductName, Thread.CurrentThread.ManagedThreadId }).AsEnumerable().Take(10);

            foreach (var p in parallel)
            {
                Console.WriteLine("Id = {0}, Product = {1}, ThreadId = {2}", p.ProductId, p.ProductName, p.ManagedThreadId);
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows AsOrdered() extension.")]
        static void PLinqAsOrdered(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel =
                from p in storage.Products.AsParallel().AsOrdered()
                select new { p.ProductId, p.ProductName, Thread.CurrentThread.ManagedThreadId };

            foreach (var p in parallel)
            {
                Console.WriteLine("Id = {0}, Product = {1}, ThreadId = {2}", p.ProductId, p.ProductName, p.ManagedThreadId);
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows AsUnordered() extension.")]
        static void PLinqAsUnordered(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel =
                from p in storage.Products.AsParallel().AsOrdered().Take(10).AsUnordered()
                select new { p.ProductId, p.ProductName, Thread.CurrentThread.ManagedThreadId };

            foreach (var p in parallel)
            {
                Console.WriteLine("Id = {0}, Product = {1}, ThreadId = {2}", p.ProductId, p.ProductName, p.ManagedThreadId);
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows WithDegreeOfParallelism() extension.")]
        static void PLinqWithDegreeOfParallelism(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel =
                from p in storage.Products.AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount)
                select new { p.ProductId, p.ProductName, Thread.CurrentThread.ManagedThreadId };

            foreach (var p in parallel)
            {
                Console.WriteLine("Id = {0}, Product = {1}, ThreadId = {2}", p.ProductId, p.ProductName, p.ManagedThreadId);
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows WithExecutionMode() extension.")]
        static void PLinqWithExecutionMode(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel =
                from p in storage.Products.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                select new { p.ProductId, p.ProductName, Thread.CurrentThread.ManagedThreadId };

            foreach (var p in parallel)
            {
                Console.WriteLine("Id = {0}, Product = {1}, ThreadId = {2}", p.ProductId, p.ProductName, p.ManagedThreadId);
            }
        }
        
        [Category("PLINQ")]
        [Description("The following example shows WithMergeOptions(NotBuffered) extension.")]
        static void PLinqWithMergeOptionsNotBuffered()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel = ParallelEnumerable.Range(1, 10).WithMergeOptions(ParallelMergeOptions.NotBuffered).Select(p => 
            {
                Thread.Sleep(1000);
                return new { Value = p, Thread.CurrentThread.ManagedThreadId };
            });

            Stopwatch sw = Stopwatch.StartNew();

            foreach (var p in parallel)
            {
                Console.WriteLine("Number = {0}, ThreadId = {1}, Elapsed = {2}", p.Value, p.ManagedThreadId, sw.ElapsedMilliseconds);
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows WithMergeOptions(FullBuffered) extension.")]
        static void PLinqWithMergeOptionsFullBuffered()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel = ParallelEnumerable.Range(1, 10).WithMergeOptions(ParallelMergeOptions.FullyBuffered).Select(p =>
            {
                Thread.Sleep(1000);
                return new { Value = p, Thread.CurrentThread.ManagedThreadId };
            });

            Stopwatch sw = Stopwatch.StartNew();

            foreach (var p in parallel)
            {
                Console.WriteLine("Number = {0}, ThreadId = {1}, Elapsed = {2}", p.Value, p.ManagedThreadId, sw.ElapsedMilliseconds);
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows WithCancellation() extension in query.")]
        static void PLinqWithCancellation1()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var cs = new CancellationTokenSource();

            var parallel =
                from p in ParallelEnumerable.Range(1, 10000000).WithCancellation(cs.Token)
                where ((p % 3) == 0)
                select new { Value = p, Thread.CurrentThread.ManagedThreadId };

            // Start a new asynchronous task that will cancel the  operation from another thread.
            Task.Factory.StartNew(() =>
            {
                var rand = new Random();

                // Wait between 150 and 500 ms, then cancel.
                Thread.Sleep(rand.Next(150, 350));

                cs.Cancel();
            }, cs.Token);

            try
            {
                foreach (var p in parallel)
                {
                    Console.WriteLine("Number = {0}, ThreadId = {1}", p.Value, p.ManagedThreadId);
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (AggregateException aggrex)
            {
                aggrex.Handle(ex =>
                {
                    Console.WriteLine(ex.Message);
                    return true;
                });
            }
        }

        private static double DoHardWork(int n, CancellationToken ct)
        {
            for (var i = 0; i < 5; i++)
            {
                // Work hard for approx 1 millisecond.
                Thread.SpinWait(50000);

                // Check for cancellation request.
                ct.ThrowIfCancellationRequested();
            }        
    
            // Anything will do for our purposes.
            return Math.Sqrt(n);
        }

        [Category("PLINQ")]
        [Description("The following example shows WithCancellation() extension in user code.")]
        static void PLinqWithCancellation2()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var cs = new CancellationTokenSource();

            var parallel =
                from p in ParallelEnumerable.Range(1, 10000000).WithCancellation(cs.Token)
                where ((p % 3) == 0)
                select new { Value = DoHardWork(p, cs.Token), Thread.CurrentThread.ManagedThreadId };

            // Start a new asynchronous task that will cancel the  operation from another thread.
            Task.Factory.StartNew(() =>
            {
                var rand = new Random();

                // Wait between 150 and 500 ms, then cancel.
                Thread.Sleep(rand.Next(150, 350));

                cs.Cancel();
            }, cs.Token);

            try
            {
                foreach (var p in parallel)
                {
                    Console.WriteLine("Number = {0}, ThreadId = {1}", p.Value, p.ManagedThreadId);
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (AggregateException aggrex)
            {
                aggrex.Handle(ex =>
                {
                    Console.WriteLine(ex.Message);
                    return true;
                });
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows custom aggregation of the parallel LINQ results.")]
        static void PLinqAggregate()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var result =
                (from p in ParallelEnumerable.Range(1, 1000)
                 where ((p % 3) == 0)
                 select new { Value = p, Thread.CurrentThread.ManagedThreadId }).Aggregate(
                    // Initial seed for each thread.
                    () => 0,
                    // Do this aggregation step for the current thread.
                    (subtotal, item) => subtotal + item.Value,
                    // Do this aggregation step after all threads are done.
                    (total, thread) => total + thread,
                    // Calculate the final result.
                    final => 2 * final);

            Console.WriteLine("Aggregate = {0}", result);
        }

        [Category("PLINQ")]
        [Description("The following example shows ParallelEnumerable.Range() method.")]
        static void PLinqRange()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel =
                from p in ParallelEnumerable.Range(1, 1000)
                where ((p % 3) == 0)
                select new { Value = p, Thread.CurrentThread.ManagedThreadId };

            foreach (var p in parallel)
            {
                Console.WriteLine("Number = {0}, ThreadId = {1}", p.Value, p.ManagedThreadId);
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows ParallelEnumerable.Repeat() method.")]
        static void PLinqRepeat()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel =
                from p in ParallelEnumerable.Repeat(1, 100)
                select new { Value = p, Thread.CurrentThread.ManagedThreadId };

            foreach (var p in parallel)
            {
                Console.WriteLine("Number = {0}, ThreadId = {1}", p.Value, p.ManagedThreadId);
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows ParallelEnumerable.Empty() method.")]
        static void PLinqEmpty()
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel =
                from p in ParallelEnumerable.Empty<int>()
                select new { Value = p, Thread.CurrentThread.ManagedThreadId };

            foreach (var p in parallel)
            {
                Console.WriteLine("Number = {0}, ThreadId = {1}", p.Value, p.ManagedThreadId);
            }
        }

        [Category("PLINQ")]
        [Description("The following example shows ForAll() extension.")]
        static void PLinqForAll(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            storage.Products.AsParallel().ForAll(p => Console.WriteLine("Id = {0}, Product = {1}, ThreadId = {2}", p.ProductId, p.ProductName, Thread.CurrentThread.ManagedThreadId));
        }

        [Category("PLINQ")]
        [Description("The following example shows how to handle AggregateException.")]
        static void PLinqExceptions(CustomersStorage storage)
        {
            Console.WriteLine("=== " + MethodInfo.GetCurrentMethod().Name + " ===");

            var parallel = storage.Products.AsParallel().Select(p =>
            {
                if (p.ProductName.StartsWith("P"))
                    throw new Exception("Problem with product: " + p.ProductName);
                
                return new { p.ProductId, p.ProductName, Thread.CurrentThread.ManagedThreadId };
            });

            try
            {
                foreach (var p in parallel)
                {
                    Console.WriteLine("Id = {0}, Product = {1}, ThreadId = {2}", p.ProductId, p.ProductName, p.ManagedThreadId);
                }                            
            }
            catch (AggregateException aggrex)
            {
                aggrex.Handle(ex =>
                {
                    Console.WriteLine(ex.Message); 
                    return true;
                });
            }
        }
    }
}
