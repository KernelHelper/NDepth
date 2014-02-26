using NUnit.Framework.Constraints;

namespace NUnit.Framework.ExtensionMethods
{
    /// <summary>
    /// Extension methods to make working with NUnit in C# 3 a little more DSL-like
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="actual">The actual value to test</param>
        /// <param name="constraint">A Constraint to be applied</param>
        public static void Should(this object actual, Constraint constraint)
        {
            Assert.That(actual, constraint);
        }

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="actual">The actual value to test</param>
        /// <param name="constraint">A Constraint to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        public static void Should(this object actual, Constraint constraint, string message)
        {
            Assert.That(actual, constraint, message);
        }

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="actual">The actual value to test</param>
        /// <param name="constraint">A Constraint to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void Should(this object actual, Constraint constraint, string message, params object[] args)
        {
            Assert.That(actual, constraint, message, args);
        }
    }

    /// <summary>
    /// The Be class is a synonym for Is intended for use with the Should extension methods for more DSL-like syntax
    /// </summary>
    public class Be : Is { }

    /// <summary>
    /// The Have class is a synonym for Has intended for use with the Should extension methods for more DSL-like syntax
    /// </summary>
    public class Have : Has { }
}
