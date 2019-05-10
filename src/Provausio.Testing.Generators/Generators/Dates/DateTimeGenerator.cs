using System;
using System.Collections.Generic;

namespace Provausio.Testing.Generators.Generators.Dates
{
    public class DateTimeGenerator
    {
        private readonly Random _random;

        public DateTimeGenerator()
        {
            _random = new Random();
        }

        public IEnumerable<DateTime> Generate()
        {
            var start = new DateTime(1995, 1, 1);
            var range = (DateTime.Today - start).Days;
            yield return start.AddDays(_random.Next(range));
        }
    }
}
