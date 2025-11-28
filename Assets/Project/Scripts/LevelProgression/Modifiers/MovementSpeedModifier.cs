using UnityEngine;

namespace LevelProgression
{
    [CreateAssetMenu(menuName = "Scriptables/LevelProgression/Modifiers/MovementSpeed")]
    public class MovementSpeedModifier : ModifierBase, IStatModifier<float>
    {
        [field: SerializeField] public float Flat { get; private set; }
        [field: SerializeField] public float Multiplier { get; private set; }
        private float multiplyCache;

        public float Apply(float speed)
        {
            multiplyCache = speed * Multiplier;
            speed += Flat + multiplyCache;
            return speed;
        }
        public float Revert(float speed)
        {
            speed -= Flat + multiplyCache;
            return speed;
        }

        public override void ApplyTo(PlayerAbilities abilities) => abilities.AddModifier(this);
        public override void RemoveFrom(PlayerAbilities abilities) => abilities.RemoveModifier(this);
    }
}
