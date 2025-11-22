using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Projectile/Behaviour/Homing")]
    public class HomingConfig : BehaviourBaseConfig
    {
        [field: SerializeField] public float MaxDistance { get; private set; }

        public override T Create<T>()
        {
            return CreateInstance<HomingConfig>() as T;
        }
    }
}
