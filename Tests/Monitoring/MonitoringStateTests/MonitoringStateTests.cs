using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Business.BusinessObjects.Domain;
using NDepth.Module;
using NDepth.Monitoring;
using NDepth.Monitoring.Core;
using NUnit.Framework;

namespace NDepth.Tests.Monitoring.MonitoringStateTests
{
    public class MonitoringStateTests : ModuleBase
    {
        private DateTime _timestampStart;
        private DateTime _timestampEnd;
        private long _lastId;

        #region Module interface

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnRun()
        {
            TestIMonitoringRegister();
            TestIMonitoringRegisterRepeat();
            TestMonitoringModuleAndComponents();
            TestNumericPerformanceCounters();
            TestNumericPerformanceCountersNumberOfItems();
            TestNumericPerformanceCountersCountPerSecond();
            TestNumericPerformanceCountersAverageValue();
            TestNumericPerformanceCountersDeltaValue();
            TestNumericPerformanceCountersPercentValue();
            TestNumericPerformanceCountersElapsedTime();
            TestStringPerformanceCounters();
            TestSystemPerformanceCounters();
            TestSystemPerformanceCountersForeach();
        }

        protected override void OnStop()
        {
        }

        #endregion

        #region Test methods

        private void TestIMonitoringRegister()
        {
            _timestampStart = DateTime.UtcNow;

            Monitoring.Register(Severity.None, "Severity.None", "Monitoring event with None severity");
            Monitoring.Register(Severity.Trace, "Severity.Trace", "Monitoring event with Trace severity");
            Monitoring.Register(Severity.Debug, "Severity.Debug", "Monitoring event with Debug severity");
            Monitoring.Register(Severity.Info, "Severity.Info", "Monitoring event with Info severity");
            Monitoring.Register(Severity.Warning, "Severity.Warning", "Monitoring event with Warning severity");
            Monitoring.Register(Severity.Error, "Severity.Error", "Monitoring event with Error severity");
            Monitoring.Register(Severity.ErrorWithEmail, "Severity.ErrorWithEmail", "Monitoring event with ErrorWithEmail severity");
            Monitoring.Register(Severity.ErrorWithSms, "Severity.ErrorWithSms", "Monitoring event with ErrorWithSms severity");
            Monitoring.Register(Severity.Fatal, "Severity.Fatal", "Monitoring event with Fatal severity");
            Monitoring.Register(Severity.Notify, "Severity.Notify", "Monitoring event with Notify severity");
            Monitoring.Register(Severity.NotifyWithEmail, "Severity.NotifyWithEmail", "Monitoring event with NotifyWithEmail severity");
            Monitoring.Register(Severity.NotifyWithSms, "Severity.NotifyWithSms", "Monitoring event with NotifyWithSms severity");

            _timestampEnd = DateTime.UtcNow;

            // Wait for one second to complete.
            Thread.Sleep(1000);

            // Get last monitoring events.
            IList<MonitoringEvent> events = Monitoring.Fetch(_timestampStart, _timestampEnd, null, null, null, _lastId, 1000, true);

            // Check monitoring events count.
            Assert.AreEqual(24, events.Count, "Monitoring events count is not as designed!");

            // Update last monitoring event Id.
            _lastId += events.Count;
        }

        private void TestIMonitoringRegisterRepeat()
        {
            _timestampStart = DateTime.UtcNow;

            for (int i = 0; i < 150; i++)
            {
                Monitoring.RegisterRepeat(TimeSpan.FromSeconds(1), Severity.Info, "Monitoring repeat event", i + 1);
                Thread.Sleep(10);
            }

            _timestampEnd = DateTime.UtcNow;

            // Wait for one second to complete.
            Thread.Sleep(1000);

            // Get last monitoring events.
            IList<MonitoringEvent> events = Monitoring.Fetch(_timestampStart, _timestampEnd, null, null, null, _lastId, 1000, true);

            // Check monitoring events count.
            Assert.GreaterOrEqual(events.Count, 3, "Monitoring repeat events count is not as designed!");
            Assert.LessOrEqual(events.Count, 4, "Monitoring repeat events count is not as designed!");

            // Update last monitoring event Id.
            _lastId += events.Count;
        }

