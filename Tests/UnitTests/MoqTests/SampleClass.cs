using System;

namespace NDepth.Tests.UnitTests.MoqTests
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
}
