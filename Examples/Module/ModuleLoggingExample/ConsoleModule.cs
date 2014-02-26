using System;
using NDepth.Module;

namespace NDepth.Examples.Module.ModuleLoggingExample
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
            Logger.Trace("Trace message");
            Logger.Debug("Debug message");
            Logger.Info("Info message");
            Logger.Warn("Warn message");
            Logger.Error("Error message");
            Logger.Fatal("Fatal message");

            // Inner exceptions example.
            try
            {
                ThrowExceptionCallStack();
            }
            catch (Exception ex)
            {
                Logger.Fatal("Fatal exception", ex);
            }
        }

        protected override void OnStop()
        {
            Logger.InfoFormat(Resources.Strings.ModuleStopped, ModuleName, MachineName);
        }

        #endregion

        #region Utility methods

        static void ThrowInnerException()
        {
            throw new Exception("Inner exception");
        }

        static void ThrowException()
        {
            try
            {
                ThrowInnerException();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception", ex);
            }
        }

        static void ThrowExceptionCallStack()
        {
            ThrowException();
        }

        #endregion
    }
}