        private void TestMonitoringModuleAndComponents()
        {
            _timestampStart = DateTime.UtcNow;

            IMonitoringStateComponent subcomponent;
            IMonitoringStateComponent subsubcomponent;

            // Initialized test module severity.
            State.Severity = Severity.Info;

            // Initialized test module.
            State.ChangeState(NDepth.Monitoring.State.Initializing, "Test module initializing", "Initializing test module...");
            {
                // Initialized sub-component.
                Assert.AreEqual(0, State.Child.Count(), "Child components count of the test module is not 0!");
                subcomponent = State.AttachComponent("Sub-Component");
                Assert.AreEqual(1, State.Child.Count(), "Child components count of the test module is not 1!");
                Assert.AreEqual(State, subcomponent.Parent, "Sub-Component has parent which is different from the test module!");
                subcomponent.ChangeState(NDepth.Monitoring.State.Initializing, "Sub-Component initializing", "Initializing sub-component...");
                {
                    Assert.AreEqual(0, subcomponent.Child.Count(), "Child components count of the sub-component is not 0!");
                    subsubcomponent = subcomponent.AttachComponent("Sub-Sub-Component");
                    Assert.AreEqual(1, subcomponent.Child.Count(), "Child components count of the sub-component is not 1!");
                    Assert.AreEqual(subcomponent, subsubcomponent.Parent, "Sub-Sub-Component has parent which is different from the sub-component!");
                    subsubcomponent.ChangeState(NDepth.Monitoring.State.Initializing, "Sub-Sub-Component initializing", "Initializing sub-sub-component...");

                    // Register test monitoring event...
                    subsubcomponent.Monitoring.Register(Severity.Info, "Test", "Test message");

                    subsubcomponent.ChangeState(NDepth.Monitoring.State.Initialized, "Sub-Sub-Component initialized", "Sub-Sub-Component was successfully initialized...");                    
                }
                subcomponent.ChangeState(NDepth.Monitoring.State.Initialized, "Sub-Component initialized", "Sub-Component was successfully initialized...");                
            }
            State.ChangeState(NDepth.Monitoring.State.Initialized, "Test module initialized", "Test module was successfully initialized...");

            // Remove sub-sub-component.
            subsubcomponent.RemoveFromMonitoring();
            Assert.AreEqual(0, subcomponent.Child.Count(), "Child components count of the sub-component is not 0!");
            Assert.AreEqual(null, subsubcomponent.Parent, "Sub-Sub-Component has parent which is not null!");

            // Remove sub-sub-component.
            subcomponent.RemoveFromMonitoring();
            Assert.AreEqual(0, State.Child.Count(), "Child components count of the test module is not 0!");
            Assert.AreEqual(null, subcomponent.Parent, "Sub-Component has parent which is not null!");

            _timestampEnd = DateTime.UtcNow;

            // Wait for one second to complete.
            Thread.Sleep(1000);

            // Get last monitoring events.
            IList<MonitoringEvent> events = Monitoring.Fetch(_timestampStart, _timestampEnd, null, null, null, _lastId, 1000, true);

            // Check monitoring events count and its content.
            Assert.AreEqual(7, events.Count, "Monitoring repeat events count is not as designed!");
            Assert.AreEqual("Sub-Sub-Component", events[3].Component, "Monitoring event component name is not as designed!");
            Assert.AreEqual("Test", events[3].Title, "Monitoring event title is not as designed!");
            Assert.AreEqual("Test message", events[3].Description, "Monitoring event description is not as designed!");

            // Update last monitoring event Id.
            _lastId += events.Count;
        }

        private void TestNumericPerformanceCounters()
        {
            const float value = 12345678;

            IPerformanceCounterNumeric counter1 = State.AttachNumericCounter("Counter1");
            IPerformanceCounterNumeric counter2 = State.AttachNumericCounter("Counter2", () => value);

            counter1.SetRawValue(value);

            for (int i = 0; i < 10; i++)
            {
                counter1.Update();
                Assert.AreEqual(value, counter1.CounterValue, 0.001, "Numeric performance counter value is invalid!");
                counter2.Update();
                Assert.AreEqual(value, counter2.CounterValue, 0.001, "Numeric performance counter value is invalid!");
            }

            counter1.RemoveFromMonitoring();
            counter2.RemoveFromMonitoring();
        }

