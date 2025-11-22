using System;
using System.Collections.Generic;
using Combat.Projectiles.Behaviours;

public static class BehaviourTypeExtensions
{
    public static bool Has(this BehaviourType behaviours, BehaviourType flag)
    {
        return (behaviours & flag) != 0;
    }

    public static BehaviourType Add(this BehaviourType behaviours, BehaviourType flag)
    {
        return behaviours | flag;
    }

    public static BehaviourType Remove(this BehaviourType behaviours, BehaviourType flag)
    {
        return behaviours & ~flag;
    }

    public static IEnumerator<int> GetEnumerator(this BehaviourType behaviours)
    {
        ulong bits = Convert.ToUInt64(behaviours);
        int index = 0;

        while (bits != 0)
        {
            if ((bits & 1UL) != 0)
                yield return index;

            bits >>= 1;
            index++;
        }
    }

    public static int Index(this BehaviourType behaviours)
    {
        int value = (int)behaviours;
        int index = 0;

        while (value > 1)
        {
            value >>= 1;
            index++;
        }

        return index;
    }

    public static BehaviourType Get(this BehaviourType type, int index) => (BehaviourType)(1 << index);

    public static string ToNiceString(this BehaviourType behaviours)
    {
        List<string> names = new();

        foreach (int index in behaviours)
        {
            BehaviourType flag = behaviours.Get(index);
            names.Add(flag.ToString());
        }

        return string.Join(", ", names);
    }
}