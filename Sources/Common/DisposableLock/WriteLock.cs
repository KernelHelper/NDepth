using System.Threading;

namespace NDepth.Common.DisposableLock
{
    /// <summary>
    /// Write lock class enters write lock on construction and performs exit write lock on dispose.
    /// </summary>
    public class WriteLock : DisposableLock
    {
        public WriteLock(ReaderWriterLockSlim slimLock)
            : base(slimLock.ExitWriteLock)
        {
            slimLock.EnterWriteLock();
        }
    }
}
