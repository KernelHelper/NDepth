using System.IO;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;

namespace log4net.Async.Appender
{
    /// <summary>
    /// Asynchronous logging wrapper appender. Adapter for any of log4net appenders.
    /// </summary>
    public class AsyncWrapperAppender : IAsyncAppender, IOptionHandler
    {
        #region Options

        /// <summary>
        /// Gets or sets the wrapped appender.
        /// </summary>
        public AppenderSkeleton Appender { get; set; }

        #endregion

        #region IAsyncAppender

        /// <summary>
        /// Gets or sets the name of this appender.
        /// </summary>
        /// <value>The name of the appender.</value>
        /// <remarks>
        /// <para>The name uniquely identifies the appender.</para>
        /// </remarks>
        public string Name
        {
            get { return Appender.Name; }
            set { Appender.Name = value; }
        }

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
            Appender.DoAppend(new LoggingEvent(null, null, null, null, loggingMessage, null));
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
            Appender.DoAppend(loggingEvent);
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
            Appender.Close();
        }

        #endregion

        #region IOptionHandler

        /// <summary>
        /// Activate the options that were previously set with calls to properties.
        /// </summary>
        virtual public void ActivateOptions()
        {
            // Check the wrapped appender.
            if (Appender == null)
                throw new LogException(Resources.Strings.WrappedAppenderIsNull);

            // Change the layout of the wrapped appender.
            Appender.Layout = new WrappedLayout();
        }

        #endregion

        #region Utility classes

        private class WrappedLayout : ILayout
        {
            public string ContentType { get { return "text/plain"; } }
            public string Header { get { return ""; } }
            public string Footer { get { return ""; } }
            public bool IgnoresException { get { return true; } }

            public void Format(TextWriter writer, LoggingEvent loggingEvent)
            {
                writer.Write(loggingEvent.MessageObject);
            }
        }

        #endregion
    }
}
