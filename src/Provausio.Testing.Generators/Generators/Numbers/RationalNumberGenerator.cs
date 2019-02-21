using System;

namespace Provausio.Testing.Generators.Generators.Numbers
{
    internal class RationalNumberGenerator
    {
        private readonly Random _rand = new Random();

        public T Generate<T>(int min, int max, RationalType treatAsType = RationalType.Decimal)
        {
            T val;
            var number = Generate(min, max);

            if (treatAsType == RationalType.Money)
            {
                var money = (decimal) Convert.ChangeType(number, typeof(decimal));
                val = (T) Convert.ChangeType(Math.Round(money, 2), typeof(T));
            }
            else val = (T) Convert.ChangeType(number, typeof(T));

            return val;
        }

        private object Generate(int min = 1, int max = 10000) => _rand.Next(min, max) + _rand.NextDouble();
    }

    public enum RationalType
    {
        Float,
        Double,
        Decimal,
        Money
    }
}
