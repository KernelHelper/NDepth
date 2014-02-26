using NDepth.Business.BusinessObjects.Common;
using NDepth.Module;
using NUnit.Framework;

namespace NDepth.Tests.Module.ModuleTests
{
    public class ModuleBaseTests : ModuleBase
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
            Monitoring.Register(Severity.Info, "ModuleBaseTests", "Test module starting...");

            OnStartCalled = true;

            // Check base module properties.
            Assert.IsNotNullOrEmpty(MachineName);
            Assert.IsNotNullOrEmpty(MachineDescription);
            Assert.IsNotNullOrEmpty(MachineAddress);
            Assert.IsNotNullOrEmpty(ModuleName);
            Assert.IsNotNullOrEmpty(ModuleDescription);
            Assert.IsNotNullOrEmpty(ModuleVersion);

            Monitoring.Register(Severity.Info, "ModuleBaseTests", "Test module started!");
            Logger.Info("OnStart() end");
        }

        protected override void OnRun()
        {
            Logger.Info("OnRun() begin");
            Monitoring.Register(Severity.Info, "ModuleBaseTests", "Test module running...");

            OnRunCalled = true;

            Logger.Info("OnRun() end");
            Monitoring.Register(Severity.Info, "ModuleBaseTests", "Test module run!");
        }

        protected override void OnStop()
        {
            Logger.Info("OnStop() begin");
            Monitoring.Register(Severity.Info, "ModuleBaseTests", "Test module stopping...");

            OnStopCalled = true;

            Logger.Info("OnStop() end");
            Monitoring.Register(Severity.Info, "ModuleBaseTests", "Test module stopped!");
        }

        #endregion
    }
}
