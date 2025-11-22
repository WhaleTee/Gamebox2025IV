using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Projectile/Behaviour/ChangeTarget")]
    public class ChangeTargetConfig : BehaviourBaseConfig
    {
        [field: SerializeField] public float MaxDistance { get; private set; }
        [field: SerializeField] public int MaxTargets { get; private set; }

        public override T Create<T>()
        {
            return CreateInstance<ChangeTargetConfig>() as T;
        }
    }
}
