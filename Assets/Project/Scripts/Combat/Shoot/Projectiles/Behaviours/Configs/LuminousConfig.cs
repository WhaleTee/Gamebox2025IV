using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    [CreateAssetMenu(menuName = "Scriptables/Combat/Projectile/Behaviour/Luminous")]
    public class LuminousConfig : BehaviourBaseConfig
    {
        [field: SerializeField] public float Intensity;
        [field: SerializeField] public float Distance;
        [field: SerializeField] public float LifeTime;
        [field: SerializeField] public float FadeIn;
        [field: SerializeField] public float FadeOut;

        public override T Create<T>()
        {
            return CreateInstance<LuminousConfig>() as T;
        }
    }
}