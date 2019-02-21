using System;
using System.Collections.Generic;

namespace Provausio.Testing.Generators.Shared
{
    /// <summary>
    /// Provides an abstraction for stepping over an iterator one by one without the need to manage an instance of <see cref="IEnumerable{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IteratorStepper<T>
    {
        private readonly IEnumerator<T> _iterator;

        public IteratorStepper(IEnumerable<T> enumerable)
        {
            _iterator = enumerable.GetEnumerator();
        }

        /// <summary>
        /// Gets the next item in the iterator.
        /// </summary>
        /// <returns></returns>
        public T GetNext()
        {
            if (_iterator.MoveNext())
                return _iterator.Current;
            else throw new EndOfIteratorException();
        }
    }

    public class EndOfIteratorException : Exception
    {
        public EndOfIteratorException()
            : base("There are no more items in this iterator.") { }
    }
}
