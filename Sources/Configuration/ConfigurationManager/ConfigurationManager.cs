using System;
using System.Threading;
using NDepth.Configuration.ConfigurationManager.Metadata;

namespace NDepth.Configuration.ConfigurationManager
{
    /// <summary>
    /// Configuration manager provides easy and unified way to manage component/module/cluster configuration.
    /// </summary>
    public class ConfigurationManager
    {
        #region Constructor

        public ConfigurationManager()
        {
            Metadata = new ConfigurationMetaRoot(this);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Initialization flag
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Configuration metadata root node
        /// </summary>
        public ConfigurationMetaRoot Metadata { get; private set; }

        #endregion

        #region Threading syncronization

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>Synchronization object which should be used before access to the configuration</summary>
        public ReaderWriterLockSlim Lock { get { return _lock; } }

        internal void CheckReaderLockOrThrow()
        {
            if (!Lock.IsReadLockHeld)
                throw new InvalidOperationException(Resources.Strings.ReaderLockNotHeld);
        }

        internal void CheckWriterLockOrThrow()
        {
            if (!Lock.IsWriteLockHeld)
                throw new InvalidOperationException(Resources.Strings.WriterLockNotHeld);
        }

        #endregion
    }
}
