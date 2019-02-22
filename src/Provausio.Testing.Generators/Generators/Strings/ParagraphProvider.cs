using System;
using System.Linq;

namespace Provausio.Testing.Generators.Generators.Strings
{
    public class ParagraphProvider : IGenerateData
    {
        private readonly uint _count;
        private readonly SentenceGenerator _generator;
        private readonly uint _minSentences;
        private readonly uint _maxSentences;

        public Type Type => typeof(string);

        public ParagraphProvider(uint count, uint minSentences = 5, uint maxSentences = 10)
        {
            _count = count;
            _generator = new SentenceGenerator();
            _minSentences = minSentences;
            _maxSentences = maxSentences;
        }

        public object Generate()
        {
            var paragraphs = _generator
                .GenerateParagraphs(_minSentences, _maxSentences, _count)
                .ToList();

            return string.Join("\n\n", paragraphs);
        }
    }    
}
