using System;
using System.Threading;
using NDepth.Module.Test;
using NUnit.Framework;

namespace NDepth.Tests.Module.ModuleWithDBTests
{
    [TestFixture]
    [Category("Module with DB tests")]
    public class TestModule
    {
        [Test]
        public void ModuleInitializationTest()
        {
            // Create new test module bootstrap with DB support.
            using (var bootstrap = new ModuleBootstrapTestWithDB<ModuleWithDBTests>())
            {
                var from = DateTime.UtcNow;

                // Start module.
                bootstrap.OnStart(null);

                // Run module.
                bootstrap.OnRun();

                // Stop module.
                bootstrap.OnStop();

                var to = DateTime.UtcNow;

                // Check unit test assertions.
                Assert.IsTrue(bootstrap.Module.OnStartCalled, "ModuleWithDBTests.OnStart() was not called!");
                Assert.IsTrue(bootstrap.Module.OnRunCalled, "ModuleWithDBTests.OnRun() was not called!");
                Assert.IsTrue(bootstrap.Module.OnStopCalled, "ModuleWithDBTests.OnStop() was not called!");

                // Sleep for a while to let monitoring events to be flushed.
                Thread.Sleep(1000);

                // Fetch and check monitoring events with paging.
                bootstrap.Module.FetchMonitoringEvents(from, to, 2, true);
                bootstrap.Module.FetchMonitoringEvents(from, to, 2, false);
            }
        }
    }
}
