using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NDepth.Examples.Common.DisposeAndFinalizeExample
{
    // Design pattern for a base class.
    public abstract class Base : IDisposable
    {
        private readonly string _instanceName;
        private readonly List<object> _trackingList;

        protected Base(string instanceName, List<object> tracking)
        {
            _instanceName = instanceName;
            _trackingList = tracking;
            _trackingList.Add(this);
        }

        public string InstanceName { get { return _instanceName; } }

        #region IDisposable implementation

        // Disposed flag.
        private bool _disposed;

        // Implement IDisposable.
        public void Dispose()
        {
            Console.WriteLine("\n[{0}].Base.Dispose()", _instanceName);

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposingManagedResources)
        {
            if (!_disposed)
            {
                Console.WriteLine("[{0}].Base.Dispose({1})", _instanceName, disposingManagedResources);

                if (disposingManagedResources)
                {
                    // Dispose managed resources here...
                    _trackingList.Remove(this);

                    Console.WriteLine("[{0}] Removed from tracking list: {1:x16}", _instanceName, GetHashCode());
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~Base()
        {
            Console.WriteLine("\n[{0}].Base.Finalize()", _instanceName);

            Dispose(false);
        }

        #endregion
    }

    // Design pattern for a derived class.
    public class Derived : Base
    {
        private IntPtr _unmanagedResource;

        public Derived(string instanceName, List<object> tracking) : 
            base(instanceName, tracking)
        {
            // Save the instance name as an unmanaged resource.
            _unmanagedResource = Marshal.StringToCoTaskMemAuto(instanceName);
        }

        #region IDisposable implementation

        // Disposed flag.
        private bool _disposed;

        protected override void Dispose(bool disposingManagedResources)
        {
            if (!_disposed)
            {
                Console.WriteLine("[{0}].Derived.Dispose({1})", InstanceName, disposingManagedResources);

                if (disposingManagedResources)
                {
                    // Dispose managed resources here...
                }

                // Dispose unmanaged resources here...
                if (_unmanagedResource != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(_unmanagedResource);

                    Console.WriteLine("[{0}] Unmanaged memory freed at {1:x16}", InstanceName, _unmanagedResource.ToInt64());

                    _unmanagedResource = IntPtr.Zero;
                }

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }

            // Call Dispose in the base class.
            base.Dispose(disposingManagedResources);
        }

        // The derived class does not have a Finalize method
        // or a Dispose method without parameters because it inherits
        // them from the base class.

        #endregion
    }

    class Program
    {
        static void Main()
        {
            var tracking = new List<object>();

            // Dispose is not called, Finalize will be called later.
            using (null)
            {
                Console.WriteLine("\nDisposal Scenario: #1\n");
                var d1 = new Derived("d1", tracking);
                Console.WriteLine("    Reference Object: {0}, {1:x16}", d1.InstanceName, d1.GetHashCode());
            }

            // Dispose is implicitly called in the scope of the using statement.
            using (var d2 = new Derived("d2", tracking))
            {
                Console.WriteLine("\nDisposal Scenario: #2\n");
                Console.WriteLine("    Reference Object: {0}, {1:x16}", d2.InstanceName, d2.GetHashCode());
            }

            // Dispose is explicitly called.
            using (null)
            {
                Console.WriteLine("\nDisposal Scenario: #3\n");
                var d3 = new Derived("d3", tracking);
                Console.WriteLine("    Reference Object: {0}, {1:x16}", d3.InstanceName, d3.GetHashCode());
                d3.Dispose();
            }

            // Again, Dispose is not called, Finalize will be called later.
            using (null)
            {
                Console.WriteLine("\nDisposal Scenario: #4\n");
                var d4 = new Derived("d4", tracking);
                Console.WriteLine("    Reference Object: {0}, {1:x16}", d4.InstanceName, d4.GetHashCode());
            }

            // List the objects remaining to dispose.
            Console.WriteLine("\nObjects remaining to dispose = {0:d}", tracking.Count);
            foreach (Derived dd in tracking)
            {
                Console.WriteLine("    Reference Object: {0}, {1:x16}", dd.InstanceName, dd.GetHashCode());
            }

            // Queued finalizers will be executed when Main() goes out of scope.
            Console.WriteLine("\nDequeueing finalizers...");
        }
    }
}
