using System;

namespace Provausio.Testing.Generators.Generators.IDs
{
    public class IdProvider : IGenerateData
    {
        private readonly IdType _idType;
        private readonly IdGenerator _generator;

        public Type Type => typeof(string);

        public IdProvider(IdType idType)
        {
            _idType = idType;
            _generator = new IdGenerator();
        }

        public object Generate()
        {
            return _generator.Generate(_idType);
        }
    }
}
