using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Projectile/Behaviour/Joint")]
    public class JointConfig : BehaviourBaseConfig
    {
        [field: SerializeField] public float MaxDistance;

        public override T Create<T>()
        {
            return CreateInstance<JointConfig>() as T;
        }
    }
}
