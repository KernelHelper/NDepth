// ReSharper disable ConvertToAutoProperty
// ReSharper disable EmptyConstructor
// ReSharper disable ImplicitlyCapturedClosure
// ReSharper disable UnusedVariable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NDepth.Examples.Common.BlockingCollectionExample
{
    public sealed class ValueEntry
    {
        private int _index;
        private long _value;

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public long Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }

    class Program
    {
        private static int _countPerThread = 100000;
        private static int _producersCount = 1;
        private static int _consumersCount = 1;
        private static int _totalCount;

        private static void Main(string[] args)
        {
            try
            {
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
                if ((args.Length >= 3) && (!int.TryParse(args[2], out _consumersCount)) || (_consumersCount < 1))
                {
                    Console.WriteLine(Resources.Strings.UsageMessage);
                    throw new ArgumentException(Resources.Strings.InvalidArgumentConsumersCount);
                }

                _countPerThread = _countPerThread / _producersCount;
                _totalCount = _countPerThread * _producersCount;
                
                var stopWatchProduce = Stopwatch.StartNew();
                var stopWatchTotal = Stopwatch.StartNew();
                var results = new double[_totalCount];
                for (var i = 0; i < _totalCount; i++)
                    results[i] = 0;

                var collection = new BlockingCollection<ValueEntry>(1024);

                // Create and start consumer tasks.
                var consumers = new Task[_consumersCount];
                for (var consumerIndex = 0; consumerIndex < _consumersCount; consumerIndex++)
                {
                    consumers[consumerIndex] = Task.Factory.StartNew(state =>
                    {
                        var index = (int)state;
                        foreach (var value in collection.GetConsumingEnumerable())
                        {
                            // Console.WriteLine(Resources.Strings.ConsumeMessage, value.Index, results[value.Index], index);                    

                            results[value.Index] += stopWatchTotal.ElapsedTicks - value.Value;
                        }
                    }, consumerIndex, TaskCreationOptions.LongRunning);
                }

                // Restart stopwatch timers.
                stopWatchProduce.Restart();
                stopWatchTotal.Restart();

                // Create and start producer tasks.
                var producers = new Task[_producersCount];
                for (var producerIndex = 0; producerIndex < _producersCount; producerIndex++)
                {
                    producers[producerIndex] = Task.Factory.StartNew(state =>
                    {
                        var index = (int)state;
                        for (var i = 0; i < _countPerThread; i++)
                        {
                            var value = new ValueEntry { Index = index * _countPerThread + i, Value = stopWatchTotal.ElapsedTicks };

                            // Console.WriteLine(Resources.Strings.ProduceMessage, value.Index, index);

                            collection.Add(value);
                        }
                    }, producerIndex, TaskCreationOptions.LongRunning);
                }

                // Wait for producing complete.
                Task.WaitAll(producers);
                collection.CompleteAdding();
                stopWatchProduce.Stop();

                // Wait for consuming complete.
                Task.WaitAll(consumers);
                stopWatchTotal.Stop();

                // Adjust result table.
                var frequency = Stopwatch.Frequency / (1000.0 * 1000.0);
                for (var i = 0; i < _totalCount; i++)
                    results[i] /= frequency;
                Array.Sort(results);

                // Show report messages.
                var throughputProduce = _totalCount / ((double)stopWatchProduce.ElapsedTicks / Stopwatch.Frequency);
                Console.WriteLine(Resources.Strings.ReportMessageForProducers, _totalCount, stopWatchProduce.ElapsedMilliseconds, throughputProduce);
                var throughputTotal = _totalCount / ((double)stopWatchTotal.ElapsedTicks / Stopwatch.Frequency);
                Console.WriteLine(Resources.Strings.ReportMessageForConsumers, _totalCount, stopWatchTotal.ElapsedMilliseconds, throughputTotal);

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
                Console.WriteLine(Resources.Strings.StopMessage);
                Console.ReadKey();                
            }
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
    }
}
// ReSharper restore ConvertToAutoProperty
// ReSharper restore EmptyConstructor
// ReSharper restore ImplicitlyCapturedClosure
// ReSharper restore UnusedVariable