using Provausio.Testing.Generators.Generators.Strings;
using Xunit;

namespace Provausio.Testing.Generators.Tests.AcceptanceTests
{
    public class WhenGeneratingWords
    {
        private WordProvider _generator;

        public WhenGeneratingWords()
        {
            _generator = new WordProvider();
        }

        [Fact]
        public void Generate_GeneratesUniqueWords()
        {
            // arrange

            // act
            var word1 = (string) _generator.Generate();
            var word2 = (string) _generator.Generate();

            // assert
            Assert.NotEqual(word1, word2);
        }
    }
}
