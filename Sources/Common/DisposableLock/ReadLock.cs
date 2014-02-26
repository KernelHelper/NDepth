using System.Threading;

namespace NDepth.Common.DisposableLock
{
    /// <summary>
    /// Read lock class enters read lock on construction and performs exit read lock on dispose.
    /// </summary>
    public class ReadLock : DisposableLock
    {
        public ReadLock(ReaderWriterLockSlim slimLock)
            : base(slimLock.ExitReadLock)
        {
            slimLock.EnterReadLock();
        }
    }
}
