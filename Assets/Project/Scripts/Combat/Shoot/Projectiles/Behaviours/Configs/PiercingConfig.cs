using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Projectile/Behaviour/Piercing")]
    public class PiercingConfig : BehaviourBaseConfig
    {
        [field: SerializeField] public int MaxTargets { get; private set; }

        public override T Create<T>()
        {
            return CreateInstance<PiercingConfig>() as T;
        }
    }
}
