using System;
using System.Linq;
using Provausio.Testing.Generators.Generators.Names;
using Xunit;

namespace Provausio.Testing.Generators.Tests.UnitTests.Names
{
    public class NameGeneratorTests
    {
        [Fact]
        public void Generate_MaleNames_GeneratesMaleNames()
        {
            // arrange
            var gen = new NameGenerator();

            // act
            var names = gen.Generate(5, NameType.Given, Gender.Male).ToList();

            // assert
            Assert.All(names, s => Assert.NotNull(NameSource.GivenMale.SingleOrDefault(n => n.Equals(s))));
        }

        [Fact]
        public void Generate_FemaleNames_GeneratesFemaleNames()
        {
            // arrange
            var gen = new NameGenerator();

            // act
            var names = gen.Generate(5, NameType.Given, Gender.Female).ToList();

            // assert
            Assert.All(names, s => Assert.NotNull(NameSource.GivenFemale.SingleOrDefault(n => n.Equals(s))));
        }

        [Fact]
        public void Generate_GivenName_BothGenders_ContainsMaleAndFemaleNames()
        {
            // arrange
            var gen = new NameGenerator();

            // act
            var names = gen.Generate(50, NameType.Given, Gender.Both).ToList();

            // assert
            var maleNames = names.Where(fn => NameSource.GivenMale.Contains(fn));
            var femaleNames = names.Where(fn => NameSource.GivenFemale.Contains(fn));
            
            Assert.True(maleNames.Any() && femaleNames.Any());
        }

        [Fact]
        public void Generate_BothNames_BothGenders_ContainsBothNames()
        {
            // arrange
            var gen = new NameGenerator();

            // act
            var names = gen.Generate(5, NameType.Both, Gender.Both).ToList();

            // assert
            Assert.All(names, s => Assert.Equal(2, s.Split(" ").Length));
        }

        [Fact]
        public void Generate_Surnames_GeneratesSurnames()
        {
            // arrange
            var gen = new NameGenerator();

            // act
            var names = gen.Generate(5, NameType.Surname, Gender.NotApplicable).ToList();

            // assert
            Assert.All(names, s => Assert.Contains(s, NameSource.Surnames));
        }

        [Fact]
        public void GenerateFull_GeneratesFirstAndLast()
        {
            // arrange
            var gen = new NameGenerator();

            // act
            var names = gen.GenerateFull(5, Gender.Both).ToList();

            // assert
            Assert.All(names, s =>
            {
                var split = s.Split(" ");
                Assert.Equal(2, split.Length);
                Assert.Contains(split[1], NameSource.Surnames);
            });
        }

        [Fact]
        public void GenerateFull_Male_FirstNameIsMale()
        {
            // arrange
            var gen = new NameGenerator();

            // act
            var names = gen.GenerateFull(5, Gender.Male).ToList();

            // assert
            Assert.All(names, s =>
            {
                var split = s.Split(" ");
                Assert.Contains(split[0], NameSource.GivenMale);
            });
        }

        [Fact]
        public void GenerateFull_Female_FirstNameIsFemale()
        {
            // arrange
            var gen = new NameGenerator();

            // act
            var names = gen.GenerateFull(5, Gender.Female).ToList();

            // assert
            Assert.All(names, s =>
            {
                var split = s.Split(" ");
                Assert.Contains(split[0], NameSource.GivenFemale);
            });
        }

        [Fact]
        public void GenerateFull_Both_ContainsBothMaleAndFemaleNames()
        {
            // arrange
            var gen = new NameGenerator();

            // act
            var names = gen.GenerateFull(50, Gender.Both).ToList();

            // assert
            var firstnames = names.Select(n => n.Split(" ")[0]);
            var maleNames = firstnames.Where(fn => NameSource.GivenMale.SingleOrDefault(n => n.Equals(fn)) != null);
            var femaleNames = firstnames.Where(fn => NameSource.GivenFemale.SingleOrDefault(n => n.Equals(fn)) != null);

            Assert.True(maleNames.Any() && femaleNames.Any());
        }

        [Fact]
        public void GenerateUsername_GeneratesUsername()
        {
            // arrange
            var gen = new NameGenerator();

            // act
            var username = gen.GenerateUsername();

            // assert
            var surname = username.Substring(1);
            Assert.Contains(surname, NameSource.Surnames, StringComparer.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData("Jon", "Snow", "jsnow")]
        [InlineData("Jeremy", "Stafford", "jstafford")]
        [InlineData("Bill", " Gates", "bgates")] // contains an extra space
        public void ToUserName_ConstructsProperUsername(string fname, string lname, string expected)
        {
            // arrange
            var gen = new NameGenerator();

            // act
            var username = gen.ToUsername(fname, lname);

            // assert
            Assert.Equal(expected, username);
        }
    }
}
