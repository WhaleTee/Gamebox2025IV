using DG.Tweening;
using UnityEngine;

namespace Puzzle.Entities
{
    [CreateAssetMenu(menuName = "Scriptables/Puzzle/Entities/Effects/Animation/PolyCrystal")]
    public class PolyCrystalAnimationConfig : PuzzlePieceAnimationConfig
    {
        [field: SerializeField] public Sprite NotActive { get; private set; }
        [field: SerializeField] public Sprite Activated { get; private set; }

        [field: SerializeField] public float Duration { get; private set; } = 0.2f;
        [field: SerializeField] public Ease Ease { get; private set; }
    }
}
