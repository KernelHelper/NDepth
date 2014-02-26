using System;

namespace NDepth.Common.DisposableLock
{
    /// <summary>
    /// Disposable lock class performs exit action on dispose operation.
    /// </summary>
    public class DisposableLock : IDisposable
    {
        private readonly Action _exitLock;

        public DisposableLock(Action exitLock)
        {
            _exitLock = exitLock;
        }

        public void Dispose()
        {
            _exitLock();
        }
    }
}
