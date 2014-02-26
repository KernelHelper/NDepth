using NDepth.Module.Test;
using NUnit.Framework;

namespace NDepth.Tests.Module.ModuleTests
{
    [TestFixture]
    [Category("Module base tests")]
    public class TestModule
    {
        [Test]
        public void ModuleInitializationTest()
        {
            // Create new test module bootstrap.
            using (var bootstrap = new ModuleBootstrapTestSimple<ModuleBaseTests>())
            {
                // Start module.
                bootstrap.OnStart(null);

                // Run module.
                bootstrap.OnRun();

                // Stop module.
                bootstrap.OnStop();

                // Check unit test assertions.
                Assert.IsTrue(bootstrap.Module.OnStartCalled, "ModuleBaseTests.OnStart() was not called!");
                Assert.IsTrue(bootstrap.Module.OnRunCalled, "ModuleBaseTests.OnRun() was not called!");
                Assert.IsTrue(bootstrap.Module.OnStopCalled, "ModuleBaseTests.OnStop() was not called!");
            }
        }
    }
}
