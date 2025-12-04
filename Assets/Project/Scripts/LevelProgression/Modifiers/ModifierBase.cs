using UnityEngine;

namespace LevelProgression
{
    public abstract class ModifierBase : ScriptableObject
    {
        [field: SerializeField] public string Name { get; protected set; }

        public abstract void ApplyTo(PlayerAbilities abilities);
        public abstract void RemoveFrom(PlayerAbilities abilities);
    }
}