using Combat;
using UnityEngine;
namespace LevelProgression
{
    public class CharacterStatsConfig : ScriptableObject
    {
        [field: SerializeField] public DamageBundle Damage { get; private set; }
        [field: SerializeField] public int Jumps { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }
    }
}