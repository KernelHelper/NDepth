// ReSharper disable AccessToModifiedClosure
// ReSharper disable ConvertToAutoProperty
// ReSharper disable ImplicitlyCapturedClosure
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NDepth.Examples.Common.HotSwapExample
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
        private static int _totalCount;

        static void Swap<T>(ref T foo, ref T bar)
        {
            var tmp = foo;
            foo = bar;
            bar = tmp;
        }

        static void Main(string[] args)
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

                _countPerThread = _countPerThread / _producersCount;
                _totalCount = _countPerThread * _producersCount;

                var stopWatchProduce = Stopwatch.StartNew();
                var stopWatchTotal = Stopwatch.StartNew();
                var results = new double[_totalCount];
                for (var i = 0; i < _totalCount; i++)
                    results[i] = 0;

                var locker = new object();
                var wakeup = new AutoResetEvent(false);
                var producingCollection = new List<ValueEntry>();
                var consumingCollection = new List<ValueEntry>();

                // Create and start consumer task.
                var consumer = Task.Factory.StartNew(() =>
                {
                    var exit = false;

                    while (!exit)
                    {
                        // Perform the hot swap operation.
                        lock (locker)
                        {
                            Swap(ref producingCollection, ref consumingCollection);
                        }

                        // Consume items.
                        foreach (var value in consumingCollection)
                        {
                            // Check for last consuming item.
                            if (value == null)
                            {
                                exit = true;
                                break;
                            }

                            // Console.WriteLine(Resources.Strings.ConsumeMessage, value.Index, results[value.Index]);

                            results[value.Index] += stopWatchTotal.ElapsedTicks - value.Value;
                        }

                        // Clear consuming collection.
                        consumingCollection.Clear();

                        // Wait for next available produced items and wakeup.
                        if (!exit)
                            wakeup.WaitOne();
                    }
                }, TaskCreationOptions.LongRunning);

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
                            var value = new ValueEntry
                            {
                                Index = index * _countPerThread + i,
                                Value = stopWatchTotal.ElapsedTicks
                            };

                            // Console.WriteLine(Resources.Strings.ProduceMessage, value.Index, index);

                            var isFirstItem = false;

                            // Safe add item to the producing collection.
                            lock (locker)
                            {
                                // Check for the first item.
                                if (producingCollection.Count == 0)
                                    isFirstItem = true;

                                // Add item.
                                producingCollection.Add(value);
                            }

                            // Notify about new items available.
                            if (isFirstItem)
                                wakeup.Set();
                        }
                    }, producerIndex, TaskCreationOptions.LongRunning);
                }

                // Wait for producing complete.
                Task.WaitAll(producers);
                stopWatchProduce.Stop();

                // Add last special item to stop the consumer.
                lock (locker)
                {
                    producingCollection.Add(null);
                }
                wakeup.Set();

                // Wait for consuming complete.
                Task.WaitAll(consumer);
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
// ReSharper restore AccessToModifiedClosure
// ReSharper restore ConvertToAutoProperty
// ReSharper restore ImplicitlyCapturedClosure