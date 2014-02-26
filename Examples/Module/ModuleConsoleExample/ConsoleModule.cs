using System;
using NDepth.Module;

namespace NDepth.Examples.Module.ModuleConsoleExample
{
    public class ConsoleModule : ModuleBase
    {
        #region Module interface

        protected override void OnStart(string[] args)
        {
            Logger.InfoFormat(Resources.Strings.ModuleStarted, ModuleName, MachineName);
        }

        protected override void OnRun()
        {
            // Wait for user stop action.
            Console.WriteLine(Resources.Strings.ModuleStopMessage);
            Console.ReadKey();
        }

        protected override void OnStop()
        {
            Logger.InfoFormat(Resources.Strings.ModuleStopped, ModuleName, MachineName);
        }

        #endregion
    }
}
