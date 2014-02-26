using Moq;
using NUnit.Framework;
using NUnit.Framework.ExtensionMethods;

namespace NDepth.Tests.UnitTests.MoqTests
{
    [TestFixture]
    public class MockTests
    {
        public ISampleClass CreateMock()
        {
            return new Mock<ISampleClass>().Object;
        }

        [Test]
        public void YouCanCheckToSeeIfAPropertyWasSet()
        {
            // Create a mock.
            var mock = CreateMock();
            Mock<ISampleClass> mockClass = Mock.Get(mock);

            // Set a property.
            mock.Property = "foo";

            // Check that the property setter was called.
            mockClass.VerifySet(m => m.Property = "foo");
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
            Mock<ISampleClass> mockClass = Mock.Get(mock);

            // Get a property value.
            mock.Property.Should(Be.Null);

            // Check that property getter was called.
            mockClass.VerifyGet(m => m.Property);
        }

        [Test]
        public void AnotherWayToVerifyExpectationsInsteadOfAssertWasCalled()
        {
            // Create a mock.
            var mock = CreateMock();
            Mock<ISampleClass> mockClass = Mock.Get(mock);

            // Here I'm setting up an expectation that a method will be called.
            mockClass.Setup(m => m.MethodThatReturnsInteger("foo")).Returns(5);

            // Call the method and check that it returns 5.
            mock.MethodThatReturnsInteger("foo").Should(Be.EqualTo(5));

            // ... and now I'm verifying that the method was called.
            mockClass.VerifyAll();
        }
    }
}
