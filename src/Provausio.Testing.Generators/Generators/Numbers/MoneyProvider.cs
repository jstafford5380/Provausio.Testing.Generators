using System;
using Provausio.Practices.Validation.Assertion;

namespace Provausio.Testing.Generators.Generators.Numbers
{
    public class MoneyProvider : IGenerateData
    {
        private readonly int _min;
        private readonly int _max;
        private readonly RationalNumberGenerator _generator;

        public Type Type => typeof(decimal);

        public MoneyProvider(int min, int max)
        {
            Ensure.That(min > -1, "Min must be a positive integer.");
            Ensure.That(max > -1, "Max must be a positive integer.");

            _min = min;
            _max = max;
            _generator = new RationalNumberGenerator();
        }

        public object Generate()
        {
            return _generator.Generate<decimal>(_min, _max, RationalType.Money);
        }
    }
}
