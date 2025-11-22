using Characters.Equipment;
using UnityEngine;
using VisualEffects;

namespace Combat.Weapon
{
    public abstract class WeaponConfig<TStats> : EquipmentConfig<TStats>
    {
        [field: SerializeField] public WeaponConfigAudio Audio { get; protected set; }
        [field: SerializeField] public WeaponConfigVisualEffects VisualEffects { get; protected set; }
    }
}