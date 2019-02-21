using System;

namespace Provausio.Testing.Generators.Generators.Names
{
    public class NameProvider : IGenerateData
    {
        private readonly Gender _gender;
        private readonly NameGenerator _generator;
        private readonly NameType _nameType;

        public Type Type => typeof(string);
        
        public NameProvider(NameType type = NameType.Both, Gender gender = Gender.Both)
        {
            _generator = new NameGenerator();
            _nameType = type;
            _gender = gender;
        }

        public object Generate()
        {
            return _generator.Generate(_nameType, _gender);
        }
    }
}
