using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NDepth.Module;

namespace NDepth.Examples.Module.ModuleLoggingPerformance
{
    public class ConsoleModule : ModuleBase
    {
        private static int _countPerThread = 100000;
        private static int _producersCount = 1;
        private static int _totalCount;

        #region Module interface

        protected override void OnStart(string[] args)
        {
            Logger.InfoFormat(Resources.Strings.ModuleStarted, ModuleName, MachineName);

            if ((args.Length >= 1) && (!int.TryParse(args[0], out _countPerThread)) || (_countPerThread < 1))
            {
                Console.WriteLine(Resources.Strings.UsageMessage);
                throw new ArgumentException(Resources.Strings.InvalidArgumentCountPerThread);
            }
            if ((args.Length >= 2) && (!int.TryParse(args[1], out _producersCount)) || (_producersCount < 1))
            {
                Console.WriteLine(Resources.Strings.UsageMessage);
                throw new ArgumentException(Resources.Strings.InvalidArgumentProducersCount);
            }

            _countPerThread = _countPerThread / _producersCount;
            _totalCount = _countPerThread * _producersCount;
        }

        protected override void OnRun()
        {
            try
            {
                // Cache logging message.
                string logMessage = Resources.Strings.LogMessage;

                var results = new double[_totalCount];
                for (var i = 0; i < _totalCount; i++)
                    results[i] = 0;

                // Start the stopwatch timer.
                var stopWatch = Stopwatch.StartNew();

                // Create and start producer tasks.
                var producers = new Task[_producersCount];
                for (var producerIndex = 0; producerIndex < _producersCount; producerIndex++)
                {
                    producers[producerIndex] = Task.Factory.StartNew(state =>
                    {
                        var index = (int)state;
                        for (var i = 0; i < _countPerThread; i++)
                        {
                            var id = index * _countPerThread + i;
                            results[id] = stopWatch.ElapsedTicks;
                            Logger.InfoFormat(logMessage, id + 1, _totalCount);
                            results[id] = stopWatch.ElapsedTicks - results[id];
                        }
                    }, producerIndex, TaskCreationOptions.LongRunning);
                }

                // Inner exceptions example.
                try
                {
                    ThrowExceptionCallStack();
                }
                catch (Exception ex)
                {
                    Logger.Fatal("Fatal exception", ex);
                }

                // Wait for producing complete.
                Task.WaitAll(producers);
                stopWatch.Stop();

                // Adjust result table.
                var frequency = Stopwatch.Frequency / (1000.0 * 1000.0);
                for (var i = 0; i < _totalCount; i++)
                    results[i] /= frequency;
                Array.Sort(results);

                // Show report message.
                var throughput = _totalCount / ((double)stopWatch.ElapsedTicks / Stopwatch.Frequency);
                Console.WriteLine(Resources.Strings.ReportMessage, _totalCount, stopWatch.ElapsedMilliseconds, throughput);

                // Show latency histogram.
                ShowHistogram(results, 10);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Wait for user stop action.
                Console.WriteLine(Resources.Strings.ModuleStopMessage);
                Console.ReadKey();
            }
        }

        protected override void OnStop()
        {
            Logger.InfoFormat(Resources.Strings.ModuleStopped, ModuleName, MachineName);
        }

        #endregion

        #region Utility methods

        static void ThrowInnerException()
        {
            throw new Exception("Inner exception");
        }

        static void ThrowException()
        {
            try
            {
                ThrowInnerException();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception", ex);
            }
        }

        static void ThrowExceptionCallStack()
        {
            ThrowException();
        }

        private static void ShowHistogram(IList<double> values, int count, double? maxValue = null)
        {
            // Calculate mean value.
            var mean = values.Aggregate((s, v) => s + v) / values.Count;
            Console.WriteLine(Resources.Strings.HistogramMeanMessage, mean);

            // Calculate bounds values.
            var bound99 = (int)(values.Count * 99.0 / 100.0);
            Console.WriteLine(Resources.Strings.Histogram99Message, values[bound99]);
            var bound9999 = (int)(values.Count * 99.99 / 100.0);
            Console.WriteLine(Resources.Strings.Histogram9999Message, values[bound9999]);

            var min = values[0];
            var max = maxValue ?? values[bound99];
            var step = (max - min) / count;

            for (var i = 0; i < count; i++)
            {
                var lower = min + i * step;
                var upper = min + ((i + 1) * step);
                Console.WriteLine(Resources.Strings.HistogramPartMessage, i, lower, upper, values.Count(v => (v >= lower) && (v <= upper)));
            }
        }

        #endregion
    }
}
