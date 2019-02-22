using System;

namespace Provausio.Testing.Generators.Generators.Strings
{
    public class SentenceProvider : IGenerateData
    {        
        private readonly SentenceGenerator _generator;

        public Type Type => typeof(string);

        public SentenceProvider()
        {            
            _generator = new SentenceGenerator();
        }

        public object Generate()
        {
            return _generator.GenerateSentence();
        }
    }
}
