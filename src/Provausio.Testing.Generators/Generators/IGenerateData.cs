using System;

namespace Provausio.Testing.Generators.Generators
{
    /// <summary>
    /// When implemented, provides a generator for data.
    /// </summary>
    public interface IGenerateData
    {
        /// <summary>
        /// The <see cref="Type"/> returned by the Generate method.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Generates the data.
        /// </summary>
        /// <returns></returns>
        object Generate();
    }
}
