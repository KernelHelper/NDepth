using System;
using Moq;
using NUnit.Framework;
using NUnit.Framework.ExtensionMethods;

// ReSharper disable RedundantAssignment
namespace NDepth.Tests.UnitTests.MoqTests
{
    [TestFixture]
    public class StubTests
    {
        public ISampleClass CreateStub()
        {
            return new Mock<ISampleClass>().SetupAllProperties().Object;
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
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            // Arrange stub method.
            stubClass.Setup(s => s.MethodThatReturnsInteger("foo")).Returns(5);

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
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            // Arrange stub method.
            stubClass.Setup(s => s.MethodThatReturnsInteger(It.IsAny<string>())).Returns(5);

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
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            // It.Is<>() allows us to specify a lambda expression that specifies
            // whether the return value should be used in this case.  Here we're saying
            // that we'll return 5 if the string passed in is longer than 2 characters.
            stubClass.Setup(s => s.MethodThatReturnsInteger(It.Is<string>(arg => arg != null && arg.Length > 2))).Returns(5);

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
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            int[] count = {0};

            // Arrange stub method.
            stubClass.Setup(s => s.MethodThatReturnsInteger(It.IsAny<string>())).Returns(() => count[0]);

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
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            // Here's how you stub an "out" parameter.
            var outi = 10;
            stubClass.Setup(s => s.MethodWithOutParameter(out outi));

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

            // If you call the method with the specified input argument, it will
            // not change the parameter to the value you specified.
            string param = "input";
            stub.MethodWithRefParameter(ref param);
            param.Should(Be.EqualTo("input"));

            // If I call the method with any other input argument, it will
            // not change the parameter to the value you specified.
            param = "some other value";
            stub.MethodWithRefParameter(ref param);
            param.Should(Be.EqualTo("some other value"));
        }

        [Test]
        public void YouCanTellTheStubToThrowAnExceptionWhenAMethodIsCalled()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            // Calling the void method will throw exception.
            stubClass.Setup(s => s.VoidMethod()).Throws<InvalidOperationException>();
            // Calling the method with "foo" as the parameter will throw exception.
            stubClass.Setup(s => s.MethodThatReturnsInteger("foo")).Throws(new InvalidOperationException());

            // Call method that throws exception.
            Assert.Throws<InvalidOperationException>(stub.VoidMethod);
            Assert.Throws<InvalidOperationException>(() => stub.MethodThatReturnsInteger("foo"));
        }

        [Test]
        public void YouCanCheckToSeeIfAMethodWasCalled()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            // Call some method.
            stub.MethodThatReturnsInteger("foo");

            // Check that particular method was called.
            stubClass.Verify(s => s.MethodThatReturnsInteger("foo"));
            stubClass.Verify(s => s.MethodThatReturnsInteger(It.IsAny<string>()));
        }
        
        [Test]
        public void YouCanCheckToSeeIfAMethodWasCalledACertainNumberOfTimes()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            // Call some methods.
            stub.MethodThatReturnsInteger("foo");
            stub.MethodThatReturnsInteger("bar");

            // This will pass.
            stubClass.Verify(s => s.MethodThatReturnsInteger("foo"), Times.Once());

            // Call the method a second time.
            stub.MethodThatReturnsInteger("foo");

            // Now this will fail because we called it a second time.
            Assert.Throws<MockException>(() => stubClass.Verify(s => s.MethodThatReturnsInteger("foo"), Times.Once()));

            // Some other options.
            stubClass.Verify(s => s.MethodThatReturnsInteger("foo"), Times.Exactly(2));
            stubClass.Verify(s => s.MethodThatReturnsInteger("foo"), Times.AtLeastOnce());
        }

        [Test]
        public void YouCanCheckToSeeIfAMethodWasNotCalled()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            // Call some method.
            stub.MethodThatReturnsInteger("foo");

            // Check that other methods were not called.
            Assert.Throws<MockException>(() => stubClass.Verify(s => s.MethodThatReturnsInteger("asdfdsf")));
            Assert.Throws<MockException>(() => stubClass.Verify(s => s.MethodThatReturnsObject(It.IsAny<int>())));
            Assert.Throws<MockException>(() => stubClass.Verify(s => s.VoidMethod()));
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
        public void YouCanUseAssertWasCalledWithPropertiesOnAStub()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            // Set the property.
            stub.Property = "foo";

            // Get the property.
            stub.Property.Should(Be.EqualTo("foo"));

            // Check that the property was set and get.
            stubClass.VerifySet(s => s.Property = "foo");
            stubClass.VerifyGet(s => s.Property);
        }

        [Test]
        public void YouCanTellEventsOnAStubToFire()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            bool eventWasHandled = false;

            // Attach an event handler.
            stub.SomeEvent += (args, e) => eventWasHandled = true;

            // Raise the event.
            stubClass.Raise(s => s.SomeEvent += null, this, EventArgs.Empty);

            // Check that event was handled.
            eventWasHandled.Should(Be.True);
        }

        [Test]
        public void YouCanDoArbitraryStuffWhenAMethodIsCalled()
        {
            // Create a stub.
            ISampleClass stub = CreateStub();
            Mock<ISampleClass> stubClass = Mock.Get(stub);

            bool before = false;
            bool after = false;

            // Prepare stub method.
            stubClass.Setup(s => s.MethodThatReturnsInteger(It.IsAny<string>()))
                .Callback(() => { before = true; })
                .Returns<string>(int.Parse)
                .Callback<string>(s => { after = true; });

            // Check that method returns proper values.
            stub.MethodThatReturnsInteger("3").Should(Be.EqualTo(3));
            stub.MethodThatReturnsInteger("6").Should(Be.EqualTo(6));
            before.Should(Be.True);
            after.Should(Be.True);
        }
    }
}
// ReSharper restore RedundantAssignment
