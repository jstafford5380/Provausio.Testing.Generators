using Provausio.Testing.Generators.Generators.Strings;
using Xunit;

namespace Provausio.Testing.Generators.Tests.AcceptanceTests
{
    public class WhenGeneratingParagraphs
    {
        [Theory]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(1)]
        public void Generate_MultipleParagraphs_GenerateMultipleParagraphs(uint count)
        {
            // arrange
            var generator = new ParagraphProvider(count);

            // act
            var paragraphs = (string)generator.Generate();

            // assert
            var split = paragraphs.Split("\n\n");
            Assert.Equal((int) count, split.Length);
        }
    }
}
