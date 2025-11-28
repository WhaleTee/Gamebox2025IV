using System.Collections.Generic;
using UnityEngine;

namespace LevelProgression
{
    [CreateAssetMenu(menuName = "Scriptables/LevelProgression/Modifiers/Damage")]
    public class DamageModifier : ModifierBase, IStatModifier<DamageBundle>
    {
        [field: SerializeField] public int AdditionalDamage { get; private set; }
        [field: SerializeField] public float DamageMultiplier { get; private set; } = 0f;
        [field: SerializeField] public DamageType Type { get; private set; }
        private Dictionary<DamageType, int> cachedMultipliedDamage = new()
        {
            { DamageType.Web, 0 },
            { DamageType.Stone, 0 },
            { DamageType.Plant, 0 },
            { DamageType.Metall, 0 }
        };

        public DamageBundle Apply(DamageBundle bundle)
        {
            foreach (var type in Type)
            {
                if (bundle.Has(type))
                {
                    cachedMultipliedDamage[type] = Mathf.FloorToInt(bundle.Damage[type] * DamageMultiplier);
                    int amount = cachedMultipliedDamage[type] + AdditionalDamage;
                    bundle.Damage[type] += amount;
                }
                else
                    bundle.Damage.Add(type, AdditionalDamage);
            }

            return bundle;
        }

        public DamageBundle Revert(DamageBundle bundle)
        {
            foreach (var type in Type)
            {
                int amount = cachedMultipliedDamage[type] + AdditionalDamage;
                if (bundle.Has(type))
                    bundle.Damage[type] -= amount;
            }

            return bundle;
        }

        public override void ApplyTo(PlayerAbilities abilities) => abilities.AddModifier(this);
        public override void RemoveFrom(PlayerAbilities abilities) => abilities.RemoveModifier(this);
    }
}