        private void TestNumericPerformanceCountersNumberOfItems()
        {
            IPerformanceCounterNumeric counter = State.AttachNumericCounter("Counter");

            counter.SetRawValue(100);

            for (int i = 1; i <= 10; i++)
            {
                counter.Increment();

                counter.Update();
                Assert.AreEqual(100 + i, counter.CounterValue, 0.001, "Numeric performance counter value is invalid!");
            }

            counter.SetRawValue(100);

            for (int i = 1; i <= 10; i++)
            {
                counter.IncrementBy(10);

                counter.Update();
                Assert.AreEqual(100 + 10 * i, counter.CounterValue, 0.001, "Numeric performance counter value is invalid!");
            }

            counter.SetRawValue(100);

            for (int i = 1; i <= 10; i++)
            {
                counter.Decrement();

                counter.Update();
                Assert.AreEqual(100 - i, counter.CounterValue, 0.001, "Numeric performance counter value is invalid!");
            }

            counter.SetRawValue(100);

            for (int i = 1; i <= 10; i++)
            {
                counter.DecrementBy(10);

                counter.Update();
                Assert.AreEqual(100 - 10 * i, counter.CounterValue, 0.001, "Numeric performance counter value is invalid!");
            }

            counter.RemoveFromMonitoring();
        }

        private void TestNumericPerformanceCountersCountPerSecond()
        {
            IPerformanceCounterNumeric counter = State.AttachNumericCounter("Counter", PerformanceCounterType.CountPerSecond);

            for (int i = 1; i <= 30; i++)
            {
                counter.Increment();
                counter.Update();
                Assert.LessOrEqual(counter.CounterValue, 10, "Numeric performance counter value is invalid!");

                Thread.Sleep(100);
            }

            counter.RemoveFromMonitoring();
        }

        private void TestNumericPerformanceCountersAverageValue()
        {
            IPerformanceCounterNumeric counter = State.AttachNumericCounter("Counter", PerformanceCounterType.AverageValue);

            for (int i = 1; i <= 1000; i++)
            {
                counter.Increment();
                if ((i % 100 == 0))
                    counter.IncrementBase();

                counter.Update();
                Assert.LessOrEqual(counter.CounterValue, 100, "Numeric performance counter value is invalid!");
            }

            counter.RemoveFromMonitoring();            
        }

        private void TestNumericPerformanceCountersDeltaValue()
        {
            var random = new Random();

            IPerformanceCounterNumeric counter = State.AttachNumericCounter("Counter", PerformanceCounterType.DeltaValue);

            for (int i = 1; i <= 100; i++)
            {
                counter.IncrementBy(random.Next(100));

                counter.Update();
                Assert.LessOrEqual(counter.CounterValue, 100, "Numeric performance counter value is invalid!");
            }

            counter.RemoveFromMonitoring();                        
        }

        private void TestNumericPerformanceCountersPercentValue()
        {
            IPerformanceCounterNumeric counter = State.AttachNumericCounter("Counter", PerformanceCounterType.PercentValue);

            for (int i = 1; i <= 1000; i++)
            {
                counter.IncrementBy(0.01f);
                if ((i % 100 == 0))
                    counter.IncrementBase();

                counter.Update();
                Assert.LessOrEqual(counter.CounterValue, 100, "Numeric performance counter value is invalid!");
            }

            counter.RemoveFromMonitoring();                        
        }

        private void TestNumericPerformanceCountersElapsedTime()
        {
            IPerformanceCounterNumeric counter = State.AttachNumericCounter("Counter", PerformanceCounterType.ElapsedTime);

            counter.SetRawValue(DateTime.UtcNow);

            Thread.Sleep(2500);

            counter.Update();
            Assert.GreaterOrEqual(counter.CounterValue, 2, "Numeric performance counter value is invalid!");
            Assert.LessOrEqual(counter.CounterValue, 3, "Numeric performance counter value is invalid!");

            counter.RemoveFromMonitoring();                                    
        }

