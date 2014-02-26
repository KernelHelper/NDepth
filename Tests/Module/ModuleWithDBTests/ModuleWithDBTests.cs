using System;
using System.Linq;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Module;
using NUnit.Framework;

namespace NDepth.Tests.Module.ModuleWithDBTests
{
    public class ModuleWithDBTests : ModuleBase
    {
        public bool OnStartCalled { get; private set; }
        public bool OnRunCalled { get; private set; }
        public bool OnStopCalled { get; private set; }

        #region Module interface

        protected override void OnStart(string[] args)
        {
            // Check base module components.
            Assert.NotNull(Container);
            Assert.NotNull(Logger);
            Assert.NotNull(Monitoring);
            Assert.NotNull(State);

            Logger.Info("OnStart() begin");
            Monitoring.Register(Severity.Info, "ModuleWithDBTests", "Test module starting...");

            OnStartCalled = true;

            // Check base module properties.
            Assert.IsNotNullOrEmpty(MachineName);
            Assert.IsNotNullOrEmpty(MachineDescription);
            Assert.IsNotNullOrEmpty(MachineAddress);
            Assert.IsNotNullOrEmpty(ModuleName);
            Assert.IsNotNullOrEmpty(ModuleDescription);
            Assert.IsNotNullOrEmpty(ModuleVersion);

            Monitoring.Register(Severity.Info, "ModuleWithDBTests", "Test module started!");
            Logger.Info("OnStart() end");
        }

        protected override void OnRun()
        {
            Logger.Info("OnRun() begin");
            Monitoring.Register(Severity.Info, "ModuleWithDBTests", "Test module running...");

            OnRunCalled = true;

            Logger.Info("OnRun() end");
            Monitoring.Register(Severity.Info, "ModuleWithDBTests", "Test module run!");
        }

        protected override void OnStop()
        {
            Logger.Info("OnStop() begin");
            Monitoring.Register(Severity.Info, "ModuleWithDBTests", "Test module stopping...");

            OnStopCalled = true;

            Logger.Info("OnStop() end");
            Monitoring.Register(Severity.Info, "ModuleWithDBTests", "Test module stopped!");
        }

        #endregion

        #region Utility methods

        public void FetchMonitoringEvents(DateTime from, DateTime to, int page, bool forward)
        {
            var count = 0;
            var lastId = forward ? 0L : long.MaxValue;
            var monitoringEventsList = Monitoring.Fetch(from, to, null, null, null, null, page, forward);
            while (monitoringEventsList.Count > 0)
            {
                count += monitoringEventsList.Count;
                foreach (var monitoringEvent in monitoringEventsList)
                {
                    if (forward)
                        Assert.Greater(monitoringEvent.Id, lastId);
                    else
                        Assert.Less(monitoringEvent.Id, lastId);
                    Assert.GreaterOrEqual(monitoringEvent.Timestamp, from.AddSeconds(-1));
                    Assert.LessOrEqual(monitoringEvent.Timestamp, to.AddSeconds(1));
                    Assert.IsNotNullOrEmpty(monitoringEvent.Machine);
                    Assert.IsNotNullOrEmpty(monitoringEvent.Module);
                    Assert.IsNotNullOrEmpty(monitoringEvent.Component);
                    Assert.IsNotNullOrEmpty(monitoringEvent.Title);
                    Assert.IsNotNullOrEmpty(monitoringEvent.Description);
                    lastId = monitoringEvent.Id;
                }

                monitoringEventsList = Monitoring.Fetch(from, to, null, null, null, monitoringEventsList.Last().Id, page, forward);
            }

            Assert.Greater(count, 0, "Some monitoring events should be fetched!");
        }

        #endregion
    }
}
