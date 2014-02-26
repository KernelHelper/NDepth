using System;
using System.Collections;
using NUnit.Framework;

namespace NDepth.Tests.UnitTests.NUnitTests
{
    [TestFixture]
    [Category("Theory tests")]
    public class TestCaseTests
    {
        [Test]
        [TestCase(12, 3, 4)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 4, 3)]
        public void DivideTestVoid(int n, int d, int q)
        {
            Assert.That(n / d, Is.EqualTo(q));
        }

        [Test]
        [TestCase(12, 3, Result = 4)]
        [TestCase(12, 2, Result = 6)]
        [TestCase(12, 4, Result = 3)]
        public int DivideTestReturns(int n, int d)
        {
            return n / d;
        }

        public static readonly object[] DivideCases =
        {
            new object[] { 12, 3, 4 },
            new object[] { 12, 2, 6 },
            new object[] { 12, 4, 3 } 
        };

        [Test] 
        [TestCaseSource("DivideCases")]
        public void DivideTestData1(int n, int d, int q)
        {
            Assert.That(n / d, Is.EqualTo(q));
        }

        public class MyFactoryClass
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(12, 3).Returns(4);
                    yield return new TestCaseData(12, 2).Returns(6);
                    yield return new TestCaseData(12, 4).Returns(3);
                    yield return new TestCaseData(0, 0)
                      .Throws(typeof(DivideByZeroException))
                      .SetName("DivideByZero")
                      .SetDescription("An exception is expected");
                }
            }
        }

        [Test]
        [TestCaseSource(typeof(MyFactoryClass), "TestCases")]
        public int DivideTestData2(int n, int d)
        {
            return n / d;
        }
    }
}