        private void TestStringPerformanceCounters()
        {
            IPerformanceCounterString counter1 = State.AttachStringCounter("Counter1");
            IPerformanceCounterString counter2 = State.AttachStringCounter("Counter2", "Value2");
            IPerformanceCounterString counter3 = State.AttachStringCounter("Counter3", () => "Value3");

            counter1.SetRawValue("Value1");

            for (int i = 0; i < 10; i++)
            {
                counter1.Update();
                Assert.AreEqual("Value1", counter1.CounterValue, "String performance counter value is invalid!");
                counter2.Update();
                Assert.AreEqual("Value2", counter2.CounterValue, "String performance counter value is invalid!");
                counter3.Update();
                Assert.AreEqual("Value3", counter3.CounterValue, "String performance counter value is invalid!");
            }

            counter1.RemoveFromMonitoring();
            counter2.RemoveFromMonitoring();
            counter3.RemoveFromMonitoring();
        }        
        
        private void TestSystemPerformanceCounters()
        {
            IPerformanceCounterNumeric memory = State.AttachSystemCounter("Memory", "Available Bytes");
            IPerformanceCounterNumeric fiorate = State.AttachSystemCounter("System", "File Read Operations/sec");
            IPerformanceCounterNumeric paging = State.AttachSystemCounter("Processor", "% Processor Time", "_Total");
            IPerformanceCounterNumeric uptime = State.AttachSystemCounter("System", "System Up Time");

            for (int i = 0; i < 100; i++)
            {
                memory.Update();
                float value1 = memory.CounterValue;
                Assert.GreaterOrEqual(value1, 0, "System performance counter value is invalid!");

                fiorate.Update();
                float value2 = fiorate.CounterValue;
                Assert.GreaterOrEqual(value2, 0, "System performance counter value is invalid!");

                paging.Update();
                float value3 = paging.CounterValue;
                Assert.GreaterOrEqual(value3, 0, "System performance counter value is invalid!");

                uptime.Update();
                float value4 = uptime.CounterValue;
                Assert.GreaterOrEqual(value4, 0, "System performance counter value is invalid!");
            }

            memory.RemoveFromMonitoring();
            fiorate.RemoveFromMonitoring();
            paging.RemoveFromMonitoring();
            uptime.RemoveFromMonitoring();
        }

        private void TestSystemPerformanceCountersForeach()
        {
            var categories = PerformanceCounterSystem.GetSystemCounterCategories().ToList();
            Assert.NotNull(categories, "List of system performance counter categories should not be null!");
            Assert.Greater(categories.Count, 0, "List of system performance counter categories should not be empty!");
            foreach (var category in categories)
            {
                Assert.IsNotNullOrEmpty(category, "System performance counter category should not be empty!");
            }

            var instancesMemory = PerformanceCounterSystem.GetSystemCounterInstances("Memory").ToList();
            Assert.NotNull(instancesMemory, "List of memory performance counter instances should not be null!");
            Assert.AreEqual(0, instancesMemory.Count, "List of memory performance counter instances should be empty!");

            var instancesProcessor = PerformanceCounterSystem.GetSystemCounterInstances("Processor").ToList();
            Assert.NotNull(instancesProcessor, "List of processor performance counter instances should not be null!");
            Assert.Greater(instancesProcessor.Count, 0, "List of processor performance counter instances should not be empty!");
            foreach (var instance in instancesProcessor)
            {
                Assert.IsNotNullOrEmpty(instance, "Processor performance counter instance should not be empty!");
            }

            var countersMemory = PerformanceCounterSystem.GetSystemCounterNames("Memory").ToList();
            Assert.NotNull(countersMemory, "List of memory performance counters should not be null!");
            Assert.Greater(countersMemory.Count, 0, "List of memory performance counters should not be empty!");
            foreach (var counter in countersMemory)
            {
                Assert.IsNotNullOrEmpty(counter, "Memory performance counter should not be empty!");
            }

            var countersProcessor = PerformanceCounterSystem.GetSystemCounterNames("Processor").ToList();
            Assert.NotNull(countersProcessor, "List of processor performance counters should not be null!");
            Assert.Greater(countersProcessor.Count, 0, "List of processor performance counters should not be empty!");
            foreach (var counter in countersProcessor)
            {
                Assert.IsNotNullOrEmpty(counter, "Processor performance counter should not be empty!");
            }
        }

        #endregion 
    }
}