using System;
using System.Collections.Generic;

namespace Provausio.Testing.Generators.Generators.Numbers
{
    public class IntegerGenerator
    {
        private readonly Random _rand = new Random();
        
        public int Generate(int min = int.MinValue, int max = int.MaxValue)
        {
            return _rand.Next(min, max);
        }
        
        public IEnumerable<int> GenerateIncremented(int startAt, int max)
        {
            var current = startAt;
            for (int i = 0; i < max; i++)
            {
                yield return current;
                current++;
            }
        }
    }
}
