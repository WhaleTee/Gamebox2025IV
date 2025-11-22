using System;
using UnityEngine;
using Characters.Equipment;

namespace Combat.Weapon
{
    [Serializable]
    public class WeaponStats : EquipmentStats
    {
        [field: SerializeField] public StrikeType StrikeType { get; protected set; }
        [field: SerializeField] public FireMode Mode { get; protected set; }
        [field: SerializeField] public Damage Damage { get; protected set; }
        [field: SerializeField] public float FireRate { get; protected set; } = 5f;
        public float FireInterval => 1f / FireRate;
        [field: SerializeField] public float Range { get; protected set; } = 30f;
        [field: SerializeField] public float SpreadAngle { get; protected set; } = 2f;
        [field: SerializeField] public float SpreadOffset { get; protected set; } = 0.02f;
    }
}
