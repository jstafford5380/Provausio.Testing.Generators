using Provausio.Testing.Generators.Generators.Numbers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Provausio.Testing.Generators.Tests.Numbers
{
    public class RationalNumberTests
    {
        private RationalNumberGenerator _gen;

        public RationalNumberTests()
        {
            _gen = new RationalNumberGenerator();
        }

        [Fact]
        public void GenerateDouble_GeneratesDouble()
        {
            // arrange

            // act
            var d = _gen.Generate<double>(1, 1000, RationalType.Double);

            // assert
            Assert.IsType<double>(d);
            Assert.True(d >= 1 && d <= 1000);
        }

        [Fact]
        public void GenerateDecimal_GeneratesDecimal()
        {
            // arrange

            // act
            var d = _gen.Generate<decimal>(1, 1000, RationalType.Decimal);

            // assert
            Assert.IsType<decimal>(d);
        }

        [Fact]
        public void GenerateMoney_OnlyTwoDecimalPlaces()
        {
            // arrange

            // act
            var d = _gen.Generate<decimal>(1, 1000, RationalType.Money);

            // assert
            var split = d.ToString().Split(".");
            Assert.True(split[1].Length < 3);
        }
    }
}
