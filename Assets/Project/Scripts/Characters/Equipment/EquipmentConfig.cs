using UnityEngine;

namespace Characters.Equipment
{
    public abstract class EquipmentConfig<TStats> : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; protected set; }
        [field: SerializeField] public TStats Stats { get; protected set; }
        [field: SerializeField] public string Name { get; protected set; }
    }
}
