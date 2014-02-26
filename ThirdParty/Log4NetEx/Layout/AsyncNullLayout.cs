using System.IO;
using log4net.Core;

namespace log4net.Async.Layout
{
    /// <summary>
    /// Asynchronous logging null layout. It returns an empty string for every logging message.
    /// </summary>
    public class AsyncNullLayout : IAsyncLayout
    {
        #region IAsyncLayout

        /// <summary>
        /// The content type output by this layout. 
        /// </summary>
        public string ContentType { get { return "text/plain"; } }

        /// <summary>
        /// The header for the layout format.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// The footer for the layout format.
        /// </summary>
        public string Footer { get; set; }

        /// <summary>
        /// Flag indicating if this layout doest not show exception.
        /// </summary>
        public bool IgnoresException { get; set; }

        /// <summary>
        /// Format logging event to string.
        /// </summary>
        /// <param name="loggingEvent">The event to format</param>
        /// <returns>String with the formatted event</returns> 
        public string Format(LoggingEvent loggingEvent)
        {
            return string.Empty;
        }

        /// <summary>
        /// Implement this method to create your own layout format.
        /// </summary>
        /// <param name="writer">The TextWriter to write the formatted event to</param>
        /// <param name="loggingEvent">The event to format</param>
        /// <remarks>
        /// <para>
        /// This method is called by an appender to format
        /// the <paramref name="loggingEvent"/> as text and output to a writer.
        /// </para>
        /// <para>
        /// If the caller does not have a <see cref="TextWriter"/> and prefers the
        /// event to be formatted as a <see cref="string"/> then the following
        /// code can be used to format the event into a <see cref="StringWriter"/>.
        /// </para>
        /// <code lang="C#">
        /// StringWriter writer = new StringWriter();
        /// Layout.Format(writer, loggingEvent);
        /// string formattedEvent = writer.ToString();
        /// </code>
        /// </remarks>
        public void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            // Do nothing here...
        }

        #endregion
    }
}
