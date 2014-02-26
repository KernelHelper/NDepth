using NUnit.Framework;
using NUnit.Framework.ExtensionMethods;
using Rhino.Mocks;
using Rhino.Mocks.Exceptions;

namespace NDepth.Tests.UnitTests.RhinoMocksTests
{
    [TestFixture]
    public class MockTests
    {
        public ISampleClass CreateMock()
        {
            return MockRepository.GenerateMock<ISampleClass>();
        }

        public ISampleClass CreateStrictMock()
        {
            return MockRepository.GenerateStrictMock<ISampleClass>();
        }

        public ISampleClass CreateDynamicMock()
        {
            return MockRepository.GenerateMock<ISampleClass>();
        }

        [Test]
        public void YouCanCheckToSeeIfAPropertyWasSet()
        {
            // Create a mock.
            var mock = CreateMock();

            // Set a property.
            mock.Property = "foo";

            // Check that the property setter was called.
            mock.AssertWasCalled(m => m.Property = "foo");
        }

        [Test]
        public void IfYouSetAPropertyOnAMockThePropertyGetterWillNotReturnTheValueYouSet()
        {
            // Create a mock.
            var mock = CreateMock();

            // Set a property.
            mock.Property = "foo";

            // Check that property returns null.
            mock.Property.Should(Be.Null);
        }

        [Test]
        public void YouCanCheckToSeeIfAPropertyGetterWasCalled()
        {
            // Create a mock.
            var mock = CreateMock();

            // Get a property value.
            mock.Property.Should(Be.Null);

            // Check that property getter was called.
            mock.AssertWasCalled(m => m.Property);
        }

        [Test]
        public void AnotherWayToVerifyExpectationsInsteadOfAssertWasCalled()
        {
            // Create a mock.
            var mock = CreateMock();

            // Here I'm setting up an expectation that a method will be called.
            mock.Expect(m => m.MethodThatReturnsInteger("foo")).Return(5);
            
            // Call the method and check that it returns 5.
            mock.MethodThatReturnsInteger("foo").Should(Be.EqualTo(5));

            // ... and now I'm verifying that the method was called.
            mock.VerifyAllExpectations();
        }

        [Test]
        public void StrictMockWillNotAllowYouToCallUnexpectedMethods()
        {
            // Create a strict mock.
            var mock = CreateStrictMock();

            // Here I'm setting up an expectation that a method will be called.
            mock.Expect(m => m.MethodThatReturnsInteger("foo")).Return(5);

            // Call the method and check that it returns 5.
            mock.MethodThatReturnsInteger("foo").Should(Be.EqualTo(5));

            // Call unexpected method.
            Assert.Throws<ExpectationViolationException>(mock.VoidMethod);

            // ... and now I'm verifying that the method was called and verification will fail.
            Assert.Throws<ExpectationViolationException>(mock.VerifyAllExpectations);
        }

        [Test]
        public void DynamicMockWillAllowYouToCallUnexpectedMethods()
        {
            // Create a dynamic mock.
            var mock = CreateDynamicMock();

            // Here I'm setting up an expectation that a method will be called.
            mock.Expect(m => m.MethodThatReturnsInteger("foo")).Return(5);

            // Call the method and check that it returns 5.
            mock.MethodThatReturnsInteger("foo").Should(Be.EqualTo(5));

            // Call unexpected method.
            mock.VoidMethod();

            // ... and now I'm verifying that the method was called.
            mock.VerifyAllExpectations();
        }
    }
}
