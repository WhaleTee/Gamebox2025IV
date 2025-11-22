using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Projectile/Behaviour/Ricochet")]
    public class RicochetConfig : BehaviourBaseConfig
    {
        [field: SerializeField] public int MaxCount { get; private set; } = 5;
        [field: SerializeField] public float MaxLiftime { get; private set; } = 5f;

        public override T Create<T>()
        {
            return CreateInstance<RicochetConfig>() as T;
        }
    }
}
