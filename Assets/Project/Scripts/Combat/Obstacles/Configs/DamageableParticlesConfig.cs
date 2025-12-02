using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Obstacles/Effects/Particles")]
    public class DamageableParticlesConfig : ScriptableObject
    {
        [field: SerializeField] public ParticleSystem ImpactPrefab { get; private set; }
        [field: SerializeField] public int MaxImpacts { get; private set; } = 3;
        [field: SerializeField] public ParticleSystem DeathPrefab { get; private set; }
    }
}
