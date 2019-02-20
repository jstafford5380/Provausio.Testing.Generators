using System;

namespace Provausio.Testing.Generators.Shared.Ext
{
    public static class ArrayEx
    {
        public static void Shuffle<T>(this T[] arr, Random rand)
        {
            int n = arr.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + rand.Next(n - i);
                T t = arr[r];
                arr[r] = arr[i];
                arr[i] = t;
            }
        }
    }
}
