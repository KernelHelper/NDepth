using log4net.Core;
using log4net.Layout;

namespace log4net.Async.Layout
{
    /// <summary>
    /// Asynchronous logging layout interface.
    /// </summary>
    public interface IAsyncLayout : ILayout
    {
        /// <summary>
        /// Format logging event to string.
        /// </summary>
        /// <param name="loggingEvent">The event to format</param>
        /// <returns>String with the formatted logging event</returns> 
        string Format(LoggingEvent loggingEvent);
    }
}
