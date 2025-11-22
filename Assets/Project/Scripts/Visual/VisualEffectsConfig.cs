using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualEffects
{
    [CreateAssetMenu(fileName = "VisualConfig", menuName = "Scriptables/Visual/Effects")]
    public class WeaponConfigVisualEffects : ScriptableObject
    {
        [field: SerializeField] public List<EffectWeaponKVP> Effects { get; private set; }
    }

    [Serializable]
    public class EffectWeaponKVP
    {
        [field: SerializeField] public EffectType Type { get; private set; }
        [field: SerializeField] public List<ParticleSystem> Prefabs { get; private set; }
        [field: SerializeField] public int Limit { get; private set; }
        public ParticleSystem this[int index] { get {
                if (index < 0 || index >= Prefabs.Count)
                    index = UnityEngine.Random.Range(0, Prefabs.Count);

                return Prefabs[index];
            }
        }
    }
}
