using System;
using System.Collections.Generic;
using System.Linq;
using Provausio.Testing.Generators.Shared.Ext;

namespace Provausio.Testing.Generators.Generators.Strings
{
    internal class SentenceGenerator
    {
        private const int MinWords = 15;
        private const int MaxWords = 30;

        private readonly Random _random = new Random();

        /// <summary>
        /// Generates a single sentence.
        /// </summary>
        /// <returns></returns>
        public string GenerateSentence()
        {
            var sentenceLength = _random.Next(MinWords, MaxWords);
            var sentence = ToSentence(GenerateWords().Take(sentenceLength));
            return sentence;
        }

        /// <summary>
        /// Generates a single paragraph.
        /// </summary>
        /// <param name="minSentences">Minimum number of sentences in the paragraph.</param>
        /// <param name="maxSentences">Maximum number of sentences in the paragraph.</param>
        /// <returns></returns>
        public string GenerateParagraph(uint minSentences, uint maxSentences)
        {
            var length = _random.Next((int) minSentences, (int) maxSentences);

            var sentences = new List<string>();
            for (var i = 0; i < length; i++)
                sentences.Add(GenerateSentence());

            return string.Join(". ", sentences);
        }

        /// <summary>
        /// Generates many paragraphs.
        /// </summary>
        /// <param name="minSentences">Minimum number of sentences in the paragraph.</param>
        /// <param name="maxSentences">Maximum number of sentences in the paragraph.</param>
        /// <param name="max">Maximum number of sentences to generate. Setting this value prevents cases where an unbounded list of paragraphs are generated.</param>
        /// <returns></returns>
        public IEnumerable<string> GenerateParagraphs(uint minSentences, uint maxSentences, uint max = 20)
        {
            for (var i = 0; i < max; i++)
                yield return GenerateParagraph(minSentences, maxSentences);
        }

        private string ToSentence(IEnumerable<string> words)
        {
            var str = string.Join(" ", words).ToLower();
            return str.FirstToUpper();
        }

        private IEnumerable<string> GenerateWords()
        {
            // ensure no repeated words by not making this a random word pick
            var wordSoup = new string[WordGenerator.Words.Length];
            Array.Copy(WordGenerator.Words, wordSoup, WordGenerator.Words.Length);
            wordSoup.Shuffle(_random);

            var words = wordSoup.ToList();
            while (words.Any())
            {
                var word = words.FirstOrDefault();
                if(string.IsNullOrEmpty(word)) yield break;
                yield return word;
                words.Remove(word);
            }
        }
    }
}
