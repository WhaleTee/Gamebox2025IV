using UnityEngine;

namespace LevelProgression
{
    [CreateAssetMenu(menuName = "Scriptables/LevelProgression/Modifiers/JumpsCount")]
    public class JumpModifier : ModifierBase, IStatModifier<int>
    {
        [field: SerializeField] public int AdditionalJumps { get; private set; }

        public int Apply(int jumps) => jumps + AdditionalJumps;
        public int Revert(int jumps) => jumps - AdditionalJumps;

        public override void ApplyTo(PlayerAbilities abilities) => abilities.AddModifier(this);
        public override void RemoveFrom(PlayerAbilities abilities) => abilities.RemoveModifier(this);
    }
}