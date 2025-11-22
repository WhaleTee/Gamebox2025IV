using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Projectile/Behaviour/Default")]
    public class DefaultBehaviourConfig : BehaviourBaseConfig
    {
        [field: SerializeField] public float Speed;
        [field: SerializeField] public float LifeTime;

        public override T Create<T>()
        {
            return CreateInstance<DefaultBehaviourConfig>() as T;
        }
    }
}
