using System;
using UnityEngine;

namespace Combat.Weapon
{
    [Serializable]
    public class WeaponStatsProjectile : WeaponStats
    {
        [Header("Projectiles")]
        [field: SerializeField] public Variant Variant { get; private set; }
        [field: SerializeField] public float ProjectileSpeed { get; private set; }
        [field: SerializeField] public float LifeTime { get; private set; }
    }
}