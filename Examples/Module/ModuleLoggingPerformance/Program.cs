using NDepth.Module;

namespace NDepth.Examples.Module.ModuleLoggingPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create new console module bootstrap.
            using (var bootstrap = new ModuleBootstrapConsole<ConsoleModule>())
            {
                // Start module.
                bootstrap.OnStart(args);

                // Run module.
                bootstrap.OnRun();

                // Stop module.
                bootstrap.OnStop();                
            }
        }
    }
}
