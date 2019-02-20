using System;
using System.Collections.Generic;

namespace Provausio.Testing.Generators.Generators.Names
{
    internal class NameGenerator
    {
        private readonly Random _random = new Random();

        public IEnumerable<string> Generate(int max, NameType type = NameType.Both, Gender gender = Gender.Both) 
            => YieldUntil(max, () => Generate(type, gender));

        public IEnumerable<string> GenerateFull(int max, Gender gender = Gender.Both)
            => YieldUntil(max, () => GenerateFull(gender));

        public string Generate(NameType type = NameType.Both, Gender gender = Gender.Both)
        {
            string name;
            switch (type)
            {
                case NameType.Given:
                    name = GenerateGiven(gender);
                    break;
                case NameType.Surname:
                    name = GenerateSurname();
                    break;
                case NameType.Both:
                    name = GenerateFull(gender);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender));
            }

            return name;
        }

        public string GenerateFull(Gender gender)
        {
            var g = gender == Gender.Both
                ? _random.Next() % 2 == 0
                    ? Gender.Male
                    : Gender.Female
                : gender;

            return $"{GenerateGiven(g)} {GenerateSurname()}";
        }

        private string GenerateGiven(Gender gender = Gender.Both)
        {
            var g = gender == Gender.Both
                ? _random.Next() % 2 == 0
                    ? Gender.Male
                    : Gender.Female
                : gender;

            string name;
            switch (g)
            {
                case Gender.Male:
                    name = NameSource.GivenMale[_random.Next(0, NameSource.GivenMale.Length - 1)];
                    break;
                case Gender.Female:
                    name = NameSource.GivenFemale[_random.Next(0, NameSource.GivenFemale.Length - 1)];
                    break;
                case Gender.Both: // this should be prevented by the random generator above
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender));
            }

            return name;
        }

        public string GenerateUsername()
        {
            return ToUsername(
                GenerateGiven(),
                GenerateSurname());
        }

        public string ToUsername(string givenName, string surname)
        {
            return $"{givenName[0]}{surname.Trim()}".ToLower();
        }

        private string GenerateSurname()
        {
            return NameSource.Surnames[_random.Next(0, NameSource.Surnames.Length - 1)];
        }

        private IEnumerable<string> YieldUntil(int max, Func<string> yield)
        {
            if (max == -1)
            {
                while (true)
                    yield return yield();
            }
            else
            {
                for (var i = 0; i < max; i++)
                    yield return yield();
            }
        }
    }

    public enum Gender
    {
        NotApplicable,
        Male,
        Female,
        Both
    }

    public enum NameType
    {
        Given,
        Surname,
        Both
    }
}
