using UnityEngine;

namespace Puzzle.Entities
{
    [CreateAssetMenu(menuName = "Scriptables/Puzzle/Entities/Effects/Animation/MagicSpehere")]
    public class MagicSphereAnimationConfig : PolyCrystalAnimationConfig
    {
        [field: SerializeField] public Sprite Broken { get; private set; }
    }
}