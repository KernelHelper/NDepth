using NDepth.Module.Test;
using NUnit.Framework;

namespace NDepth.Tests.Monitoring.MonitoringStateTests
{
    [TestFixture]
    [Category("Monitoring state tests")]
    public class TestModule
    {
        [Test]
        public void ModuleInitializationTest()
        {
            // Create new test module bootstrap.
            var bootstrap = new ModuleBootstrapTestMonitoring<MonitoringStateTests>();

            // Dispose module at the end.
            using (bootstrap.Module)
            {
                // Start module.
                bootstrap.OnStart(null);

                // Run module.
                bootstrap.OnRun();

                // Stop module.
                bootstrap.OnStop();                
            }
        }
    }
}