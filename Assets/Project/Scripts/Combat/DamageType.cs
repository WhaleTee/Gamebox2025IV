using System;

[Flags]
public enum DamageType
{
    Web = 1 << 0,
    Stone = 1 << 1,
    Plant = 1 << 2,
    Metall = 1 << 3
}