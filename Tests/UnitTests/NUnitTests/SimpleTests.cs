using System;
using System.IO;
using NUnit.Framework;

namespace NDepth.Tests.UnitTests.NUnitTests
{
    [TestFixture]
    [Category("Simple tests")]
    public class SimpleTests
    {
        [Test]
        public void EqualityAssertsTest()
        {
            Assert.AreEqual(10, 10, "10 is equal to 10");
            Assert.AreEqual(10.123, 10.1234567, 0.001, "10.123 is equal to 10.1234567 with tolerance 0.001");
            Assert.AreEqual("test", "test", "Two strings are equal");
            Assert.AreEqual(new ObjectWrapper(10), new ObjectWrapper(10), "Two similar objects are equal");

            Assert.AreEqual(double.PositiveInfinity, double.PositiveInfinity);
            Assert.AreEqual(double.NegativeInfinity, double.NegativeInfinity);
            Assert.AreEqual(double.NaN, double.NaN);

            Assert.AreEqual(new [] { 1, 2, 3, 4, 5 }, new [] { 1, 2, 3, 4, 5 }, "Two similar int arrays are equal");

            Assert.AreEqual(typeof(ObjectWrapper), (new ObjectWrapper(10)).GetType(), "Two similar object types are equal");
            Assert.AreEqual("System.String", "test".GetType().FullName, "Two string types are equal");

            Assert.AreNotEqual(10, 11, "10 is not equal to 11");
            Assert.AreNotEqual("test", "notest", "Two strings are not equal");
            Assert.AreNotEqual(10.123, 10.1234567, "10.123 is not equal to 10.1234567");
            Assert.AreNotEqual(new ObjectWrapper(10), new ObjectWrapper(11), "Two different objects are not equal");

            Assert.AreNotEqual(double.PositiveInfinity, double.NegativeInfinity);
            Assert.AreNotEqual(double.NegativeInfinity, double.PositiveInfinity);
            Assert.AreNotEqual(double.NaN, double.NegativeInfinity);

            Assert.AreNotEqual(new [] { 1, 2, 3, 4, 5 }, new [] { 1, 2, 3, 4, 5, 6 }, "Two different int arrays are not equal");

            Assert.AreNotEqual(typeof(ObjectWrapper), 10.GetType(), "Two different object types are not equal");
            Assert.AreNotEqual("System.Object", "test".GetType().FullName, "Two different object types are not equal");
        }

        [Test]
        public void IdentityAssertsTest()
        {
            object obj1 = new ObjectWrapper(10);
            object obj2 = new ObjectWrapper(10);
            object obj3 = obj1;

            Assert.AreSame(obj1, obj3, "Two objects with same reference are same");
            Assert.AreNotSame(obj1, obj2, "Two objects with different reference are not same");
        }

        [Test]
        public void ConditionAssertsTest()
        {
            object obj1 = null;
            object obj2 = new ObjectWrapper(10);

            Assert.IsNull(obj1, "Object is null");
            Assert.Null(obj1, "Object is null");

            Assert.IsNotNull(obj2, "Object is not null");
            Assert.NotNull(obj2, "Object is not null");

            Assert.IsTrue(2 + 2 == 4, "2 + 2 == 4");
            Assert.True(2 + 2 == 4, "2 + 2 == 4");

            Assert.IsFalse(2 + 2 == 5, "2 + 2 != 5");
            Assert.False(2 + 2 == 5, "2 + 2 != 5");

            Assert.IsNaN(float.NaN, "Float NaN");

            string str = string.IsNullOrEmpty(string.Empty) ? null : string.Empty;
            Assert.IsEmpty("", "Empty string");
            Assert.IsNotEmpty("test", "Not empty string");
            Assert.IsNullOrEmpty("", "Null or empty string");
            Assert.IsNullOrEmpty(str, "Null or empty string");
            Assert.IsNotNullOrEmpty("test", "Not null or empty string");

            Assert.IsEmpty(new int[] { }, "Empty collection of int numbers");
            Assert.IsNotEmpty(new [] { 1, 2, 3 }, "Not empty collection of int numbers");
        }

