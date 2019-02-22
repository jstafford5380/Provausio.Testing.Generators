using System;
using System.Linq;
using Newtonsoft.Json;
using Provausio.Testing.Generators.Generators;
using Provausio.Testing.Generators.Generators.IDs;
using Provausio.Testing.Generators.Generators.Names;
using Provausio.Testing.Generators.Generators.Numbers;
using Xunit;

namespace Provausio.Testing.Generators.Tests.AcceptanceTests
{
    public class Usage
    {
        //[Fact]
        public void AsJson()
        {
            var filler = new ObjectFill<Employee>()
                .For(p => p.FirstName, new StaticNameProvider("Peter"));

            var objects = filler.Generate(5).ToList();
            var json = JsonConvert.SerializeObject(objects);
        }

        [Fact]
        public void Using_a_specific_provider()
        {
            // arrange
            var configuration = new ObjectFill<TestClassA>()
                .For(p => p.Firstname, new NameProvider(NameType.Given, Gender.Both))
                .For(p => p.LastName, new NameProvider(NameType.Surname, Gender.NotApplicable))
                .For(p => p.HourlyRate, new MoneyProvider(1, 165));

            // act
            var result = configuration.Generate(10).ToList();

            // assert
            Assert.Equal(10, result.Count());
        }

        [Fact]
        public void Using_customprovider()
        {
            // arrange
            var configuration = new ObjectFill<TestClassA>()
                .For(p => p.Id, new CustomGenerator<string>(() => "Im an Id!"))
                .For(p => p.Firstname, new CustomGenerator<string>(() => "Jon"))
                .For(p => p.LastName, new CustomGenerator<string>(() => "Snuh"))
                .For(p => p.HourlyRate, new CustomGenerator<decimal>(() => 1.09m));

            // act
            var result = configuration.Generate(10).ToList();

            // assert
            Assert.All(result, a =>
            {
                Assert.Equal("Im an Id!", a.Id);
                Assert.Equal("Jon", a.Firstname);
                Assert.Equal("Snuh", a.LastName);
                Assert.Equal(1.09m, a.HourlyRate);
            });
        }

        [Fact]
        public void Generate_FilledCorrectly()
        {
            // arrange
            var configuration = new ObjectFill<TestClassA>()
                .For(p => p.Id, new IdProvider(IdType.Base58))
                .For(p => p.Firstname, new NameProvider(NameType.Given, Gender.Male))
                .For(p => p.LastName, new NameProvider(NameType.Surname, Gender.NotApplicable))
                .For(p => p.HourlyRate, new MoneyProvider(1, 165));

            // act
            var result = configuration.Generate(10).ToList();

            // assert
            Assert.All(result, a =>
            {
                Assert.NotNull(a.Id);
                Assert.Contains(a.Firstname, NameSource.GivenMale);
                Assert.Contains(a.LastName, NameSource.Surnames);
            });
        }

        
    }

    public class TestClassA
    {
        public string Id { get; set; }

        public string Firstname { get; set; }

        public string LastName { get; set; }

        public decimal HourlyRate { get; set; }
    }

    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public decimal Rate { get; set; }
    }

    public class StaticNameProvider : IGenerateData
    {
        private string _name;

        public Type Type => typeof(string);

        public StaticNameProvider(string name)
        {
            _name = name;
        }

        public object Generate()
        {
            return _name;
        }
    }
}
