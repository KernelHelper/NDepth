namespace NDepth.Common.Interfaces
{
    /// <summary>
    /// Generic interface to perform clone operation on different types.
    /// </summary>
    public interface ICloneable<out T> where T : ICloneable<T>
    {
        /// <summary>
        /// Clone the current instance.
        /// </summary>
        /// <returns>Clone of the current instance</returns> 
        T Clone();
    }     
}
