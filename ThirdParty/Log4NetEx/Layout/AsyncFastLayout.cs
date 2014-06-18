using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using NDepth.Common.Interfaces;
using log4net.Core;

namespace log4net.Async.Layout
{
    /// <summary>
    /// Asynchronous logging fast layout. It returns a string in the following format for every logging message -
    /// "%date %time [%thread] %level - %logger - %message%newline"
    /// </summary>
    public class AsyncFastLayout : IAsyncLayout, ICloneable<AsyncFastLayout>
    {
        [ThreadStatic] private static AsyncFastLayout _instance;
        private StringBuilder _stringBuilder;
        private StringBuilder _stringBuilderDate;
        private StringBuilder _stringBuilderTime;
        private DateTime _cachedTimestamp;
        private string _cachedDate;
        private string _cachedTime;
        private string _cachedThread;

        #region IAsyncLayout

        /// <summary>
        /// The content type output by this layout. 
        /// </summary>
        public string ContentType
        {
            get { return "text/plain"; }
        }

        /// <summary>
        /// The header for the layout format.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// The footer for the layout format.
        /// </summary>
        public string Footer { get; set; }

        /// <summary>
        /// Flag indicating if this layout shows timestamp in UTC form.
        /// </summary>
        public bool IsUTC { get; set; }

        /// <summary>
        /// Flag indicating if this layout does not show date.
        /// </summary>
        public bool IgnoresDate { get; set; }

        /// <summary>
        /// Flag indicating if this layout does not show time.
        /// </summary>
        public bool IgnoresTime { get; set; }

        /// <summary>
        /// Flag indicating if this layout does not show thread.
        /// </summary>
        public bool IgnoresThread { get; set; }

        /// <summary>
        /// Flag indicating if this layout does not show logging level.
        /// </summary>
        public bool IgnoresLevel { get; set; }

        /// <summary>
        /// Flag indicating if this layout does not show logger name.
        /// </summary>
        public bool IgnoresName { get; set; }

        /// <summary>
        /// Flag indicating if this layout does not show logging message.
        /// </summary>
        public bool IgnoresMessage { get; set; }

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
            // Create thread static instance if necessary.
            if (_instance == null)
            {
                string thread = '[' + Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture) + ']';

                _instance = Clone();
                _instance._cachedThread = thread.PadLeft(6);
                _instance._stringBuilder = new StringBuilder(1024);
                _instance._stringBuilderDate = new StringBuilder(10);
                _instance._stringBuilderTime = new StringBuilder(12);
            }

            return _instance.FormatInternal(loggingEvent);
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
        /// event to be formatted as a <see cref="String"/> then the following
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
            writer.Write(Format(loggingEvent));
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Clone the current instance.
        /// </summary>
        /// <returns>Clone of the current instance</returns> 
        public AsyncFastLayout Clone()
        {
            return new AsyncFastLayout
            {
                Header = Header,
                Footer = Footer,
                IsUTC = IsUTC,
                IgnoresDate = IgnoresDate,
                IgnoresTime = IgnoresTime,
                IgnoresThread = IgnoresThread,
                IgnoresLevel = IgnoresLevel,
                IgnoresName = IgnoresName,
                IgnoresMessage = IgnoresMessage,
                IgnoresException = IgnoresException
            };
        }

        #endregion

        #region Utility methods

