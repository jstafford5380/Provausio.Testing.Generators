using System;
using System.Collections.Generic;

namespace Provausio.Testing.Generators.Generators.Numbers
{
    public class IncrementedIntegerProvider : IGenerateData
    {
        private readonly IEnumerator<int> _generator;

        public Type Type => typeof(int);

        public IncrementedIntegerProvider(int startAt = 0, int max = GeneratorLimit.Default)
        {
            var gen = new IntegerGenerator();
            _generator = gen
                .GenerateIncremented(startAt, max)
                .GetEnumerator();
        }

        public object Generate()
        {
            _generator.MoveNext();
            return _generator.Current;
        }
    }
}
