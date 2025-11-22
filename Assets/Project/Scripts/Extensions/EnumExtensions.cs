using System;
using System.Collections.Generic;

public static class EnumExtensions
{
    public static bool Has<T>(this T flags, T flag) where T : Enum
    => (Convert.ToUInt64(flags) & Convert.ToUInt64(flag)) != 0;

    public static T Add<T>(this T flags, T flag) where T : Enum
    => (T)Enum.ToObject(typeof(T),
        Convert.ToUInt64(flags) | Convert.ToUInt64(flag));

    public static T Remove<T>(this T flags, T flag) where T : Enum
    => (T)Enum.ToObject(typeof(T),
        Convert.ToUInt64(flags) & ~Convert.ToUInt64(flag));

    public static IEnumerator<T> GetEnumerator<T>(this T flags) where T : Enum
    {
        ulong bits = Convert.ToUInt64(flags);
        ulong mask = 1;

        while (bits != 0)
        {
            if ((bits & mask) != 0)
                yield return (T)Enum.ToObject(typeof(T), mask);

            bits &= ~mask;
            mask <<= 1;
        }
    }

    public static int Index<T>(this T flags) where T : Enum
    {
        int value = Convert.ToInt32(flags);
        int index = 0;

        while (value > 1)
        {
            value >>= 1;
            index++;
        }

        return index;
    }

    public static T Get<T>(this T flags, int index) where T : Enum
     => (T)Enum.ToObject(typeof(T), Enum.GetUnderlyingType(typeof(T)) switch
     {
         Type t when t == typeof(int) => 1 << index,
         Type t when t == typeof(uint) => (uint)1 << index,
         Type t when t == typeof(long) => 1L << index,
         Type t when t == typeof(ulong) => 1UL << index,
         _ => throw new NotSupportedException("Enum must be int/uint/long/ulong")
     });


    public static string ToNiceString<T>(this T flags) where T: Enum, IEnumerable<int>
        => string.Join(", ", flags.GetEnumerator());
}