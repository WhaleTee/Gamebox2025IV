using UnityEngine;

namespace Puzzle.Entities
{
    [CreateAssetMenu(menuName = "Scriptables/Puzzle/Entities/Effects/Animation/Crystal")]
    public class CrystalAnimationConfig : PuzzlePieceAnimationConfig
    {
        [field: SerializeField] public Sprite Intact { get; private set; }
        [field: SerializeField] public Sprite Damaged { get; private set; }
        [field: SerializeField] public Sprite StronglyDamaged { get; private set; }
        [field: SerializeField] public Sprite Broken { get; private set; }
        [field: SerializeField] public Sprite Charged { get; private set; }
        [field: SerializeField] public Sprite ChargedDamaged { get; private set; }
        [field: SerializeField] public Sprite ChargedStronglyDamaged { get; private set; }

        [field: SerializeField] public float Duration { get; private set; } = 0.2f;
    }
}