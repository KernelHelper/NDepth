using System;
using System.IO;
using System.Text;
using log4net.Core;
using log4net.Util;

namespace log4net.Async.Appender
{
    /// <summary>
    /// Asynchronous logging file appender. It logs messages into specified file.
    /// </summary>
    public class AsyncFileAppender : IAsyncAppender, IOptionHandler
    {
        // Current declaration type.
        private readonly static Type DeclaringType = typeof(AsyncFileAppender);

        // Reopen delay in milliseconds.
        private const int ReopenDelay = 1000;

        // File stream for logging.
        private FileStream _fileStream;
        // Last access timestamp.
        private DateTime _timestamp = DateTime.UtcNow;

        #region Constructor

        /// <summary>
        /// Create instance of the asynchronous logging file appender.
        /// </summary>
        public AsyncFileAppender()
        {
            AppendToFile = true;
            ImmediateFlush = true;
        }

        #endregion

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
        /// Gets or sets the path to the file that logging will be written to.
        /// </summary>
        /// <value>
        /// The path to the file that logging will be written to.
        /// </value>
        /// <remarks>
        /// <para>
        /// If the path is relative it is taken as relative from 
        /// the application base directory.
        /// </para>
        /// </remarks>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets a flag that indicates whether the file should be
        /// appended to or overwritten.
        /// </summary>
        /// <value>
        /// Indicates whether the file should be appended to or overwritten.
        /// </value>
        /// <remarks>
        /// <para>
        /// If the value is set to false then the file will be overwritten, if 
        /// it is set to true then the file will be appended to.
        /// </para>
        /// The default value is true.
        /// </remarks>
        public bool AppendToFile { get; set; }

        /// <summary>
        /// Gets or set whether the appender will flush at the end 
        /// of each append operation.
        /// </summary>
        /// <value>
        /// <para>
        /// The default behavior is to flush at the end of each 
        /// append operation.
        /// </para>
        /// <para>
        /// If this option is set to <c>false</c>, then the underlying 
        /// stream can defer persisting the logging event to a later 
        /// time.
        /// </para>
        /// </value>
        /// <remarks>
        /// Avoiding the flush operation at the end of each append results in
        /// a performance gain of 10 to 20 percent. However, there is safety
        /// trade-off involved in skipping flushing. Indeed, when flushing is
        /// skipped, then it is likely that the last few log events will not
        /// be recorded on disk when the application exits. This is a high
        /// price to pay even for a 20% performance gain.
        /// </remarks>
        public bool ImmediateFlush { get; set; }

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
            // If file stream is not ready, then try to reopen it after a small delay.
            if (_fileStream == null)
            {
                // Reopen file stream or skip the logging message.
                if ((DateTime.UtcNow - _timestamp).TotalMilliseconds > ReopenDelay)
                    OpenFile();
            }

            // If file steam is still not opened, then skip the current logging message.
            if (_fileStream == null)
                return;

            try
            {
                _fileStream.Write(Encoding.UTF8.GetBytes(loggingMessage), 0, Encoding.UTF8.GetByteCount(loggingMessage));
                if (ImmediateFlush)
                    _fileStream.Flush();
            }
            catch (Exception ex)
            {
                LogLog.Error(DeclaringType, string.Format(Resources.Strings.WriteFileError, File), ex);
                CloseFile();
            }
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
            throw new InvalidOperationException(Resources.Strings.AsyncFileAppenderInvalidOperation);
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
            // Close the current file.
            CloseFile();
        }

        #endregion

        #region IOptionHandler

        /// <summary>
        /// Activate the options that were previously set with calls to properties.
        /// </summary>
        virtual public void ActivateOptions()
        {
            // Close the current file.
            CloseFile();

            // Check the file name.
            if (string.IsNullOrEmpty(File))
                throw new LogException(Resources.Strings.FileNameIsNullOrEmpty);

            // Close new file.
            OpenFile();
        }

        #endregion

        #region Utility methods
        
        private void OpenFile()
        {
            try
            {
                _fileStream = new FileStream(File, AppendToFile ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read, 64 * 1024);
            }
            catch (Exception ex)
            {
                LogLog.Error(DeclaringType, string.Format(Resources.Strings.OpenFileError, File), ex);
                _timestamp = DateTime.UtcNow;
                _fileStream = null;
            }            
        }

        private void CloseFile()
        {
            try
            {
                if (_fileStream != null)
                {
                    FileStream temp = _fileStream;

                    _timestamp = DateTime.UtcNow;
                    _fileStream = null;

                    temp.Flush();
                    temp.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogLog.Error(DeclaringType, string.Format(Resources.Strings.CloseFileError, File), ex);
                _timestamp = DateTime.UtcNow;
                _fileStream = null;
            }
        }

        #endregion
    }
}
