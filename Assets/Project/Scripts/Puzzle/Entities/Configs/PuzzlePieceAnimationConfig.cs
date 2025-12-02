using UnityEngine;

namespace Puzzle
{
    public class PuzzlePieceAnimationConfig : ScriptableObject
    {
        [field: SerializeField] public float PunchPositionStrength { get; private set; } = 0.1f;
        [field: SerializeField] public float PunchScaleStrength { get; private set; } = -0.06f;
    }
}