using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Projectile/Behaviour/Boomerang")]
    public class BoomerangConfig : BehaviourBaseConfig
    {
        [field: SerializeField] public float DelayBeforeReturn { get; private set; }
        [field: SerializeField] public bool OverrideSpeed { get; private set; }
        [field: SerializeField] public float ReturnSpeed { get; private set; }

        public override T Create<T>()
        {
            return CreateInstance<BoomerangConfig>() as T;
        }
    }
}
