using NDepth.Module;
using NDepth.Module.WinService;

namespace NDepth.Cluster.ClusterAdminService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            // Create new module and run it as a Windows Service.
            using (var bootstrap = new ModuleBootstrapService<ClusterAdminModule>())
            {
                WinServiceBootstrap.Run(bootstrap);
            }
        }
    }
}
