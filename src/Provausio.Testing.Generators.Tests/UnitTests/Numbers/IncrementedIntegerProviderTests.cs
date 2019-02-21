using Provausio.Testing.Generators.Generators;
using Provausio.Testing.Generators.Generators.Numbers;
using Xunit;

namespace Provausio.Testing.Generators.Tests.UnitTests.Numbers
{
    public class IncrementedIntegerProviderTests
    {
        [Fact]
        public void Generate_NumberIncrements()
        {
            // arrange
            var provider = new IncrementedIntegerProvider(1, GeneratorLimit.Default);
            
            // act
            var a = (int) provider.Generate();
            var b = (int) provider.Generate();
            var c = (int) provider.Generate();

            // assert   
            Assert.True(b == a + 1);
            Assert.True(c == b + 1);
        }
    }
}
