using UnityEngine;

namespace Puzzle.Entities
{
    [CreateAssetMenu(menuName = "Scriptables/Puzzle/Entities/Effects/Animation/Lever")]
    public class LeverAnimationConfig : PolyCrystalAnimationConfig
    {
        [field: SerializeField] public Vector2 PositionOn { get; private set; }
        [field: SerializeField] public Vector2 PositionOff { get; private set; }
        [field: SerializeField] public Quaternion RotationOn { get; private set; }
        [field: SerializeField] public Quaternion RotationMiddlePoint { get; private set; }
        [field: SerializeField] public Quaternion RotationOff { get; private set; }
    }
}