        [Test]
        public void ComparisonsTest()
        {
            IComparable obj1 = new ObjectWrapper(10);
            IComparable obj2 = new ObjectWrapper(20);

            Assert.Greater(20, 10, "20 > 10");
            Assert.Greater(20.123, 10.123, "20.123 > 10.123");
            Assert.Greater(obj2, obj1, "IComparable(20) > IComparable(10)");

            Assert.GreaterOrEqual(20, 10, "20 >= 10");
            Assert.GreaterOrEqual(20, 20, "20 >= 20");
            Assert.GreaterOrEqual(20.123, 10.123, "20.123 >= 10.123");
            Assert.GreaterOrEqual(20.123, 20.123, "20.123 >= 20.123");
            Assert.GreaterOrEqual(obj2, obj1, "IComparable(20) >= IComparable(10)");
            Assert.GreaterOrEqual(obj2, obj2, "IComparable(20) >= IComparable(20)");

            Assert.Less(10, 20, "10 < 20");
            Assert.Less(10.123, 20.123, "10.123 < 20.123");
            Assert.Less(obj1, obj2, "IComparable(10) < IComparable(20)");

            Assert.LessOrEqual(10, 20, "10 <= 20");
            Assert.LessOrEqual(10, 10, "10 <= 10");
            Assert.LessOrEqual(10.123, 20.123, "10.123 <= 20.123");
            Assert.LessOrEqual(10.123, 10.123, "10.123 <= 10.123");
            Assert.LessOrEqual(obj1, obj2, "IComparable(10) <= IComparable(20)");
            Assert.LessOrEqual(obj1, obj1, "IComparable(10) <= IComparable(10)");
        }

        [Test]
        public void TypeAssertsTest()
        {
            object objWrapper = new ObjectWrapper(10);
            object objWrapperChild = new ObjectWrapperChild(20);

            Assert.IsInstanceOf(typeof(int), 10, "10 is int type");
            Assert.IsInstanceOf(typeof(object), objWrapper, "Object wrapper is object type");
            Assert.IsInstanceOf(typeof(ObjectWrapper), objWrapper, "Object wrapper is ObjectWrapper type");
            Assert.IsInstanceOf(typeof(ObjectWrapper), objWrapperChild, "Object wrapper child is ObjectWrapper type");
            Assert.IsInstanceOf(typeof(ObjectWrapperChild), objWrapperChild, "Object wrapper child is ObjectWrapperChild type");

            Assert.IsInstanceOf<int>(10, "10 is int type");
            Assert.IsInstanceOf<object>(objWrapper, "Object wrapper is object type");
            Assert.IsInstanceOf<ObjectWrapper>(objWrapper, "Object wrapper is ObjectWrapper type");
            Assert.IsInstanceOf<ObjectWrapper>(objWrapperChild, "Object wrapper child is ObjectWrapper type");
            Assert.IsInstanceOf<ObjectWrapperChild>(objWrapperChild, "Object wrapper child is ObjectWrapperChild type");

            Assert.IsNotInstanceOf(typeof(string), 10, "10 is not string type");
            Assert.IsNotInstanceOf(typeof(string), objWrapper, "Object wrapper is not string type");
            Assert.IsNotInstanceOf(typeof(ObjectWrapperChild), objWrapper, "Object wrapper is not ObjectWrapperChild type");

            Assert.IsNotInstanceOf<string>(10, "10 is not string type");
            Assert.IsNotInstanceOf<string>(objWrapper, "Object wrapper is not string type");
            Assert.IsNotInstanceOf<ObjectWrapperChild>(objWrapper, "Object wrapper is not ObjectWrapperChild type");

            Assert.IsAssignableFrom(typeof(int), 10, "10 can be assignable from int type");
            Assert.IsAssignableFrom(typeof(ObjectWrapper), objWrapper, "Object wrapper can be assignable from ObjectWrapper type");
            Assert.IsAssignableFrom(typeof(ObjectWrapperChild), objWrapper, "Object wrapper child can be assignable from ObjectWrapper type");
            Assert.IsAssignableFrom(typeof(ObjectWrapperChild), objWrapperChild, "Object wrapper child can be assignable from ObjectWrapperChild type");

            Assert.IsAssignableFrom<int>(10, "10 can be assignable from int type");
            Assert.IsAssignableFrom<ObjectWrapper>(objWrapper, "Object wrapper can be assignable from ObjectWrapper type");
            Assert.IsAssignableFrom<ObjectWrapperChild>(objWrapper, "Object wrapper child can be assignable from ObjectWrapper type");
            Assert.IsAssignableFrom<ObjectWrapperChild>(objWrapperChild, "Object wrapper child can be assignable from ObjectWrapperChild type");

            Assert.IsNotAssignableFrom(typeof(string), 10, "10 can not be assignable from string type");
            Assert.IsNotAssignableFrom(typeof(object), objWrapper, "Object wrapper can not be assignable from object type");
            Assert.IsNotAssignableFrom(typeof(ObjectWrapper), objWrapperChild, "Object wrapper child can not be assignable from ObjectWrapper type");

            Assert.IsNotAssignableFrom<string>(10, "10 can not be assignable from string type");
            Assert.IsNotAssignableFrom<object>(objWrapper, "Object wrapper can not be assignable from object type");
            Assert.IsNotAssignableFrom<ObjectWrapper>(objWrapperChild, "Object wrapper child can not be assignable from ObjectWrapper type");
        }

