using log4net.Appender;

namespace log4net.Async.Appender
{
    /// <summary>
    /// Asynchronous logging appender interface.
    /// </summary>
    public interface IAsyncAppender : IAppender
    {
        /// <summary>
        /// Log the logging message in Appender specific way.
        /// </summary>
        /// <param name="loggingMessage">The message to log</param>
        /// <remarks>
        /// <para>
        /// This method is called to log a message into this appender.
        /// </para>
        /// </remarks>
        void DoAppend(string loggingMessage);
    }
}
