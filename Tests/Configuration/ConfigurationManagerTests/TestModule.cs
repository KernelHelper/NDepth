using NDepth.Module.Test;
using NUnit.Framework;

namespace NDepth.Tests.Configuration.ConfigurationManagerTests
{
    [TestFixture]
    [Category("Configuration module tests")]
    public class TestModule
    {
        [Test]
        public void ModuleInitializationTest()
        {
            // Create new test module bootstrap.
            var bootstrap = new ModuleBootstrapTestMonitoring<ConfigurationModuleTests>();

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