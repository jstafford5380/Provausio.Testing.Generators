using System.Linq;
using Xunit;

namespace Provausio.Testing.Generators.Tests.AcceptanceTests
{
    public class WhenGeneratingObjectsWithUnMappedComplexTypes
    {
        [Fact]
        public void Test()
        {
            // arrange
            var filler = new ObjectFill<ObjectWithComplexProperties>().FillUnmappedProperties();

            // act
            var results = filler.Generate(2).ToList();

            // assert
            Assert.All(results, properties =>
            {
                Assert.NotNull(properties.Prop1);
                Assert.NotNull(properties.Prop2);
                Assert.NotNull(properties.Prop2.Foo);
                Assert.False(properties.Prop2.Bar == default);
            });
        }
    }

    public class ObjectWithComplexProperties
    {
        public string Prop1 { get; set; }

        public ComplexProperty Prop2 { get; set; }
    }

    public class ComplexProperty
    {
        public string Foo { get; set; }

        public int Bar { get; set; }
    }
}
