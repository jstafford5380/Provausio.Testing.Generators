namespace Provausio.Testing.Generators.Shared.Ext
{
    internal static class StringEx
    {
        public static string FirstToUpper(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}
