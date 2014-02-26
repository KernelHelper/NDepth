using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Xml.Linq;

namespace NDepth.Module.WinService
{
    /// <summary>
    /// Windows Service bootstrap class.
    /// </summary>
    public class WinServiceBootstrap
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        /// <summary>Windows Service executable</summary>
        public static string WinServiceExe = Path.GetFileName(Assembly.GetEntryAssembly().Location);
        /// <summary>Windows Service name</summary>
        public static string WinServiceName = WinServiceExe.Replace(".exe", "");
        /// <summary>Windows Service description </summary>
        public static string WinServiceDescription = "Windows Service";
        /// <summary>Windows Service path</summary>
        public static string WinServicePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        /// <summary>Main entry point of Windows Service</summary>
        /// <param name="service">Windows Service to run</param>
        /// <returns>Result value</returns>
        public static int Run<T>(T service) where T : ServiceBase
        {
            // Update Windows Service name and description.
            string module = WinServicePath + Path.DirectorySeparatorChar + "module.xml";
            if (File.Exists(module) && !UpdateWinServiceProperties(module))
                throw new ArgumentException(string.Format(Resources.Strings.ParseModuleConfigFailed, module));

            if (Environment.UserInteractive)
            {
                // Show help.
                if (Environment.CommandLine.Contains("-help") || Environment.CommandLine.Contains("/help") || Environment.CommandLine.Contains("-?") || Environment.CommandLine.Contains("/?"))
                {
                    if (!AllocConsole())
                        throw new InvalidOperationException(Resources.Strings.AllocateConsoleFailed);

                    Console.Title = WinServiceName;

                    Console.WriteLine(Resources.Strings.UsageMessage1, WinServiceName);
                    Console.WriteLine(Resources.Strings.UsageMessage2, WinServiceName);
                    Console.WriteLine(Resources.Strings.UsageMessage3, WinServiceName);
                    Console.WriteLine(Resources.Strings.UsageMessage4, WinServiceName);
                    Console.WriteLine(Resources.Strings.UsageMessage5, WinServiceName);
                    Console.WriteLine(Resources.Strings.UsageMessage6, WinServiceName);

                    if (!FreeConsole())
                        throw new InvalidOperationException(Resources.Strings.FreeConsoleFailed);

                    return 0;
                }            

                // Install Windows Service.
                if (Environment.CommandLine.Contains("-install") || Environment.CommandLine.Contains("/install"))
                {
                    ServiceInstaller.InstallService(WinServiceName, WinServiceDescription, WinServicePath + Path.DirectorySeparatorChar + WinServiceExe);
                    return 0;
                }
                // Uninstall Windows Service.
                if (Environment.CommandLine.Contains("-uninstall") || Environment.CommandLine.Contains("/uninstall"))
                {
                    ServiceInstaller.UninstallService(WinServiceName);
                    return 0;
                }
                // Start Windows Service.
                if (Environment.CommandLine.Contains("-start") || Environment.CommandLine.Contains("/start"))
                {
                    ServiceInstaller.StartService(WinServiceName);
                    return 0;
                }
                // Stop Windows Service.
                if (Environment.CommandLine.Contains("-stop") || Environment.CommandLine.Contains("/stop"))
                {
                    ServiceInstaller.StopService(WinServiceName);
                    return 0;
                }
                // Run Windows Service in console mode.
                if (Environment.CommandLine.Contains("-console") || Environment.CommandLine.Contains("/console") || Debugger.IsAttached)
                {
                    if (!AllocConsole())
                        throw new InvalidOperationException(Resources.Strings.AllocateConsoleFailed);

                    Console.Title = WinServiceName;

                    Console.WriteLine(Resources.Strings.ServiceStarting, WinServiceName);

                    // Invoke OnStart method.
                    MethodInfo startMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.NonPublic | BindingFlags.Instance, null, new [] { typeof(string[]) }, null);
                    startMethod.Invoke(service, new object[] { new string[0] });

                    Console.WriteLine(Resources.Strings.ServiceStarted, WinServiceName);

                    Console.ReadLine();

                    Console.WriteLine(Resources.Strings.ServiceStopping, WinServiceName);

                    // Invoke OnStop method.
                    MethodInfo stopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.NonPublic | BindingFlags.Instance);
                    stopMethod.Invoke(service, null);

                    Console.WriteLine(Resources.Strings.ServiceStopped, WinServiceName);

                    if (!FreeConsole())
                        throw new InvalidOperationException(Resources.Strings.FreeConsoleFailed);

                    // Dispose service.
                    service.Dispose();

                    return 0;
                }
            }

            // Run Windows Service.
            ServiceBase.Run(service);
            return service.ExitCode;
        }

        private static bool UpdateWinServiceProperties(string module)
        {
            try
            {
                // Parse mdule.xml
                XDocument moduleDoc = XDocument.Load(module);

                // Get the root element.
                XElement root = moduleDoc.Element("Module");
                if (root == null)
                    return false;

                // Try to get module name and module description.
                XElement name = root.Element("Name");
                WinServiceName = (name != null) ? name.Value : WinServiceName;
                XElement description = root.Element("Description");
                WinServiceDescription = (description != null) ? description.Value : WinServiceDescription;

                return true;
            }
            catch
            {
                // Catch all exceptions.
                return false;
            }
        }
    }
}
