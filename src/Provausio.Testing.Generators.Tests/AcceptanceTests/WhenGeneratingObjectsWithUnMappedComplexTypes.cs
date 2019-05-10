using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Provausio.Testing.Generators.Tests.AcceptanceTests
{
    public class WhenGeneratingObjectsWithUnMappedComplexTypes
    {
        [Fact]
        public void Test()
        {
            // arrange
            var filler = new ObjectFill<ObjectWithComplexProperties>();

            // act
            var results = filler.Generate(2);

            // assert

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
