using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.ExtensionMethods;
using Rhino.Mocks;

namespace NDepth.Tests.UnitTests.RhinoMocksTests
{
    [TestFixture]
    public class PartialMockTests
    {
        public MockRepository CreateRepository()
        {
            return new MockRepository();
        }

        public SampleClass CreatePartialMock(MockRepository repository, string name = null)
        {
            return string.IsNullOrEmpty(name) ? repository.PartialMock<SampleClass>() : repository.PartialMock<SampleClass>(name);
        }

        [Test]
        public void HereIsHowYouCreateAPartialMock()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // You have to call Replay() when you're done setting up the partial mock
            // and you want to make actual calls to methods on the partial mock.
            partialMock.Replay();

            // You have to call ReplayAll() when you're done setting up the repository
            // and you want to make actual calls to methods on all partial mocks.
            repository.ReplayAll();
        }

        [Test]
        public void YouCanPassInConstuctorArgumentsAndRhinoMocksWillPickTheConstructorThatFits()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository, "foo");

            // Replay the current mock.
            partialMock.Replay();

            // Check that mock was created with constructor provided with "foo".
            partialMock.SetByConstructor.Should(Be.EqualTo("foo"));
        }

        [Test]
        public void CallingNonVirtualMethodsWillCallTheActualMethod()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            // Call a non virtual method. 
            partialMock.VoidMethod();

            // Check that the non virtual method was called.
            partialMock.VoidMethodWasCalled.Should(Be.True);
        }

        [Test]
        public void CallingVirtualMethodsWillCallTheActualMethod()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            // Call a virtual method.
            partialMock.VirtualMethod("foo").Should(Be.EqualTo(3));

            // Check that the actual virtual method was called.
            partialMock.VirtualMethodWasCalled.Should(Be.True);
            partialMock.AssertWasCalled(pm => pm.VirtualMethod("foo"));
        }

        [Test]
        public void YouCanStubAVirtualMethodAndGiveItAReturnValue()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Arrange a new virtual method.
            partialMock.Stub(pm => pm.VirtualMethod("foo")).Return(100);

            // Replay the current mock.
            partialMock.Replay();

            // Call the virtal method.
            partialMock.VirtualMethod("foo").Should(Be.EqualTo(100));

            // It's not actually going to run the real method since we stubbed it out.
            partialMock.VirtualMethodWasCalled.Should(Be.False);
            partialMock.AssertWasCalled(pm => pm.VirtualMethod("foo"));
        }

        [Test]
        public void YouCanHaveVirtualMethodsThrowAnExceptionWhenTheyAreCalled()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Arrange a new virtual method that throws an exception.
            partialMock.Stub(pm => pm.VirtualMethod("foo")).Throw(new InvalidOperationException());

            // Replay the current mock.
            partialMock.Replay();

            // Check that the new virtual method throws the exception.
            Assert.Throws<InvalidOperationException>(() => partialMock.VirtualMethod("foo"));
        }

        [Test]
        public void YouCannotStubANonVirtualMethod()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Check that attempt to arrange non virtual method leads to InvalidOperationException.
            Assert.Throws<InvalidOperationException>(() => partialMock.Stub(pm => pm.NonVirtualMethod(1)).Return(new List<int> { 3 }));
        }

        [Test]
        public void YouCanUseWhenCalledWithVirtualMethods()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Arrange a virtual method using "WhenCalled" syntax.
            partialMock.Stub(pm => pm.VirtualMethod(Arg<string>.Is.Anything))
                .Return(0)
                .WhenCalled(method =>
                {
                    var param = (string)method.Arguments[0];
                    method.ReturnValue = int.Parse(param);
                });

            // Replay the current mock.
            partialMock.Replay();

            // Check that the virtual method returns proper values.
            partialMock.VirtualMethod("3").Should(Be.EqualTo(3));
            partialMock.VirtualMethod("6").Should(Be.EqualTo(6));
        }

        [Test]
        public void YouCannotUseWhenCalledWithNonVirtualMethods()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Check that attempt to arrange non virtual method leads to InvalidOperationException.
            Assert.Throws<InvalidOperationException>(() => partialMock.Stub(pm => pm.NonVirtualMethod(Arg<int>.Is.Anything))
                .Return(null)
                .WhenCalled(method =>
                {
                    var param = (int)method.Arguments[0];
                    method.ReturnValue = new[] { param, param + 1, param + 2 };
                }));
        }

        [Test]
        public void YouCanCheckToSeeIfAVirtualMethodWasCalled()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            // Call a virtual method.
            partialMock.VirtualMethod("foo");

            // Check that the virtual method was called.
            partialMock.AssertWasCalled(pm => pm.VirtualMethod("foo"));
            partialMock.AssertWasCalled(pm => pm.VirtualMethod(Arg<string>.Is.Anything));
            partialMock.AssertWasCalled(pm => pm.VirtualMethod("foo"), o => o.Repeat.Once());
        }

        [Test]
        public void YouCannotUseAssertWasNotCalledOnANonVirtualMethod()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            // Check that "AssertWasNotCalled" will not work for non virtual methods.
            Assert.Throws<InvalidOperationException>(() => partialMock.AssertWasNotCalled(pm => pm.NonVirtualMethod(1)));
        }

        [Test]
        public void YouCanGetTheArgumentsOfCallsToAVirtualMethod()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            // Call a virtual method.
            partialMock.VirtualMethod("foo");

            // Check that call arguments are available for virtual method call.
            IList<object[]> argsPerCall = partialMock.GetArgumentsForCallsMadeOn(pm => pm.VirtualMethod("foo"));
            argsPerCall[0][0].Should(Be.EqualTo("foo"));
        }

        [Test]
        public void YouCannotGetTheArgumentsOfCallsToANonVirtualMethod()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            // Call a non virtual method.
            partialMock.NonVirtualMethod(1);

            // Check that call arguments are not available for non virtual methods.
            Assert.Throws<InvalidOperationException>(() => partialMock.GetArgumentsForCallsMadeOn(pm => pm.NonVirtualMethod(0)));
        }

        [Test]
        public void NonVirtualPropertiesWorkAsNormal()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            // Check that non virtual properties works as expected.
            partialMock.NonVirtualProperty = "foo";
            partialMock.NonVirtualProperty.Should(Be.EqualTo("foo"));
        }

        [Test]
        public void VirtualPropertiesWorkAsNormal()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            // Check that virtual properties works as expected.
            partialMock.VirtualProperty = "foo";
            partialMock.VirtualProperty.Should(Be.EqualTo("foo"));
        }

        [Test]
        public void YouCanTellVirtualEventsOnAPartialMockToFire()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            var eventWasHandled = false;

            // Attach an event handler.
            partialMock.SomeVirtualEvent += (args, e) => eventWasHandled = true;

            // Raise the event.
            partialMock.Raise(s => s.SomeVirtualEvent += null, this, EventArgs.Empty);

            // Check that the event was raised.
            eventWasHandled.Should(Be.True);
        }

        [Test]
        public void YouCanNotTellNonVirtualEventsOnAPartialMockToFire()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            var eventWasHandled = false;

            // Attach an event handler.
            partialMock.SomeEvent += (args, e) => eventWasHandled = true;

            // Raise the event.
            Assert.Throws<InvalidOperationException>(() => partialMock.Raise(pm => pm.SomeEvent += null, this, EventArgs.Empty));

            // Check that the event was raised.
            eventWasHandled.Should(Be.False);
        }

        [Test]
        public void VirtualEventsWorkNormally()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            var eventWasHandled = false;

            // Attach an event handler.
            partialMock.SomeVirtualEvent += (args, e) => eventWasHandled = true;

            // Raise the event.
            partialMock.FireSomeVirtualEvent();

            // Check that the event was raised.
            eventWasHandled.Should(Be.True);
        }

        [Test]
        public void NonVirtualEventsWorkNormally()
        {
            // Create a mock repository.
            var repository = CreateRepository();
            // Create a partial mock.
            var partialMock = CreatePartialMock(repository);

            // Replay the current mock.
            partialMock.Replay();

            var eventWasHandled = false;

            // Attach an event handler.
            partialMock.SomeEvent += (args, e) => eventWasHandled = true;

            // Raise the event.
            partialMock.FireSomeEvent();

            // Check that the event was raised.
            eventWasHandled.Should(Be.True);
        }
    }
}
