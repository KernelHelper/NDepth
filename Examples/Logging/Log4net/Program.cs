using System;
using System.Collections.Specialized;
using Common.Logging;
using Common.Logging.Log4Net;

namespace NDepth.Examples.Logging.Log4Net
{
    class Program
    {
        static void Main()
        {
            // Create logger factory.
            LogManager.Adapter = new Log4NetLoggerFactoryAdapter(new NameValueCollection { { "configType", "FILE-WATCH" }, { "configFile", "~/log4net.config" } });

            // Get logger instance.
            ILog log = LogManager.GetCurrentClassLogger();

            // Trace examples.
            log.Trace("Trace message");
            log.Trace("Trace exception", new Exception("Some exception"));
            log.TraceFormat("Trace formatted message - {0}", 123);
            log.TraceFormat("Trace formatted exception - {0}", new Exception("Some exception"), 123);
            log.Trace(m => m("Trace" + " " + "message" + " " + "expression"));
            log.Trace(m => m("Trace" + " " + "exception" + " " + "expression"), new Exception("Some exception"));

            // Debug examples.
            log.Debug("Debug message");
            log.Debug("Debug exception", new Exception("Some exception"));
            log.DebugFormat("Debug formatted message - {0}", 123);
            log.DebugFormat("Debug formatted exception - {0}", new Exception("Some exception"), 123);
            log.Debug(m => m("Debug" + " " + "message" + " " + "expression"));
            log.Debug(m => m("Debug" + " " + "exception" + " " + "expression"), new Exception("Some exception"));

            // Info examples.
            log.Info("Info message");
            log.Info("Info exception", new Exception("Some exception"));
            log.InfoFormat("Info formatted message - {0}", 123);
            log.InfoFormat("Info formatted exception - {0}", new Exception("Some exception"), 123);
            log.Info(m => m("Info" + " " + "message" + " " + "expression"));
            log.Info(m => m("Info" + " " + "exception" + " " + "expression"), new Exception("Some exception"));

            // Warn examples.
            log.Warn("Warn message");
            log.Warn("Warn exception", new Exception("Some exception"));
            log.WarnFormat("Warn formatted message - {0}", 123);
            log.WarnFormat("Warn formatted exception - {0}", new Exception("Some exception"), 123);
            log.Warn(m => m("Warn" + " " + "message" + " " + "expression"));
            log.Warn(m => m("Warn" + " " + "exception" + " " + "expression"), new Exception("Some exception"));

            // Error examples.
            log.Error("Error message");
            log.Error("Error exception", new Exception("Some exception"));
            log.ErrorFormat("Error formatted message - {0}", 123);
            log.ErrorFormat("Error formatted exception - {0}", new Exception("Some exception"), 123);
            log.Error(m => m("Error" + " " + "message" + " " + "expression"));
            log.Error(m => m("Error" + " " + "exception" + " " + "expression"), new Exception("Some exception"));

            // Fatal examples.
            log.Fatal("Fatal message");
            log.Fatal("Fatal exception", new Exception("Some exception"));
            log.FatalFormat("Fatal formatted message - {0}", 123);
            log.FatalFormat("Fatal formatted exception - {0}", new Exception("Some exception"), 123);
            log.Fatal(m => m("Fatal" + " " + "message" + " " + "expression"));
            log.Fatal(m => m("Fatal" + " " + "exception" + " " + "expression"), new Exception("Some exception"));

            // Inner exceptions example.
            try
            {
                ThrowExceptionCallStack();
            }
            catch (Exception ex)
            {
                log.Fatal("Fatal exception", ex);
            }
        }

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
    }
}
