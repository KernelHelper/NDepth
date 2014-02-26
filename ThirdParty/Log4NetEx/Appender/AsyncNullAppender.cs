using log4net.Core;

namespace log4net.Async.Appender
{
    /// <summary>
    /// Asynchronous logging null appender. It skips all logging messages.
    /// </summary>
    public class AsyncNullAppender : IAsyncAppender
    {
        #region IAsyncAppender

        /// <summary>
        /// Gets or sets the name of this appender.
        /// </summary>
        /// <value>The name of the appender.</value>
        /// <remarks>
        /// <para>The name uniquely identifies the appender.</para>
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// Log the logging message in Appender specific way.
        /// </summary>
        /// <param name="loggingMessage">The message to log</param>
        /// <remarks>
        /// <para>
        /// This method is called to log a message into this appender.
        /// </para>
        /// </remarks>
        public void DoAppend(string loggingMessage)
        {
            // Do nothing here...
        }

        /// <summary>
        /// Log the logging event in Appender specific way.
        /// </summary>
        /// <param name="loggingEvent">The event to log</param>
        /// <remarks>
        /// <para>
        /// This method is called to log a message into this appender.
        /// </para>
        /// </remarks>
        public void DoAppend(LoggingEvent loggingEvent)
        {
            // Do nothing here...
        }

        /// <summary>
        /// Closes the appender and releases resources.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Releases any resources allocated within the appender such as file handles, 
        /// network connections, etc.
        /// </para>
        /// <para>
        /// It is a programming error to append to a closed appender.
        /// </para>
        /// </remarks>
        public void Close()
        {
            // Do nothing here...
        }

        #endregion
    }
}
