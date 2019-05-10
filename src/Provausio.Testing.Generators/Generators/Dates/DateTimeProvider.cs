using System;

namespace Provausio.Testing.Generators.Generators.Dates
{
    public class DateTimeProvider : IGenerateData
    {
        private readonly DateTimeGenerator _generator;

        public Type Type => typeof(DateTime);

        public DateTimeProvider()
        {
            _generator = new DateTimeGenerator();
        }

        public object Generate()
        {
            return _generator.Generate();
        }
    }
}
