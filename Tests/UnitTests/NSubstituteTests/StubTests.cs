using System;
using NSubstitute;
using NSubstitute.Exceptions;
using NUnit.Framework;
using NUnit.Framework.ExtensionMethods;

namespace NDepth.Tests.UnitTests.NSubstituteTests
{
    [TestFixture]
    public class StubTests
    {
        public ISampleClass CreateStub()
        {
            return Substitute.For<ISampleClass>();
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
            stub.MethodThatReturnsInteger("foo").Returns(5);

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
            stub.MethodThatReturnsInteger(Arg.Any<string>()).Returns(5);

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

            // Arg.Is<>() allows us to specify a lambda expression that specifies
            // whether the return value should be used in this case.  Here we're saying
            // that we'll return 5 if the string passed in is longer than 2 characters.
            stub.MethodThatReturnsInteger(Arg.Is<string>(arg => arg != null && arg.Length > 2)).Returns(5);

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
            stub.MethodThatReturnsInteger(Arg.Any<string>()).Returns(s => count[0]);

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

            // Here's how you stub an "out" parameter.
            int outi;
            stub.WhenForAnyArgs(s => s.MethodWithOutParameter(out outi)).Do(x => x[0] = 10);

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

            // Here's how you stub an "ref" parameter.
            string refi = "input";
            stub.When(s => s.MethodWithRefParameter(ref refi)).Do(x => x[0] = "output");

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
            stub.When(s => s.VoidMethod()).Do(x => { throw new InvalidOperationException(); });
            // Calling the method with "foo" as the parameter will throw exception.
            stub.MethodThatReturnsInteger("foo").Returns(x => { throw new InvalidOperationException(); });

            // Call method that throws exception.
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
            stub.Received().MethodThatReturnsInteger("foo");
            stub.ReceivedWithAnyArgs().MethodThatReturnsInteger(Arg.Any<string>());
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
            stub.Received(1).MethodThatReturnsInteger("foo");

            // Call the method a second time.
            stub.MethodThatReturnsInteger("foo");

            // Now this will fail because we called it a second time.
            Assert.Throws<ReceivedCallsException>(() => stub.Received(1).MethodThatReturnsInteger("foo"));

            // Some other options.
            stub.Received(2).MethodThatReturnsInteger("foo");
        }

        [Test]
        public void YouCanCheckToSeeIfAMethodWasNotCalled()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Call some method.
            stub.MethodThatReturnsInteger("foo");

            // Check that other methods were not called.
            stub.DidNotReceive().MethodThatReturnsInteger("asdfdsf");
            stub.DidNotReceiveWithAnyArgs().MethodThatReturnsObject(Arg.Any<int>());
            stub.DidNotReceive().VoidMethod();
        }

        [Test]
        public void YouCanClearReceivedCalls()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Call some method.
            stub.MethodThatReturnsInteger("foo");

            // Check that method was called.
            stub.Received().MethodThatReturnsInteger("foo");

            stub.ClearReceivedCalls();

            // Check that method was not called.
            stub.DidNotReceiveWithAnyArgs().MethodThatReturnsInteger(Arg.Any<string>());
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
        public void YouCanSetAPropertyWithASequenceOfReturnValues()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Set the property.
            stub.Property.Returns("foo", "bar");

            // Check the property.
            stub.Property.Should(Be.EqualTo("foo"));
            stub.Property.Should(Be.EqualTo("bar"));
        }

        [Test]
        public void YouCanUseAssertWasCalledWithPropertiesOnAStub()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Set the property.
            stub.Property = "foo";

            // Get the property.
            stub.Property.Should(Be.EqualTo("foo"));

            // Check that the property was set and get.
            stub.Received().Property = "foo";
            stub.Received().Property.Should(Be.Null);
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
            stub.SomeEvent += Raise.EventWith(this, EventArgs.Empty);

            // Check that event was handled.
            eventWasHandled.Should(Be.True);
        }

        [Test]
        public void YouCanDoArbitraryStuffWhenAMethodIsCalled()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();

            // Prepare stub method.
            stub.MethodThatReturnsInteger(Arg.Any<string>()).Returns(s => int.Parse(s.Arg<string>()));

            // Check that method returns proper values.
            stub.MethodThatReturnsInteger("3").Should(Be.EqualTo(3));
            stub.MethodThatReturnsInteger("6").Should(Be.EqualTo(6));
        }
    }
}
