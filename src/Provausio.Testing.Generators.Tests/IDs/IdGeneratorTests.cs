using Provausio.Testing.Generators.Generators.IDs;
using Xunit;

namespace Provausio.Testing.Generators.Tests.IDs
{
    public class IdGeneratorTests
    {
        [Fact]
        public void Generate_Guid_GeneratesGuid()
        {
            // arrange
            var gen = new IdGenerator();

            // act
            var guidId = gen.Generate(IdFormat.Guid);

            // assert
            Assert.Equal(36, guidId.Length);
        }

        [Fact]
        public void Generate_Xid_GeneratesXid()
        {
            // arrange
            var gen = new IdGenerator();

            // act
            var xid = gen.Generate(IdFormat.Xid);

            // assert
            Assert.Equal(20, xid.Length);
        }

        [Fact]
        public void Generate_Base58_GeneratesBase58()
        {
            // arrange
            var gen = new IdGenerator();

            // act
            var guidId = gen.Generate(IdFormat.Base58);

            // assert
            Assert.Equal(22, guidId.Length);
        }

        [Fact]
        public void Generate_Integer_GeneratesInteger()
        {
            // arrange
            var gen = new IdGenerator();

            // act
            var intId = gen.Generate(IdFormat.Integer);
            
            // assert
            Assert.True(int.TryParse(intId, out var i));
        }
    }
}
