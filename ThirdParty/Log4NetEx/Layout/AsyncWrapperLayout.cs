using System.Globalization;
using System.IO;
using log4net.Core;
using log4net.Layout;

namespace log4net.Async.Layout
{
    /// <summary>
    /// Asynchronous logging wrapper layout. Adapter for any of log4net layouts.
    /// </summary>
    public class AsyncWrapperLayout : IAsyncLayout, IOptionHandler
    {
        #region Options

        /// <summary>
        /// Gets or sets the wrapped layout.
        /// </summary>
        public ILayout Layout { get; set; }

        #endregion

        #region IAsyncLayout

        /// <summary>
        /// The content type output by this layout. 
        /// </summary>
        public string ContentType { get { return Layout.ContentType; } }

        /// <summary>
        /// The header for the layout format.
        /// </summary>
        public string Header { get { return Layout.Header; } } 

        /// <summary>
        /// The footer for the layout format.
        /// </summary>
        public string Footer { get { return Layout.Footer; } }

        /// <summary>
        /// Flag indicating if this layout doest not show exception.
        /// </summary>
        public bool IgnoresException { get { return Layout.IgnoresException; } }

        /// <summary>
        /// Format logging event to string.
        /// </summary>
        /// <param name="loggingEvent">The event to format</param>
        /// <returns>String with the formatted event</returns> 
        public string Format(LoggingEvent loggingEvent)
        {
            var writer = new StringWriter(CultureInfo.InvariantCulture);
            Layout.Format(writer, loggingEvent);
            return writer.ToString();
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
            Layout.Format(writer, loggingEvent);
        }

        #endregion

        #region IOptionHandler

        /// <summary>
        /// Activate the options that were previously set with calls to properties.
        /// </summary>
        virtual public void ActivateOptions()
        {
            // Check the wrapped layout.
            if (Layout == null)
                throw new LogException(Resources.Strings.WrappedLayoutIsNull);
        }

        #endregion
    }
}
