using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Projectile/Behaviour/Sticky")]
    public class StickyConfig : BehaviourBaseConfig
    {
        [field: SerializeField] public float LifeTime { get; private set; }

        public override T Create<T>()
        {
            return CreateInstance<StickyConfig>() as T;
        }
    }
}