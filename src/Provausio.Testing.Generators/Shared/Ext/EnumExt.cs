using System;
using System.Linq;

namespace Provausio.Testing.Generators.Shared.Ext
{
    public static class EnumExtensions
    {
        public static Enum GetRandomEnumValue(this Type t)
        {
            return Enum.GetValues(t) // get values from Type provided
                .OfType<Enum>() // casts to Enum
                .OrderBy(e => Guid.NewGuid()) // mess with order of results
                .FirstOrDefault(); // take first item in result
        }

    }
}
