using System;

namespace NDepth.Tests.UnitTests.NUnitTests
{
    public class ObjectWrapper : IComparable, IComparable<ObjectWrapper>
    {
        public int Value { get; private set; }

        public ObjectWrapper()
        {
            Value = 0;
        }

        public ObjectWrapper(int value)
        {
            Value = value;
        }

        #region Comparable members

        public int CompareTo(object obj)
        {
            if (!(obj is ObjectWrapper))
                throw new ArgumentException("Cannot compare to invalid object");

            return CompareTo(obj as ObjectWrapper);
        }

        public int CompareTo(ObjectWrapper other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "Cannot compare to null");

            if (Value < other.Value) return -1;
            if (Value == other.Value) return 0;
            return 1;
        }

        #endregion

        #region Equality members

        protected bool Equals(ObjectWrapper other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ObjectWrapper)obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static bool operator ==(ObjectWrapper left, ObjectWrapper right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ObjectWrapper left, ObjectWrapper right)
        {
            return !Equals(left, right);
        }

        #endregion
    }

    public class ObjectWrapperChild : ObjectWrapper
    {
        public ObjectWrapperChild() { }
        public ObjectWrapperChild(int value) : base(value) { }
    }
}
