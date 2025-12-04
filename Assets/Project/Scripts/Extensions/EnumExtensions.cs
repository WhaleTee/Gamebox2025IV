using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class EnumExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Has<T>(this T flags, T flag) where T : Enum
    => (Unsafe.As<T, ulong>(ref flags) & Unsafe.As<T, ulong>(ref flag)) != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Add<T>(this T flags, T flag) where T : Enum
    {
        var fs = Unsafe.As<T, ulong>(ref flags);
        var f = Unsafe.As<T, ulong>(ref flag);

        ulong result = fs | f;
        return Unsafe.As<ulong, T>(ref result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Remove<T>(this T flags, T flag) where T : Enum
    {
        var fs = Unsafe.As<T, ulong>(ref flags);
        var f = Unsafe.As<T, ulong>(ref flag);

        ulong result = fs & ~f;
        return Unsafe.As<ulong, T>(ref result);
    }

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Get<T>(this T flags, int index) where T : Enum
    {
        ulong bit = 1UL << index;
        return Unsafe.As<ulong, T>(ref bit);
    }


    public static string ToNiceString<T>(this T flags) where T: Enum, IEnumerable<int>
        => string.Join(", ", flags.GetEnumerator());
}