        private string FormatInternal(LoggingEvent loggingEvent)
        {
            // Clear string builder.
            _stringBuilder.Clear();

            // Prepare date & time.
            if (!IgnoresDate || !IgnoresTime)
            {
                DateTime timestamp = IsUTC ? loggingEvent.TimeStamp.ToUniversalTime() : loggingEvent.TimeStamp;
                if (timestamp != _cachedTimestamp)
                {
                    // Format date.
                    if (!IgnoresDate)
                        _cachedDate = FormatDate(timestamp);

                    // Format time.
                    if (!IgnoresTime)
                        _cachedTime = FormatTime(timestamp);

                    // Cache new timestamp.
                    _cachedTimestamp = timestamp;
                }
            }

            // Format date.
            if (!IgnoresDate)
            {
                _stringBuilder.Append(_cachedDate);
                _stringBuilder.Append(' ');
            }

            // Format time.
            if (!IgnoresTime)
            {
                _stringBuilder.Append(_cachedTime);
                _stringBuilder.Append(' ');
            }

            // Format thread.
            if (!IgnoresThread)
            {
                _stringBuilder.Append(_cachedThread);
                _stringBuilder.Append(' ');
            }

            // Format logging level.
            if (!IgnoresLevel)
            {
                _stringBuilder.Append(FormatLevel(loggingEvent.Level));
                _stringBuilder.Append(' ');
            }

            // Format logger name.
            if (!IgnoresName)
            {
                _stringBuilder.Append("- ");
                _stringBuilder.Append(loggingEvent.LoggerName);
                _stringBuilder.Append(' ');
            }

            // Format logging message.
            if (!IgnoresMessage)
            {
                _stringBuilder.Append("- ");
                _stringBuilder.Append(loggingEvent.MessageObject);
            }

            _stringBuilder.AppendLine();

            // Format logging exception.
            if (!IgnoresException && (loggingEvent.ExceptionObject != null))
            {
                _stringBuilder.Append(loggingEvent.ExceptionObject);
                _stringBuilder.AppendLine();
            }

            // Return formatted string.
            return _stringBuilder.ToString();
        }

        private string FormatDate(DateTime timestamp)
        {
            // Clear string builder.
            _stringBuilderDate.Clear();

            FormatInt9999(_stringBuilderDate, timestamp.Year);
            _stringBuilderDate.Append('-');
            FormatInt99(_stringBuilderDate, timestamp.Month);
            _stringBuilderDate.Append('-');
            FormatInt99(_stringBuilderDate, timestamp.Day);

            // Return formatted string.
            return _stringBuilderDate.ToString();
        }

        private string FormatTime(DateTime timestamp)
        {
            // Clear string builder.
            _stringBuilderTime.Clear();

            FormatInt99(_stringBuilderTime, timestamp.Hour);
            _stringBuilderTime.Append(':');
            FormatInt99(_stringBuilderTime, timestamp.Minute);
            _stringBuilderTime.Append(':');
            FormatInt99(_stringBuilderTime, timestamp.Second);
            _stringBuilderTime.Append(',');
            FormatInt999(_stringBuilderTime, timestamp.Millisecond);

            // Return formatted string.
            return _stringBuilderTime.ToString();
        }

        private string FormatLevel(Level level)
        {
            switch (level.Value)
            {
                case 20000:
                    return "TRACE";
                case 30000:
                    return "DEBUG";
                case 40000:
                    return " INFO";
                case 60000:
                    return " WARN";
                case 70000:
                    return "ERROR";
                case 110000:
                    return "FATAL";
                default:
                    return "  ???";
            }
        }

        private void FormatInt99(StringBuilder builder, int value)
        {
            value %= 100;
            int part1 = value / 10;
            int part2 = value % 10;
            builder.Append((char)('0' + part1));
            builder.Append((char)('0' + part2));
        }

        private void FormatInt999(StringBuilder builder, int value)
        {
            value %= 1000;
            int part1 = value / 100;
            int part2 = value % 100;
            int part21 = part2 / 10;
            int part22 = part2 % 10;
            builder.Append((char)('0' + part1));
            builder.Append((char)('0' + part21));
            builder.Append((char)('0' + part22));
        }

        private void FormatInt9999(StringBuilder builder, int value)
        {
            value %= 10000;
            int part1 = value / 1000;
            int part2 = value % 1000;
            int part21 = part2 / 100;
            int part22 = part2 % 100;
            int part31 = part22 / 10;
            int part32 = part22 % 10;
            builder.Append((char)('0' + part1));
            builder.Append((char)('0' + part21));
            builder.Append((char)('0' + part31));
            builder.Append((char)('0' + part32));
        }

        #endregion
    }
}
