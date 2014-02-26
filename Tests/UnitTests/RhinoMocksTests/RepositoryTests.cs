using NUnit.Framework;
using NUnit.Framework.ExtensionMethods;
using Rhino.Mocks;

namespace NDepth.Tests.UnitTests.RhinoMocksTests
{
    [TestFixture]
    public class RepositoryTests
    {
        public MockRepository CreateRepository()
        {
            return new MockRepository();
        }

        public ISampleClass CreateMock(MockRepository repository)
        {
            return repository.DynamicMock<ISampleClass>();
        }

        [Test]
        public void YouCanCreateRepositoryAndExpectCallMethods()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a mock.
            var mock = CreateMock(repository);

            // Arrange some expectations.
            mock.Expect(m => m.MethodThatReturnsInteger("foo")).Return(100);
            mock.Expect(m => m.MethodThatReturnsInteger("bar")).Return(200);

            // Replay all in repository.
            repository.ReplayAll();

            // Call expected methods.
            mock.MethodThatReturnsInteger("foo").Should(Be.EqualTo(100));
            mock.MethodThatReturnsInteger("bar").Should(Be.EqualTo(200));

            // Check that all expectations were met.
            repository.VerifyAll();
        }

        [Test]
        public void YouCanCreateRepositoryAndExpectCallMethodsInOrderedWay()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a mock.
            var mock = CreateMock(repository);

            // Arrange some expectations.
            using (repository.Ordered())
            {
                mock.Expect(m => m.MethodThatReturnsInteger("foo")).Return(100);
                mock.Expect(m => m.MethodThatReturnsInteger("bar")).Return(200);
                mock.Expect(m => m.MethodThatReturnsInteger("foo")).Return(300);
            }

            // Replay all in repository.
            repository.ReplayAll();

            // Call expected methods.
            mock.MethodThatReturnsInteger("foo").Should(Be.EqualTo(100));
            mock.MethodThatReturnsInteger("bar").Should(Be.EqualTo(200));
            mock.MethodThatReturnsInteger("foo").Should(Be.EqualTo(300));

            // Check that all expectations were met.
            repository.VerifyAll();
        }

        [Test]
        public void YouCanCreateRepositoryAndExpectCallMethodsInMixedWay()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a mock.
            var mock = CreateMock(repository);

            // Arrange some expectations.
            using (repository.Ordered())
            {
                mock.Expect(m => m.MethodThatReturnsInteger("foo")).Return(100);
                using (repository.Unordered())
                {
                    mock.Expect(m => m.MethodThatReturnsInteger("bar1")).Return(201);
                    mock.Expect(m => m.MethodThatReturnsInteger("bar2")).Return(202);
                    mock.Expect(m => m.MethodThatReturnsInteger("bar3")).Return(203);
                }
                mock.Expect(m => m.MethodThatReturnsInteger("foo")).Return(300);
            }

            // Replay all in repository.
            repository.ReplayAll();

            // Call expected methods.
            mock.MethodThatReturnsInteger("foo").Should(Be.EqualTo(100));
            mock.MethodThatReturnsInteger("bar3").Should(Be.EqualTo(203));
            mock.MethodThatReturnsInteger("bar2").Should(Be.EqualTo(202));
            mock.MethodThatReturnsInteger("bar1").Should(Be.EqualTo(201));
            mock.MethodThatReturnsInteger("foo").Should(Be.EqualTo(300));

            // Check that all expectations were met.
            repository.VerifyAll();
        }
    }
}
