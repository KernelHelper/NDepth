using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NDepth.Tests.UnitTests.NUnitTests
{
    [TestFixture]
    [Category("Constraints tests")]
    public class ConstraintsTests
    {
        [Test]
        public void EqualConstraintTest()
        {
           
            Assert.That(5 + 5 == 10, "5 + 5 is equal to 10");
            Assert.That(5 + 5, Is.EqualTo(10), "5 + 5 is equal to 10");
            Assert.That(1.1, Is.EqualTo(1).Within(11).Percent, "1.1 is equal to 1 within 10%");
            Assert.That(10.123, Is.EqualTo(10.1234567).Within(0.001), "10.123 is equal to 10.1234567 with tolerance 0.001");
            Assert.That(20000000000000004.0, Is.EqualTo(20000000000000000.0).Within(1).Ulps, "20000000000000004.0 is equal to 20000000000000000.0 with 1 units in the last place");

            Assert.That("test", Is.EqualTo("test"), "Two strings are equal");
            Assert.That("test", Is.Not.EqualTo("TEST"), "Two strings are not equal");
            Assert.That("test", Is.EqualTo("TEST").IgnoreCase, "Two strings are equal ignoring case");

            var expected = new [] { "Hello", "World" };
            var actual = new [] { "HELLO", "world" };
            Assert.That(actual, Is.EqualTo(expected).IgnoreCase, "Two string collections are equal");
            
            Assert.That(new ObjectWrapper(10), Is.EqualTo(new ObjectWrapper(10)), "Two similar objects are equal");

            Assert.That(double.PositiveInfinity, Is.EqualTo(double.PositiveInfinity));
            Assert.That(double.NegativeInfinity, Is.EqualTo(double.NegativeInfinity));
            Assert.That(double.NaN, Is.EqualTo(double.NaN));

            DateTime now = DateTime.Now;
            DateTime later = now + TimeSpan.FromHours(1.0);
            Assert.That(now, Is.EqualTo(now), "Two timestamps are equal");
            Assert.That(later, Is.EqualTo(now).Within(TimeSpan.FromHours(3)), "Two timestamps are equal within 3 hours");
            Assert.That(later, Is.EqualTo(now).Within(3).Hours, "Two timestamps are equal within 3 hours");

            var i3 = new [] { 1, 2, 3 };
            var d3 = new [] { 1.0, 2.0, 3.0 };
            var iunequal = new [] { 1, 3, 2 };
            Assert.That(i3, Is.EqualTo(d3), "Two arrays of same values and convertible types are equal");
            Assert.That(i3, Is.Not.EqualTo(iunequal), "Two arrays of different order values are not equal");

            var array2X2 = new [,] { { 1, 2 }, { 3, 4 } };
            var array4 = new [] { 1, 2, 3, 4 };
            Assert.That(array2X2, Is.Not.EqualTo(array4), "Two arrays of different dimensions are not equal");
            Assert.That(array2X2, Is.EqualTo(array4).AsCollection, "Two flat arrays are equal");            
        }

        [Test]
        public void SameAsConstraintTest()
        {
            object obj1 = new ObjectWrapper(10);
            object obj2 = new ObjectWrapper(10);
            object obj3 = obj1;

            Assert.That(obj1, Is.SameAs(obj3), "Two objects with same reference are same");
            Assert.That(obj1, Is.Not.SameAs(obj2), "Two objects with different reference are not same");
        }

        [Test]
        public void ConditionConstraintsTest()
        {
            object obj = new ObjectWrapper(10);
            Assert.That(obj, Is.Not.Null, "Object is not null");

            Assert.That(2 + 2 == 4, Is.True, "2 + 2 == 4");
            Assert.That(2 + 2 == 5, Is.False, "2 + 2 != 5");

            Assert.That(float.NaN, Is.NaN, "Float NaN");

            string str = string.IsNullOrEmpty(string.Empty) ? null : string.Empty;
            Assert.That("", Is.Empty, "Empty string");
            Assert.That("test", Is.Not.Empty, "Not empty string");
            Assert.That("", Is.Null | Is.Empty, "Null or empty string");
            Assert.That(str, Is.Null | Is.Empty, "Null or empty string");
            Assert.That("test", Is.Not.Null & Is.Not.Empty, "Not null or empty string");

            Assert.That(new int[] { }, Is.Empty, "Empty collection of int numbers");
            Assert.That(new [] { 1, 2, 3 }, Is.Not.Empty, "Not empty collection of int numbers");

            Assert.That(new [] { 1, 2, 3 }, Is.Unique, "Unique collection");
            Assert.That(new [] { 1, 2, 2 }, Is.Not.Unique, "Not unique collection");
        }

        [Test]
        public void ComparisonConstraintsTest()
        {
            IComparable obj1 = new ObjectWrapper(10);
            IComparable obj2 = new ObjectWrapper(20);

            Assert.That(20, Is.Positive, "20 > 0");
            Assert.That(20.123, Is.Positive, "20.123 > 0");

            Assert.That(-20, Is.Negative, "20 < 0");
            Assert.That(-20.123, Is.Negative, "20.123 < 0");

            Assert.That(20, Is.GreaterThan(10), "20 > 10");
            Assert.That(20.123, Is.GreaterThan(10.123), "20.123 > 10.123");
            Assert.That(obj2, Is.GreaterThan(obj1), "IComparable(20) > IComparable(10)");

            Assert.That(20, Is.AtLeast(10), "20 >= 10");
            Assert.That(20, Is.GreaterThanOrEqualTo(20), "20 >= 20");
            Assert.That(20.123, Is.AtLeast(10.123), "20.123 >= 10.123");
            Assert.That(20.123, Is.GreaterThanOrEqualTo(20.123), "20.123 >= 20.123");
            Assert.That(obj2, Is.AtLeast(obj1), "IComparable(20) >= IComparable(10)");
            Assert.That(obj2, Is.GreaterThanOrEqualTo(obj2), "IComparable(20) >= IComparable(20)");

            Assert.That(10, Is.LessThan(20), "10 < 20");
            Assert.That(10.123, Is.LessThan(20.123), "10.123 < 20.123");
            Assert.That(obj1, Is.LessThan(obj2), "IComparable(10) < IComparable(20)");

            Assert.That(10, Is.AtMost(20), "10 <= 20");
            Assert.That(10, Is.LessThanOrEqualTo(10), "10 <= 10");
            Assert.That(10.123, Is.AtMost(20.123), "10.123 <= 20.123");
            Assert.That(10.123, Is.LessThanOrEqualTo(10.123), "10.123 <= 10.123");
            Assert.That(obj1, Is.AtMost(obj2), "IComparable(10) <= IComparable(20)");
            Assert.That(obj1, Is.LessThanOrEqualTo(obj1), "IComparable(10) <= IComparable(10)");

            Assert.That(1, Is.InRange(0, 10), "1 is in range [0, 10]");
            Assert.That(12, Is.Not.InRange(0, 10), "12 is not in range [0, 10]");
            Assert.That(new [] { 1, 3, 5, 7, 9 }, Is.All.InRange(0, 10), "All array's items are in range [0, 10]");
        }

        [Test]
        public void TypeConstraintsTest()
        {
            object objWrapper = new ObjectWrapper(10);
            object objWrapperChild = new ObjectWrapperChild(20);

            Assert.That(10, Is.InstanceOf(typeof(int)), "10 is int type");
            Assert.That(objWrapper, Is.InstanceOf(typeof(object)), "Object wrapper is object type");
            Assert.That(objWrapper, Is.InstanceOf(typeof(ObjectWrapper)), "Object wrapper is ObjectWrapper type");
            Assert.That(objWrapperChild, Is.InstanceOf(typeof(ObjectWrapper)), "Object wrapper child is ObjectWrapper type");
            Assert.That(objWrapperChild, Is.InstanceOf(typeof(ObjectWrapperChild)), "Object wrapper child is ObjectWrapperChild type");

            Assert.That(10, Is.InstanceOf<int>(), "10 is int type");
            Assert.That(objWrapper, Is.InstanceOf<object>(), "Object wrapper is object type");
            Assert.That(objWrapper, Is.InstanceOf<ObjectWrapper>(), "Object wrapper is ObjectWrapper type");
            Assert.That(objWrapperChild, Is.InstanceOf<ObjectWrapper>(), "Object wrapper child is ObjectWrapper type");
            Assert.That(objWrapperChild, Is.InstanceOf<ObjectWrapperChild>(), "Object wrapper child is ObjectWrapperChild type");

            Assert.That(10, Is.Not.InstanceOf(typeof(string)), "10 is not string type");
            Assert.That(objWrapper, Is.Not.InstanceOf(typeof(string)), "Object wrapper is not string type");
            Assert.That(objWrapper, Is.Not.InstanceOf(typeof(ObjectWrapperChild)), "Object wrapper is not ObjectWrapperChild type");

            Assert.That(10, Is.Not.InstanceOf<string>(), "10 is not string type");
            Assert.That(objWrapper, Is.Not.InstanceOf<string>(), "Object wrapper is not string type");
            Assert.That(objWrapper, Is.Not.InstanceOf<ObjectWrapperChild>(), "Object wrapper is not ObjectWrapperChild type");

            Assert.That(10, Is.AssignableFrom(typeof(int)), "10 can be assignable from int type");
            Assert.That(objWrapper, Is.AssignableFrom(typeof(ObjectWrapper)), "Object wrapper can be assignable from ObjectWrapper type");
            Assert.That(objWrapper, Is.AssignableFrom(typeof(ObjectWrapperChild)), "Object wrapper child can be assignable from ObjectWrapper type");
            Assert.That(objWrapperChild, Is.AssignableFrom(typeof(ObjectWrapperChild)), "Object wrapper child can be assignable from ObjectWrapperChild type");

            Assert.That(10, Is.AssignableFrom<int>(), "10 can be assignable from int type");
            Assert.That(objWrapper, Is.AssignableFrom<ObjectWrapper>(), "Object wrapper can be assignable from ObjectWrapper type");
            Assert.That(objWrapper, Is.AssignableFrom<ObjectWrapperChild>(), "Object wrapper child can be assignable from ObjectWrapper type");
            Assert.That(objWrapperChild, Is.AssignableFrom<ObjectWrapperChild>(), "Object wrapper child can be assignable from ObjectWrapperChild type");

            Assert.That(10, Is.Not.AssignableFrom(typeof(string)), "10 can not be assignable from string type");
            Assert.That(objWrapper, Is.Not.AssignableFrom(typeof(object)), "Object wrapper can not be assignable from object type");
            Assert.That(objWrapperChild, Is.Not.AssignableFrom(typeof(ObjectWrapper)), "Object wrapper child can not be assignable from ObjectWrapper type");

            Assert.That(10, Is.Not.AssignableFrom<string>(), "10 can not be assignable from string type");
            Assert.That(objWrapper, Is.Not.AssignableFrom<object>(), "Object wrapper can not be assignable from object type");
            Assert.That(objWrapperChild, Is.Not.AssignableFrom<ObjectWrapper>(), "Object wrapper child can not be assignable from ObjectWrapper type");
        }

        [Test]
        public void StringConstraintsTest()
        {
            Assert.That("Hello world!", Contains.Substring("world"));
            Assert.That("Hello world!", Is.StringContaining("world"));
            Assert.That("Hello world!", Is.StringContaining("WORLD").IgnoreCase);
            Assert.That("Hello world!", Is.Not.StringContaining("word"));
            Assert.That("Hello world!", Is.Not.StringContaining("WORLD"));

            Assert.That("Hello world!", Is.StringStarting("Hello"));
            Assert.That("Hello world!", Is.Not.StringStarting("Hell0"));
            Assert.That("Hello world!", Has.Length.GreaterThan(10).And.Not.StartsWith("Hell0"));

            Assert.That("Hello world!", Is.StringEnding("world!"));
            Assert.That("Hello world!", Is.Not.StringEnding("world?"));
            Assert.That("Hello world!", Has.Length.GreaterThan(10).And.Not.EndsWith("world?"));

            Assert.That("Hello world!", Is.EqualTo("hello WORLD!").IgnoreCase);
            Assert.That("Hello world!", Is.Not.EqualTo("hello WORD!").IgnoreCase);

            Assert.That("Hello world!", Is.StringMatching(@"\w*\s\w*"));
            Assert.That("Hello world!", Is.Not.StringMatching(@"\w\s\s\w"));
            Assert.That("Hello world!", Has.Length.GreaterThan(10).And.Not.Matches(@"\w\s\s\w"));
        }

        [Test]
        public void CollectionConstraintsTest()
        {
            Assert.That(new int[] { }, Is.Empty, "Collection is empty");
            Assert.That(new [] { 1, 2, 3 }, Is.Not.Empty, "Collection is not empty");

            var ints = new object[] { 4, 3, 2, 1 };

            Assert.That(ints, Is.All.Not.Null, "All items in array are not null");
            Assert.That(ints, Is.All.InstanceOf(typeof(int)), "All items in array are int numbers");
            Assert.That(ints, Is.All.InstanceOf<int>(), "All items in array are int numbers");
            Assert.That(ints, Is.All.GreaterThan(0), "All items in array are greater than 0");
            Assert.That(ints, Is.Unique, "All items in array are unique");

            Assert.That(ints, Has.Some.GreaterThan(2), "Some items in array are greater than 2");
            Assert.That(ints, Has.Some.LessThanOrEqualTo(3), "Some items in array are less than or equal to 3");

            Assert.That(ints, Has.Exactly(2).GreaterThan(2), "Exactly 2 items in array are greater than 2");
            Assert.That(ints, Has.Exactly(3).LessThanOrEqualTo(3), "Exactly 3 items in array are less than or equal to 3");

            Assert.That(ints, Has.No.Null, "No items in array are null");
            Assert.That(ints, Has.None.Null, "None items in array are null");
            Assert.That(ints, Has.None.GreaterThan(4), "None items in array are greater than 4");
            Assert.That(ints, Has.None.LessThanOrEqualTo(0), "None items in array are less than or equal to 0");

            Assert.That(ints, Has.Member(3), "Array has member with value 3");
            Assert.That(ints, Has.No.Member(0), "Array hasn't member with value 0");
            Assert.That(ints, Contains.Item(3), "Array contains member with value 3");

            var ints1 = new object[] { 1, 2, 3, 4 };
            var ints2 = new object[] { 2, 3, 4, 5 };
            var ints3 = ints1;

            Assert.That(ints1, Is.EqualTo(ints3), "Two references are pointed to the same array");
            Assert.That(ints1, Is.Not.EqualTo(ints2), "Two references are pointed to the different arrays");

            Assert.That(ints, Is.EquivalentTo(ints1), "Two arrays are equivalent");
            Assert.That(ints, Is.Not.EqualTo(ints2), "Two arrays are not equivalent");

            Assert.That(new [] { 2, 3 }, Is.SubsetOf(ints), "Int array contains set [2, 3]");
            Assert.That(new [] { 1, 2, 3, 4 }, Is.SubsetOf(ints), "Int array contains set [1, 4]");
            Assert.That(new [] { 4, 5 }, Is.Not.SubsetOf(ints), "Int array does not contain set [4, 5]");
            Assert.That(new [] { 0, 1, 2, 3, 4, 5 }, Is.Not.SubsetOf(ints), "Int array does not contain set [0, 5]");

            Assert.That(new [] { 1, 2, 3, 4 }, Is.Ordered, "Array of int numbers is ordered");
            Assert.That(new [] { 1, 3, 2, 4 }, Is.Not.Ordered, "Array of int numbers is not ordered");
            Assert.That(new [] { 4, 3, 2, 1 }, Is.Ordered.Descending, "Array of int numbers is ordered descending");
            Assert.That(new [] { "a", "ab", "abc", "abcd" }, Is.Ordered.By("Length"), "Array of strings is ordered by property Length");
        }

        [Test]
        public void PropertyConstraintsTest()
        {
            Assert.That("test", Has.Length, "String has property Lenght");
            Assert.That("test", Has.Property("Length"), "String has property Lenght");
            Assert.That("test", Has.Property("Length").EqualTo(4), "String has property Lenght equal to 4");

            var ints = new List<int> { 4, 3, 2, 1 };

            Assert.That(ints, Has.Count, "List has property Count");
            Assert.That(ints, Has.Property("Count"), "List has property Count");
            Assert.That(ints, Has.Property("Count").GreaterThan(2), "List has property Count greater than 2");
        }

        [Test]
        public void CompoundConstraintsTest()
        {
            Assert.That(0, Is.Not.GreaterThan(1), "0 is not greater than 1");
            Assert.That(new[] { 4, 3, 2, 1 }, Is.All.GreaterThan(0), "All array's items are greater than 0");
            Assert.That(1, Is.GreaterThan(0) & Is.LessThan(2), "1 is greater than 0 and less than 2");
            Assert.That(1, Is.GreaterThan(0) | Is.LessThan(0), "1 is greater or less than 0");
        }

        [Test]
        public void PathConstraintsTest()
        {
            Assert.That("/folder1/./junk/../folder2", Is.SamePath("/folder1/folder2"), "Two paths are same");
            Assert.That("/folder1/./junk/../folder2/x", Is.Not.SamePath("/folder1/folder2"), "Two paths are not same");

            Assert.That(@"C:\folder1\folder2", Is.SamePath(@"C:\Folder1\Folder2").IgnoreCase, "Two paths are same ignoring case");
            Assert.That("/folder1/folder2", Is.Not.SamePath("/Folder1/Folder2").RespectCase, "Two paths are not same respecting case");

            Assert.That("/folder1/./junk/../folder2/folder3", Is.SubPath("/folder1/folder2"), "One path is under under another one");
            Assert.That("/folder1/junk/folder2/folder3", Is.Not.SubPath("/folder1/folder2"), "One path is not under under another one");

            Assert.That(@"C:\folder1\folder2\folder3\folder4", Is.SubPath(@"C:\Folder1\Folder2/Folder3").IgnoreCase, "One path is under under another one ignoring case");
            Assert.That("/folder1/folder2/folder3/folder4", Is.Not.SubPath("/Folder1/Folder2/Folder3").RespectCase, "One path is not under under another one respecting case");

            Assert.That("/folder1/./junk/../folder2", Is.SamePathOrUnder("/folder1/folder2"), "Two paths are same or under");
            Assert.That("/folder1/junk/../folder2/./folder3", Is.SamePathOrUnder("/folder1/folder2"), "Two paths are same or under");
            Assert.That("/folder1/junk/folder2/folder3", Is.Not.SamePathOrUnder("/folder1/folder2"), "Two paths are not same or under");

            Assert.That(@"C:\folder1\folder2\folder3", Is.SamePathOrUnder(@"C:\Folder1\Folder2").IgnoreCase, "Two paths are same or under ignoring case");
            Assert.That("/folder1/folder2/folder3", Is.Not.SamePathOrUnder("/Folder1/Folder2").RespectCase, "Two paths are not same or under respecting case");
        }

        [Test]
        public void DelayedConstraintsTest()
        {
            int counter;

            var task = new Action(() =>
            {
                Thread.Sleep(1000);
                for (var i = 0; i < 1000; i++)
                    counter = i;

            });

            counter = 0;
            Task.Factory.StartNew(task);
            Assert.That(ref counter, Is.GreaterThan(100).After(2000), "Counter should be increased in two seconds");

            counter = 0;
            Task.Factory.StartNew(task);
            Assert.That(() => counter, Is.GreaterThan(100).After(2000, 100), "Counter should be increased in two seconds (check using polling interval in 100ms)");
        }

        [Test]
        public void ReusableConstraintsTest()
        {
            ReusableConstraint myConstraintNotNull = Is.Not.Null;
            ReusableConstraint myConstraintNotEmpty = Is.Not.Empty;
            ReusableConstraint myConstraint = myConstraintNotNull.Resolve() & myConstraintNotEmpty.Resolve();

            Assert.That("Not a null and empty", myConstraint);
        }

        [Test]
        public void ListMapperTest()
        {
            var strings = new [] { "a", "ab", "abc" };
            var lengths = new [] { 1, 2, 3 };

            Assert.That(List.Map(strings).Property("Length"), Is.EqualTo(lengths), "Map property Length from string array to array list");
        }
    }
}