        [Test]
        public void StringAssertsTest()
        {
            StringAssert.Contains("world", "Hello world!");
            StringAssert.DoesNotContain("word", "Hello world!");

            StringAssert.StartsWith("Hello", "Hello world!");
            StringAssert.DoesNotStartWith("Hell0", "Hello world!");

            StringAssert.EndsWith("world!", "Hello world!");
            StringAssert.DoesNotEndWith("world?", "Hello world!");

            StringAssert.AreEqualIgnoringCase("Hello world!", "hello WORLD!");
            StringAssert.AreNotEqualIgnoringCase("Hello world!", "hello WORD!");

            StringAssert.IsMatch(@"\w*\s\w*", "Hello world!");
            StringAssert.DoesNotMatch(@"\w\s\s\w", "Hello world!");
        }

        [Test]
        public void CollectionAssertsTest()
        {
            CollectionAssert.IsEmpty(new int[] { }, "Collection is empty");
            CollectionAssert.IsNotEmpty(new [] { 1, 2, 3 }, "Collection is not empty");
            
            var ints = new object[] { 4, 3, 2, 1 };

            CollectionAssert.AllItemsAreNotNull(ints, "All items in array are not null");
            CollectionAssert.AllItemsAreInstancesOfType(ints, typeof(int), "All items in array are int numbers");
            CollectionAssert.AllItemsAreUnique(ints, "All items in array are unique");

            CollectionAssert.Contains(ints, 2, "Int array contains value 2");
            CollectionAssert.DoesNotContain(ints, 0, "Int array does not contain value 0");

            var ints1 = new object[] { 1, 2, 3, 4 };
            var ints2 = new object[] { 2, 3, 4, 5 };
            var ints3 = ints1;

            CollectionAssert.AreEqual(ints1, ints3, "Two references are pointed to the same array");
            CollectionAssert.AreNotEqual(ints1, ints2, "Two references are pointed to the different arrays");

            CollectionAssert.AreEquivalent(ints, ints1, "Two arrays are equivalent");
            CollectionAssert.AreNotEquivalent(ints, ints2, "Two arrays are not equivalent");

            CollectionAssert.IsSubsetOf(new [] { 2, 3 }, ints, "Int array contains set [2, 3]");
            CollectionAssert.IsSubsetOf(new [] { 1, 2, 3, 4 }, ints, "Int array contains set [1, 4]");
            CollectionAssert.IsNotSubsetOf(new [] { 4, 5 }, ints, "Int array does not contain set [4, 5]");
            CollectionAssert.IsNotSubsetOf(new [] { 0, 1, 2, 3, 4, 5 }, ints, "Int array does not contain set [0, 5]");

            CollectionAssert.IsOrdered(ints1, "Array of int numbers is ordered");
        }

        [Test]
        public void FileAssertsTest()
        {
            try
            {
                FileStream file1 = File.OpenRead("Test1.txt");
                FileStream file2 = File.OpenRead("Test1.txt");
                FileStream file3 = File.OpenRead("Test2.txt");
                FileAssert.AreEqual(file1, file2, "Two files has equal content");
                FileAssert.AreNotEqual(file1, file3, "Two files has different content");

                var fileinfo1 = new FileInfo("Test1.txt");
                var fileinfo2 = new FileInfo("Test1.txt");
                var fileinfo3 = new FileInfo("Test2.txt");
                FileAssert.AreEqual(fileinfo1, fileinfo2, "Two files has equal content");
                FileAssert.AreNotEqual(fileinfo1, fileinfo3, "Two files has different content");

                FileAssert.AreEqual("Test1.txt", "Test1.txt", "Two files has equal content");
                FileAssert.AreNotEqual("Test1.txt", "Test2.txt", "Two files has different content");
            }
            catch (IOException ex)
            {
                Assert.Fail("IOException - {0}", ex);
            }
        }

        [Test]
        public void UtilityMethodsTest()
        {
            // Test is passed.
            Assert.Pass("Test is passed!");

            // Fail this test.
            // Assert.Fail("Test is failed!");

            // Ignore current test.
            // Assert.Ignore("Test is ignored!");

            // Skip this test with the current set of prepared test data and try another one.
            // Assert.Inconclusive("Test is skipped.");
        }
    }
}
