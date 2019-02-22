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
                .For((System.Linq.Expressions.Expression<Func<Employee, object>>)(p => p.FirstName), new StaticNameProvider("Peter"));

            var objects = filler.Generate(5).ToList();
            var json = JsonConvert.SerializeObject(objects);
        }

        [Fact]
        public void Using_a_specific_provider()
        {
            // arrange
            var configuration = new ObjectFill<Employee>()
                .For(p => p.FirstName, new NameProvider(NameType.Given, Gender.Both))
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
            var configuration = new ObjectFill<Employee>()
                .For(p => p.Id, new CustomGenerator<string>(() => "Im an Id!"))
                .For(p => p.FirstName, new CustomGenerator<string>(() => "Jon"))
                .For(p => p.LastName, new CustomGenerator<string>(() => "Snuh"))
                .For(p => p.HourlyRate, new CustomGenerator<decimal>(() => 1.09m));

            // act
            var result = configuration.Generate(10).ToList();

            // assert
            Assert.All(result, a =>
            {
                Assert.Equal("Im an Id!", a.Id);
                Assert.Equal("Jon", a.FirstName);
                Assert.Equal("Snuh", a.LastName);
                Assert.Equal(1.09m, a.HourlyRate);
            });
        }

        [Fact]
        public void Generate_FilledCorrectly()
        {
            // arrange
            var configuration = new ObjectFill<Employee>()
                .For(p => p.Id, new IdProvider(IdType.Base58))
                .For(p => p.FirstName, new NameProvider(NameType.Given, Gender.Male))
                .For(p => p.LastName, new NameProvider(NameType.Surname, Gender.NotApplicable))
                .For(p => p.HourlyRate, new MoneyProvider(1, 165));

            // act
            var result = configuration.Generate(10).ToList();

            // assert
            Assert.All(result, a =>
            {
                Assert.NotNull(a.Id);
                Assert.Contains(a.FirstName, NameSource.GivenMale);
                Assert.Contains(a.LastName, NameSource.Surnames);
            });
        }

        [Fact]
        public void Generate_UsingCallback_FilledCorrectly()
        {
            // arrange
            var configuration = new ObjectFill<Employee>()
                .For(p => p.Id, new IdProvider(IdType.Base58))
                .For(p => p.FirstName, new NameProvider(NameType.Given, Gender.Male))
                .For(p => p.LastName, new NameProvider(NameType.Surname, Gender.NotApplicable))
                .For(p => p.HourlyRate, new MoneyProvider(1, 165))
                .For(p => p.Age, 
                     new IntegerProvider(18, 65), 
                     (target, generator) => 
                     {
                            var age = (int)generator.Generate();
                            target.SetAge(-age); // test by making the age negative
                     });

            // act
            var result = configuration.Generate(10).ToList();

            // assert
            Assert.All(result, a =>
            {
                Assert.True(a.Age < 0);
            });
        }
    }

    public class Employee
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal HourlyRate { get; set; }

        public int Age { get; private set; }

        public void SetAge(int age)
        {
            Age = age;
        }
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
