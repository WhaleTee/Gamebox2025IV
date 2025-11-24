using System;
using System.Collections.Generic;

public static class EnumExtensions
{
    public static bool Has<T>(this T flags, T flag) where T : Enum
        => ((int)(object)flags & (int)(object)flag) != 0;

    public static T Add<T>(this T flags, T flag) where T : Enum
        => (T)(object)(((int)(object)flags) | ((int)(object)flag));

    public static T Remove<T>(this T flags, T flag) where T : Enum
        => (T)(object)(((int)(object)flags) & ~((int)(object)flag));

    public static IEnumerator<T> GetEnumerator<T>(this T flags) where T : Enum
    {
        int bits = (int)(object)flags;
        int mask = 1;

        while (bits != 0)
        {
            if ((bits & mask) != 0)
                yield return (T)(object)mask;

            bits &= ~mask;
            mask <<= 1;
        }
    }

    public static int Index<T>(this T flags) where T : Enum
    {
        int value = (int)(object)flags;
        int index = 0;

        while (value > 1)
        {
            value >>= 1;
            index++;
        }

        return index;
    }

    public static T Get<T>(this T flags, int index) where T : Enum
        => (T)(object)(1 << index);
}
