using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Obstacles/Effects/Animation")]
    public class DamageableAnimationConfig : ScriptableObject
    {
        [field: SerializeField] public float PunchPositionStrength { get; private set; } = 0.1f;
        [field: SerializeField] public float PunchScaleStrength { get; private set; } = -0.06f;
    }
}
