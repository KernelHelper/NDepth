using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.ExtensionMethods;
using Rhino.Mocks;
using Rhino.Mocks.Exceptions;

namespace NDepth.Tests.UnitTests.RhinoMocksTests
{
    [TestFixture]
    public class StubTests
    {
        public ISampleClass CreateStub()
        {
            return MockRepository.GenerateStub<ISampleClass>();
        }

        [Test]
        public void YouCanCreateStubByCallingMockRepositoryGenerateStub()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Ensure that its string property is null or empty.
            stub.Property.Should(Be.Null.Or.Empty);

            int outparam;
            string refparam = "test";

            // Call some methods.
            stub.VoidMethod();
            stub.MethodThatReturnsInteger("test").Should(Be.EqualTo(0));
            stub.MethodThatReturnsObject(0).Should(Be.Null);
            stub.MethodWithOutParameter(out outparam);
            outparam.Should(Be.EqualTo(0));
            stub.MethodWithRefParameter(ref refparam);
            refparam.Should(Be.EqualTo("test"));
        }

        [Test]
        public void CallingVoidMethodsWillDoNothing()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Call void method.
            stub.VoidMethod();
        }

        [Test]
        public void CallingMethodsThatReturnValueTypesWillReturnTheDefaultValueForThatType()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Call method that returns value type.
            stub.MethodThatReturnsInteger("foo").Should(Be.EqualTo(0));
        }

        [Test]
        public void CallingMethodsThatReturnReferenceTypesWillReturnNull()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Call method that returns reference type.
            stub.MethodThatReturnsObject(1).Should(Be.Null);
        }

        [Test]
        public void YouCanTellTheStubWhatValueToReturnWhenIsMethodIsCalledWithSpecificArguments()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Arrange stub method.
            stub.Stub(s => s.MethodThatReturnsInteger("foo")).Return(5);

            // Calling the method with "foo" as the parameter will return 5.
            stub.MethodThatReturnsInteger("foo").Should(Be.EqualTo(5));

            // Calling the method with anything other than "foo" as the parameter will return the default value.
            stub.MethodThatReturnsInteger("bar").Should(Be.EqualTo(0));
        }

        [Test]
        public void YouCanTellTheStubWhatValueToReturnWhenIsMethodIsCalledWithAnyArgument()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Arrange stub method.
            stub.Stub(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything)).Return(5);

            // Now it doesn't matter what the parameter is, we'll always get 5.
            stub.MethodThatReturnsInteger("foo").Should(Be.EqualTo(5));
            stub.MethodThatReturnsInteger("bar").Should(Be.EqualTo(5));
            stub.MethodThatReturnsInteger(null).Should(Be.EqualTo(5));
        }

        [Test]
        public void YouCanGetFancyWithParametersInStubs()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Arg<>.Matches() allows us to specify a lambda expression that specifies
            // whether the return value should be used in this case.  Here we're saying
            // that we'll return 5 if the string passed in is longer than 2 characters.
            stub.Stub(s => s.MethodThatReturnsInteger(Arg<string>.Matches(arg => arg != null && arg.Length > 2))).Return(5);

            // Call method with different parameters.
            stub.MethodThatReturnsInteger("fooo").Should(Be.EqualTo(5));
            stub.MethodThatReturnsInteger("foo").Should(Be.EqualTo(5));
            stub.MethodThatReturnsInteger("fo").Should(Be.EqualTo(0));
            stub.MethodThatReturnsInteger("f").Should(Be.EqualTo(0));
            stub.MethodThatReturnsInteger(null).Should(Be.EqualTo(0));
        }

        [Test]
        public void YouCanTellTheStubToReturnValuesInALazyWay()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            int[] count = { 0 };

            // Arrange stub method.
            stub.Stub(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything))
                .Return(0)                
                .WhenCalled(method =>
                {
                    method.ReturnValue = count[0];
                });

            // Calling the method will return 1.
            count[0]++;
            stub.MethodThatReturnsInteger("foo").Should(Be.EqualTo(1));

            // Calling the method will return 2.
            count[0]++;
            stub.MethodThatReturnsInteger("bar").Should(Be.EqualTo(2));
        }

        [Test]
        public void HandlingOutParametersInStubs()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Here's how you stub an "out" parameter.  The "Dummy" part is 
            // just to satisfy the compiler.
            stub.Stub(s => s.MethodWithOutParameter(out Arg<int>.Out(10).Dummy));

            // Call method with out parameters.
            int i;
            stub.MethodWithOutParameter(out i);
            i.Should(Be.EqualTo(10));
        }

        [Test]
        public void HandlingRefParametersInStubs()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Here's how you stub an "ref" parameter.  The "Dummy" part is 
            // just to satisfy the compiler.  (Note: Is.Equal() is part of
            // the Rhino.Mocks.Contraints namespace, there is also an 
            // Is.EqualTo() in NUnit... you want the Rhino Mocks one.)
            stub.Stub(s => s.MethodWithRefParameter(ref Arg<string>.Ref(Rhino.Mocks.Constraints.Is.Equal("input"), "output").Dummy));

            // If you call the method with the specified input argument, it will
            // change the parameter to the value you specified.
            string param = "input";
            stub.MethodWithRefParameter(ref param);
            param.Should(Be.EqualTo("output"));

            // If I call the method with any other input argument, it won't
            // change the value.
            param = "some other value";
            stub.MethodWithRefParameter(ref param);
            param.Should(Be.EqualTo("some other value"));
        }

        [Test]
        public void YouCanTellTheStubToThrowAnExceptionWhenAMethodIsCalled()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Calling the void method will throw exception.
            stub.Stub(s => s.VoidMethod()).Throw(new InvalidOperationException());
            // Calling the method with "foo" as the parameter will throw exception.
            stub.Stub(s => s.MethodThatReturnsInteger("foo")).Throw(new InvalidOperationException());

            // Call methods that throw exception.
            Assert.Throws<InvalidOperationException>(stub.VoidMethod);
            Assert.Throws<InvalidOperationException>(() => stub.MethodThatReturnsInteger("foo"));
        }

        [Test]
        public void YouCanCheckToSeeIfAMethodWasCalled()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Call some method.
            stub.MethodThatReturnsInteger("foo");

            // Check that particular method was called.
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"));
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything));
        }

        [Test]
        public void YouCanCheckToSeeIfAMethodWasCalledACertainNumberOfTimes()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Call some methods.
            stub.MethodThatReturnsInteger("foo");
            stub.MethodThatReturnsInteger("bar");

            // This will pass.
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"), o => o.Repeat.Once());

            // Call the method a second time.
            stub.MethodThatReturnsInteger("foo");

            // Now this will fail because we called it a second time.
            Assert.Throws<ExpectationViolationException>(() => stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"), o => o.Repeat.Once()));

            // Some other options.
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"), o => o.Repeat.Times(2));
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"), o => o.Repeat.AtLeastOnce());
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"), o => o.Repeat.Twice());
        }

        [Test]
        public void YouCanCheckToSeeIfAMethodWasNotCalled()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Call some method.
            stub.MethodThatReturnsInteger("foo");

            // Check that other methods were not called.
            stub.AssertWasNotCalled(s => s.MethodThatReturnsInteger("asdfdsf"));
            stub.AssertWasNotCalled(s => s.MethodThatReturnsObject(Arg<int>.Is.Anything));
            stub.AssertWasNotCalled(s => s.VoidMethod());
        }

        [Test]
        public void YouCanGetTheArgumentsOfCallsToAMethod()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Call some methods.
            stub.MethodThatReturnsInteger("foo");
            stub.MethodThatReturnsInteger("bar");

            // GetArgumentsForCallsMadeOn() returns a list of arrays that contain
            // the parameter values for each call to the method.
            IList<object[]> argsPerCall = stub.GetArgumentsForCallsMadeOn(s => s.MethodThatReturnsInteger(null));
            argsPerCall[0][0].Should(Be.EqualTo("foo"));
            argsPerCall[1][0].Should(Be.EqualTo("bar"));
        }

        [Test]
        public void IfYouSetAPropertyTheGetterWillReturnTheValue()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Set the property.
            stub.Property = "foo";

            // Check the property.
            stub.Property.Should(Be.EqualTo("foo"));
        }

        [Test]
        public void YouCannotUseAssertWasCalledWithPropertiesOnAStub()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // But why would you need to?  You can just get the value 
            // directly from the property.
            stub.Property = "foo";

            // Don't do this
            // stub.AssertWasCalled(s => s.Property);

            // Just do this.
            stub.Property.Should(Be.EqualTo("foo"));
        }

        [Test]
        public void YouCanTellEventsOnAStubToFire()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            bool eventWasHandled = false;

            // Attach an event handler.
            stub.SomeEvent += (args, e) => eventWasHandled = true;

            // Raise the event.
            stub.Raise(s => s.SomeEvent += null, this, EventArgs.Empty);

            // Check that event was handled.
            eventWasHandled.Should(Be.True);
        }

        [Test]
        public void YouCanDoArbitraryStuffWhenAMethodIsCalled()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Prepare stub method.
            stub.Stub(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything))
                .Return(0)
                .WhenCalled(method =>
                {
                    var param = (string)method.Arguments[0];
                    method.ReturnValue = int.Parse(param);
                });

            // Check that method returns proper values.
            stub.MethodThatReturnsInteger("3").Should(Be.EqualTo(3));
            stub.MethodThatReturnsInteger("6").Should(Be.EqualTo(6));
        }
    }
}
