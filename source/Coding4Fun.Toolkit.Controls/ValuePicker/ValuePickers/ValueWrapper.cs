namespace Coding4Fun.Toolkit.Controls.Primitives
{
    /// <summary>
    /// Implements a wrapper for value that provides formatted strings for ValuePicker.
    /// </summary>
    public abstract class ValueWrapper<T> where T : struct
    {
        /// <summary>
        /// Gets the value being wrapped.
        /// </summary>
        public T Value { get; private set; }

        
        /// <summary>
        /// Initializes a new instance of the ValueWrapper class.
        /// </summary>
        /// <param name="value">Value to wrap.</param>
        protected ValueWrapper(T value)
        {
            Value = value;
        }
        
        /// <summary>
        /// Instanciates a derived ValueWrapper class
        /// </summary>
        /// <param name="value">the initial value</param>
        /// <returns>the instance created</returns>
        public abstract ValueWrapper<T> CreateNew(T? value);
    }
}
