using UnityEngine;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Scriptables/Puzzle/Entities/Effects/Particles")]
    public class PuzzlePieceParticlesConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxImpacts { get; private set; } = 2;
        [field: SerializeField] public ParticleSystem ImpactPrefab { get; private set; }
        [field: SerializeField] public ParticleSystem ChargePrefab { get; private set; }
        [field: SerializeField] public ParticleSystem DeathPrefab { get; private set; }
    }
}