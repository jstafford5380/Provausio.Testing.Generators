using System.Collections.Generic;
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
                Assert.Equal(2, properties.Prop3.Count);
            });
        }
    }

    public class ObjectWithComplexProperties
    {
        public string Prop1 { get; set; }

        public ComplexProperty Prop2 { get; set; }

        public List<ComplexProperty> Prop3 { get; set; }

        public TestEnum Prop4 { get; set; }
    }

    public enum TestEnum
    {
        One,
        Two,
        Three
    }

    public class ComplexProperty
    {
        public string Foo { get; set; }

        public int Bar { get; set; }
    }
}
