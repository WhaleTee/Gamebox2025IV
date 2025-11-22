using System;

namespace Combat
{
    [Serializable]
    public struct Damage
    {
        public int amount;
        public DamageType Type;
    }
}