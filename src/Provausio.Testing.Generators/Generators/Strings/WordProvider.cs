using System;
using System.Linq;

namespace Provausio.Testing.Generators.Generators.Strings
{
    public class WordProvider : IGenerateData
    {        
        private readonly Random _random;

        public Type Type => typeof(string);

        public WordProvider()
        {
            _random = new Random();
        }

        public object Generate()
        {
            var words = WordGenerator.GenerateWords(_random, 1).ToList();
            return words.SingleOrDefault();
        }
    }
}
