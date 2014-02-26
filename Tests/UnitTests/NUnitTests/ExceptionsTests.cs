using System;
using NUnit.Framework;

namespace NDepth.Tests.UnitTests.NUnitTests
{
    [TestFixture]
    [Category("Exceptions tests")]
    public class ExceptionsTests
    {
        public void ThowsNothing() { }
        public void ThowsMyException() { throw new MyException("My exception", 123);}
        public void ThowsMyExceptionChild() { throw new MyExceptionChild("My exception child", 123); }

        [Test]
        public void AssertThrows()
        {
            Assert.DoesNotThrow(ThowsNothing, "Method does not throw any exceptions");
            Assert.DoesNotThrow(() => { }, "Lambda does not throw any exceptions");

            Assert.Throws<MyException>(ThowsMyException, "Method throws MyException");
            Assert.Throws<MyException>(() => { throw new MyException("My exception", 123); }, "Lambda throws MyException");

            Assert.Throws<MyExceptionChild>(ThowsMyExceptionChild, "Method throws MyExceptionChild");
            Assert.Throws<MyExceptionChild>(() => { throw new MyExceptionChild("My exception child", 123); }, "Lambda throws MyExceptionChild");

            Assert.Throws(Is.InstanceOf<MyException>(), ThowsMyExceptionChild, "Method throws MyExceptionChild");
            Assert.Throws(Is.InstanceOf<MyException>(), () => { throw new MyExceptionChild("My exception child", 123); }, "Lambda throws MyExceptionChild");

            Assert.Catch<MyException>(ThowsMyException, "Catches MyException");
            Assert.Catch<MyException>(ThowsMyExceptionChild, "Catches  MyException");
            Assert.Catch<MyException>(() => { throw new MyException("My exception", 123); }, "Catches MyException");
            Assert.Catch<MyException>(() => { throw new MyExceptionChild("My exception child", 123); }, "Catches MyException");

            Assert.Catch(ThowsMyException, "Catches any exception");
            Assert.Catch(ThowsMyExceptionChild, "Catches any exception");
            Assert.Catch(() => { throw new MyException("My exception", 123); }, "Catches any exception");
            Assert.Catch(() => { throw new MyExceptionChild("My exception child", 123); }, "Catches any exception");
        }

        [Test]
        public void AssertConstraintThrows()
        {
            var ex = Assert.Throws<MyException>(ThowsMyException);
            Assert.That(ex.Message, Is.EqualTo("My exception"));
            Assert.That(ex.Value, Is.EqualTo(123));

            Assert.Throws(Is.TypeOf<MyException>().And.Message.EqualTo("My exception").And.Property("Value").EqualTo(123), ThowsMyException);

            Assert.That(ThowsNothing, Throws.Nothing);
            Assert.That(ThowsMyException, Throws.TypeOf<MyException>());
            Assert.That(ThowsMyExceptionChild, Throws.InstanceOf<MyException>());

            Assert.That(ThowsMyException, Throws.TypeOf<MyException>().With.Message.EqualTo("My exception").And.Property("Value").EqualTo(123));
        }

        [Test]
        [ExpectedException("System.InvalidOperationException")]
        public void ExpectedExceptionByName()
        {
            throw new InvalidOperationException("Invalid operation");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExpectedExceptionByType()
        {
            throw new InvalidOperationException("Invalid operation");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Invalid operation")]
        public void ExpectedExceptionWithArgumentCheck()
        {
            throw new InvalidOperationException("Invalid operation");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "operation", MatchType = MessageMatch.Contains)]
        public void ExpectedExceptionWithArgumentMatch()
        {
            throw new InvalidOperationException("Invalid operation");
        }

        [Test]
        [TestCase(0)]
        [ExpectedException(typeof(ArgumentException), Handler = "ExpectedExceptionHandler")]
        public void ExpectedExceptionWithHandler(int test)
        {
            throw new ArgumentException("Argument exception", "test");
        }
        public void ExpectedExceptionHandler(Exception ex)
        {
            Assert.AreEqual("test", ((ArgumentException)ex).ParamName);
        }
    }

    #region Utilities

    public class MyException : Exception
    {
        public MyException(string message, int value) : base(message)
        {
            Value = value;
        }

        public int Value { get; private set; }
    }

    public class MyExceptionChild : MyException
    {
        public MyExceptionChild(string message, int value) : base(message, value) { }
    }

    #endregion
}
