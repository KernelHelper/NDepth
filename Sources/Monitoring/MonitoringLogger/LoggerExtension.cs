using Common.Logging;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring.Logger
{
    internal static class LoggerExtension
    {
        internal static void LogWithSeverity(this ILog logger, Severity severity, string format, params object[] args)
        {
            if (severity < Severity.Trace)
            {
                // Do nothing.
            }
            else if (severity < Severity.Debug)
            {
                if (logger.IsTraceEnabled)
                    logger.TraceFormat(format, args);
            }
            else if (severity < Severity.Info)
            {
                if (logger.IsDebugEnabled)
                    logger.DebugFormat(format, args);
            }
            else if (severity < Severity.Warning)
            {
                if (logger.IsInfoEnabled)
                    logger.InfoFormat(format, args);
            }
            else if (severity < Severity.Error)
            {
                if (logger.IsWarnEnabled)
                    logger.WarnFormat(format, args);
            }
            else if (severity < Severity.Fatal)
            {
                if (logger.IsErrorEnabled)
                    logger.ErrorFormat(format, args);
            }
            else if (severity < Severity.Notify)
            {
                if (logger.IsFatalEnabled)
                    logger.FatalFormat(format, args);
            }
        }
    }
}
