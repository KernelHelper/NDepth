using System;
using System.Collections.Generic;

namespace NDepth.Tests.UnitTests.RhinoMocksTests
{
    public interface ISampleClass
    {
        string Property { get; set; }

        void VoidMethod();
        int MethodThatReturnsInteger(string s);
        object MethodThatReturnsObject(int i);
        void MethodWithOutParameter(out int i);
        void MethodWithRefParameter(ref string i);

        event EventHandler SomeEvent;
    }

    public class SampleClass
    {
        private string _nonVirtualProperty;
        public string NonVirtualProperty
        {
            get { return _nonVirtualProperty; }
            set
            {
                _nonVirtualProperty = value;
                NonVirtualPropertyWasSet = true;
            }
        }

        private string _virtualProperty;
        public virtual string VirtualProperty
        {
            get { return _virtualProperty; }
            set
            {
                _virtualProperty = value;
                VirtualPropertyWasSet = true;
            }
        }

        public string SetByConstructor { get; private set; }
        public bool NonVirtualPropertyWasSet { get; set; }
        public bool VirtualPropertyWasSet { get; set; }
        public bool VoidMethodWasCalled { get; set; }
        public bool VirtualMethodWasCalled { get; set; }
        public bool NonVirtualMethodWasCalled { get; set; }

        public event EventHandler SomeEvent;
        public virtual event EventHandler SomeVirtualEvent;

        public SampleClass()
        {

        }

        public SampleClass(string value)
        {
            SetByConstructor = value;
        }

        public void VoidMethod()
        {
            VoidMethodWasCalled = true;
        }

        public virtual int VirtualMethod(string s)
        {
            VirtualMethodWasCalled = true;
            return s.Length;
        }

        public IList<int> NonVirtualMethod(int i)
        {
            NonVirtualMethodWasCalled = true;
            return new List<int> { i };
        }

        public void FireSomeEvent()
        {
            if (SomeEvent != null)
                SomeEvent(this, EventArgs.Empty);
        }

        public void FireSomeVirtualEvent()
        {
            if (SomeVirtualEvent != null)
                SomeVirtualEvent(this, EventArgs.Empty);
        }
    }
}
