using System;
using System.Collections.Generic;
using Provausio.Testing.Generators.Generators;
using Provausio.Testing.Generators.Generators.Dates;
using Provausio.Testing.Generators.Generators.Numbers;
using Provausio.Testing.Generators.Generators.Strings;

namespace Provausio.Testing.Generators
{

    /// <summary>
    /// Type provider factory.
    /// </summary>
    public static class It
    {
        private static readonly Dictionary<Type, Func<IGenerateData>> ProviderFactory;

        static It()
        {
            ProviderFactory = new Dictionary<Type, Func<IGenerateData>>
            {
                [typeof(int)]            = () => new IntegerProvider(),
                [typeof(string)]         = () => new WordProvider(),
                [typeof(DateTime)]       = () => new DateTimeProvider(),
                [typeof(DateTimeOffset)] = () => new DateTimeProvider(),
                [typeof(decimal)]        = () => new CustomGenerator<decimal>(() => new RationalNumberGenerator().Generate<decimal>(int.MinValue, int.MaxValue, RationalType.Decimal)),
                [typeof(double)]         = () => new CustomGenerator<double>(() => new RationalNumberGenerator().Generate<double>(int.MinValue, int.MaxValue, RationalType.Double))
            };
        }

        /// <summary>
        /// Adds the provider to the internal factory.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="generator">The generator.</param>
        public static void AddProvider(Type propertyType, Func<IGenerateData> generator)
        {
            if (ProviderFactory.ContainsKey(propertyType))
                return;

            ProviderFactory.Add(propertyType, generator);
        }

        /// <summary>
        /// Uses provider factory to create a new generator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="throwIfNotFound">When true, an exception will be thrown if the type is not registered.</param>
        /// <returns></returns>
        public static IGenerateData Is<T>(bool throwIfNotFound = false) => Is(typeof(T), throwIfNotFound);

        /// <summary>
        /// Uses provider factory to create a new generator.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="throwIfNotFound">When true, an exception will be thrown if the type is not registered.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">There is no registered type for {type}</exception>
        public static IGenerateData Is(Type type, bool throwIfNotFound = false)
        {
            if (ProviderFactory.ContainsKey(type))
                return ProviderFactory[type]();

            if(throwIfNotFound)
                throw new InvalidOperationException($"There is no registered type for {type}. You can add one to the factory with It.AddProvider(...)");

            return null;
        }
    }
}
