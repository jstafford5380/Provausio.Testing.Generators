using System;

namespace Provausio.Testing.Generators.Generators
{
    public class CustomGenerator<T> : IGenerateData
    {
        private readonly Func<T> _callback;

        public Type Type { get; }

        public CustomGenerator(Func<T> callback)
        {
            Type = typeof(T);
            _callback = callback;
        }

        public object Generate()
        {
            return _callback();
        }
    }
}
