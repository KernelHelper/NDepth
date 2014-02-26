using System;
using log4net.Appender;
using log4net.Core;
using log4net.Async.Appender;
using log4net.Async.Layout;
using log4net.Async.Strategy;

namespace log4net.Async
{
    /// <summary>
    /// Asynchronous logging appender. It uses provided layout to format logging messages into strings 
    /// and delegates them into some asynchronous strategy, which stores logging messages into provided
    /// target appender.
    /// </summary>
    public class AsyncAppender : IAppender, IOptionHandler
    {
        #region Constructor

        /// <summary>
        /// Create instance of the asynchronous forwarding appender.
        /// </summary>
        public AsyncAppender()
        {
            Layout = new AsyncNullLayout();
            Strategy = new AsyncStrategyHotSwap();
            Appender = new AsyncNullAppender();
            ErrorHandler = new AsyncErrorHandler(this);
        }

        #endregion

        #region Finalizer

        /// <summary>
        /// Finalize asynchronous forwarding appender.
        /// </summary>
        ~AsyncAppender()
        {
            // Close the current asynchronous processing.
            Close();
        }

        #endregion

        #region Options

        /// <summary>
        /// Gets or sets the name of this appender.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the asynchronous layout for this appender.
        /// </summary>
        public IAsyncLayout Layout  { get; set; }

        /// <summary>
        /// Gets or sets the asynchronous strategy for this appender.
        /// </summary>
        public IAsyncStrategy Strategy { get; set; }

        /// <summary>
        /// Gets or sets the asynchronous target appender for this appender.
        /// </summary>
        public IAsyncAppender Appender { get; set; }

        /// <summary>
        /// Gets or sets the error handler for this appender.
        /// </summary>
        public IErrorHandler ErrorHandler { get; set; }

        #endregion

        #region Asynchronous processing

        private IAsyncLayout _asyncLayout;
        private IAsyncStrategy _asyncStrategy;
        private IAsyncAppender _asyncAppender;

        private void AppendInternal(LoggingEvent loggingEvent)
        {
            // Add the formatted logging message to the asynchronous strategy.
            _asyncStrategy.AddItem(_asyncLayout.Format(loggingEvent));
        }

        #endregion

        #region IAppender

        /// <summary>
        /// Log the logging event in Appender specific way.
        /// </summary>
        virtual public void DoAppend(LoggingEvent loggingEvent)
        {
            // Skip empty logging events.
            if (loggingEvent == null)
                return;

            AppendInternal(loggingEvent);
        }

        /// <summary>
        /// Closes the appender and releases resources.
        /// </summary>
        virtual public void Close()
        {
            // Stop the previous asynchronous processing.
            if (_asyncStrategy != null)
            {
                _asyncStrategy.StopProcessing();
                _asyncStrategy.Dispose();
                _asyncStrategy = null;
            }

            // Close the asynchronous target appender.
            if (_asyncAppender != null)
            {
                _asyncAppender.Close();
                _asyncAppender = null;
            }
        }

        #endregion

        #region IErrorHandler

        private class AsyncErrorHandler : IErrorHandler
        {
            private readonly AsyncAppender _asyncLogger;

            public AsyncErrorHandler(AsyncAppender asyncLogger)
            {
                _asyncLogger = asyncLogger;
            }

            public void Error(string message, Exception e, ErrorCode errorCode)
            {
                _asyncLogger.AppendInternal(new LoggingEvent(typeof(AsyncAppender), null, _asyncLogger.Name, Level.Error, message, e));
            }

            public void Error(string message, Exception e)
            {
                _asyncLogger.AppendInternal(new LoggingEvent(typeof(AsyncAppender), null, _asyncLogger.Name, Level.Error, message, e));
            }

            public void Error(string message)
            {
                _asyncLogger.AppendInternal(new LoggingEvent(typeof(AsyncAppender), null, _asyncLogger.Name, Level.Error, message, null));
            }
        }

        #endregion

        #region IOptionHandler

        /// <summary>
        /// Activate the options that were previously set with calls to properties.
        /// </summary>
        virtual public void ActivateOptions()
        {
            // Close the current asynchronous processing.
            Close();

            // Check the asynchronous layout.
            if (Layout == null)
                throw new LogException(Resources.Strings.AsyncLayoutIsNull);

            // Check the asynchronous strategy.
            if (Strategy == null)
                throw new LogException(Resources.Strings.AsyncStrategyIsNull);

            // Check the asynchronous target appender.
            if (Appender == null)
                throw new LogException(Resources.Strings.AsyncAppenderIsNull);

            _asyncLayout = Layout;
            _asyncStrategy = Strategy;
            _asyncAppender = Appender;

            // Start new asynchronous processing.
            _asyncStrategy.HandleItem += loggingMessage => Appender.DoAppend(loggingMessage);
            _asyncStrategy.HandleOverflow += bufferLimit => ErrorHandler.Error(string.Format(Resources.Strings.AsyncQueueOverflow, bufferLimit));
            _asyncStrategy.StartProcessing();
        }

        #endregion
    }
}
