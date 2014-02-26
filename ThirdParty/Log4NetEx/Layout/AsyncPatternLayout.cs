using log4net.Core;
using log4net.Layout;

namespace log4net.Async.Layout
{
    /// <summary>
    /// Asynchronous logging pattern layout. Adapter for the log4net pattern layout.
    /// </summary>
    public class AsyncPatternLayout : PatternLayout, IAsyncLayout
    {
        #region IAsyncLayout

        /// <summary>
        /// Format logging event to string.
        /// </summary>
        /// <param name="loggingEvent">The event to format</param>
        /// <returns>String with the formatted event</returns> 
        public new string Format(LoggingEvent loggingEvent)
        {
            return base.Format(loggingEvent);
        }

        #endregion
    }
}
