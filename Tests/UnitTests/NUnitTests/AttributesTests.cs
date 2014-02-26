using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace NDepth.Tests.UnitTests.NUnitTests
{
    [TestFixture, Description("Attributes tests")]
    [Category("Attributes tests")]
    public class AttributesTests
    {
        #region Test fixture setup 

        [TestFixtureSetUp]
        public void FixtureInit()
        {
            Console.WriteLine("AttributesTests.FixtureInit()");
        }

        [TestFixtureSetUp]
        public static void FixtureInitStatic()
        {
            Console.WriteLine("AttributesTests.FixtureInitStatic()");
        }

        [TestFixtureTearDown]
        public void FixtureCleanup()
        {
            Console.WriteLine("AttributesTests.FixtureCleanup()");
        }

        [TestFixtureTearDown]
        public static void FixtureCleanupStatic()
        {
            Console.WriteLine("AttributesTests.FixtureCleanupStatic()");
        }
        
        #endregion 

        #region Test setup

        [SetUp]
        public void Init()
        {
            Console.WriteLine("AttributesTests.Init()");
        }

        [SetUp]
        public static void InitStatic()
        {
            Console.WriteLine("AttributesTests.InitStatic()");
        }

        [TearDown]
        public void Cleanup()
        {
            Console.WriteLine("AttributesTests.Cleanup()");
        }

        [TearDown]
        public static void CleanupStatic()
        {
            Console.WriteLine("AttributesTests.CleanupStatic()");
        }

        #endregion 

        #region Description attribute

        [Test, Description("Description attribute test")]
        public void DescriptionAttributeTest()
        {
            Assert.Pass();
        }

        #endregion 

        #region Value attributes

        [Test]
        public void ValuesAttributeTest([Values(1, 2, 3)] int x, [Values("A", "B")] string s)
        {
            Assert.Pass();
        }

        [Test]
        [Combinatorial]
        public void CombinatorialAttributeTest([Values(1, 2, 3)] int x, [Values("A", "B")] string s)
        {
            Assert.Pass();
        }

        [Test]
        [Pairwise]
        public void PairwiseAttributeTest([Values(1, 2, 3)] int x, [Values("A", "B")] string s)
        {
            Assert.Pass();
        }

        [Test]
        [Category("Random attribute tests")]
        public void RandomAttributeTest1([Values(1, 2, 3)] int x, [Random(3)] double d)
        {
            Assert.Pass();
        }

        [Test]
        [Category("Random attribute tests")]
        public void RandomAttributeTest2([Values(1, 2, 3)] int x, [Random(-1.0, 1.0, 3)] double d)
        {
            Assert.Pass();
        }

        [Test]
        [Category("Random attribute tests")]
        public void RandomAttributeTest3([Values(1, 2, 3)] int x, [Random(0, 100, 3)] int i)
        {
            Assert.Pass();
        }

        [Test]
        [Category("Range attribute tests")]
        public void RangeAttributeTest1([Values(1, 2, 3)] int x, [Range(1, 10)] int i)
        {
            Assert.Pass();
        }

        [Test]
        [Category("Range attribute tests")]
        public void RangeAttributeTest2([Values(1, 2, 3)] int x, [Range(1, 10, 2)] int i)
        {
            Assert.Pass();
        }

        [Test]
        [Category("Range attribute tests")]
        public void RangeAttributeTest3([Values(1, 2, 3)] int x, [Range(0.1, 1.0, 0.3)] double d)
        {
            Assert.Pass();
        }

        [Test]
        [Sequential]
        public void SequentialAttributeTest([Values(1, 2, 3)] int x, [Values("A", "B")] string s)
        {
            Assert.Pass();
        }

        #endregion

        #region Repeat attribute

        [Test]
        [Repeat(10)]
        public void RepeatAttributeTest()
        {
            Assert.Pass();
        }

        #endregion

        #region Max time & Timeout attributes

        [Test]
        [MaxTime(2000)]
        public void MaxTimeAttributeTest()
        {
            Thread.Sleep(1000);
            Assert.Pass();
        }

        [Test]
        [Timeout(2000)]
        public void TimeoutAttributeTest()
        {
            Thread.Sleep(1000);
            Assert.Pass();
        }

        #endregion

        #region RequiresSTA, RequiresMTA and RequiresThread attributes

        [Test]
        [RequiresSTA]
        public void RequiresStaTest()
        {
            Assert.Pass();
        }

        [Test]
        [RequiresMTA]
        public void RequiresMtaTest()
        {
            Assert.Pass();
        }

        [Test]
        [RequiresThread]
        public void RequiresThreadTest()
        {
            Assert.Pass();
        }

        #endregion

        #region Explicit & Ignore attributes

        [Test, Explicit]
        public void ExplicitAttributeTest()
        {
            Assert.Fail();
        }

        [Test, Ignore]
        public void IgnoreAttributeTest()
        {
            Assert.Fail();
        }

        #endregion

        #region Culture attributes

        [Test]
        [Culture("en,ru")]
        public void CultureAttributeTest()
        {
            // Test will be run and passed for "en-*" and "ru-*" cultures (for other cultures it will be skipped).
            Assert.Pass();
        }

        [Test]
        [Culture(Exclude = "de,fr")]
        public void CultureExcludeAttributeTest()
        {
            // Test will be run and passed for all cultures except "de-*" and "fr-*".
            Assert.Pass();
        }

        [Test]
        [SetCulture("ru-RU")]
        public void SetCultureAttributeTest()
        {
            Assert.That(CultureInfo.CurrentCulture.Name, Is.EqualTo("ru-RU"));
            Assert.That(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, Is.EqualTo(","));
        }

        [Test]
        [SetUICulture("ru-RU")]
        public void SetUiCultureAttributeTest()
        {
            Assert.That(CultureInfo.CurrentUICulture.Name, Is.EqualTo("ru-RU"));
            Assert.That(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator, Is.EqualTo(","));
        }

        #endregion

        #region Platfrom attribute

        [Test]
        [Platform("Net-4.0, Mono-3.5", Exclude = "Win95,Win98,WinMe")]
        public void PlatfromAttributeTest()
        {
            Assert.Pass();
        }

        #endregion
    }

    #region Setup fixture

    [SetUpFixture]
    public class MySetUpClass
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            Console.WriteLine("MySetUpClass.RunBeforeAnyTests()");
        }
        
        [TearDown]
        public void RunAfterAnyTests()
        {
            Console.WriteLine("MySetUpClass.RunAfterAnyTests()");
        }
    }

    #endregion
}
