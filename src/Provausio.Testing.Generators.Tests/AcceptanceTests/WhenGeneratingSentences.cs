using Provausio.Testing.Generators.Generators.Strings;
using Xunit;

namespace Provausio.Testing.Generators.Tests.AcceptanceTests
{
    public class WhenGeneratingSentences
    {
        private SentenceProvider _generator;

        public WhenGeneratingSentences()
        {
            _generator = new SentenceProvider();
        }

        [Fact]
        public void Generate_GeneratesUniqueSentences()
        {
            // arrange

            // act
            var sentence1 = (string)_generator.Generate();
            var sentence2 = (string)_generator.Generate();

            // assert
            Assert.True(sentence1.Split(" ").Length > 1);
            Assert.True(sentence2.Split(" ").Length > 1);
            Assert.NotEqual(sentence1, sentence2);
        }
    }
}
