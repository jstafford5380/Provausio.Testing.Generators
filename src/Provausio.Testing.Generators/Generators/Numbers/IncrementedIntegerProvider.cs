using System;
using Provausio.Testing.Generators.Shared;

namespace Provausio.Testing.Generators.Generators.Numbers
{
    public class IncrementedIntegerProvider : IGenerateData
    {
        private readonly IteratorStepper<int> _generator;

        public Type Type => typeof(int);

        public IncrementedIntegerProvider(int startAt = 0, int max = GeneratorLimit.Default)
        {
            var gen = new IntegerGenerator();
            _generator = new IteratorStepper<int>(gen
                .GenerateIncremented(startAt, max));
        }

        public object Generate()
        {
            return _generator.GetNext();
        }
    }
}
