using System;

namespace Combat.Projectiles.Behaviours
{
    [Flags]
    public enum BehaviourType
    {
        Sticky = 1 << 0,
        Luminous = 1 << 1,
        Piercing = 1 << 2,
        Ricochet = 1 << 3,
        Homing = 1 << 4,
        Boomerang = 1 << 5,
        Joint = 1 << 6,
        ChangeTarget = 1 << 7
    }
}
