using System;

namespace Provausio.Testing.Generators.Generators.Numbers
{
    public class IntegerProvider : IGenerateData
    {
        private readonly int _min;
        private readonly int _max;
        private readonly IntegerGenerator _generator;

        public Type Type => typeof(int);

        public IntegerProvider(int min = int.MinValue, int max = int.MaxValue)
        {
            _min = min;
            _max = max;
            _generator = new IntegerGenerator();
        }

        public object Generate()
        {
            return _generator.Generate(_min, _max);
        }
    }
}
