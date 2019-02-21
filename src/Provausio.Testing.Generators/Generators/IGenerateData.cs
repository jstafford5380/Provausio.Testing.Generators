using System;

namespace Provausio.Testing.Generators.Generators
{
    public interface IGenerateData
    {
        Type Type { get; }

        object Generate();
    }
}
