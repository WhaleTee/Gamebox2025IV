using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public abstract class BehaviourBaseConfig : ScriptableObject, IBehaviourConfig
    {
        [field: SerializeField] public BehaviourType Type;
        public virtual bool HasMe(BehaviourType type)
        {
            return type.Has(Type);
        }
        public abstract T Create<T>() where T : ScriptableObject, IBehaviourConfig;
    }
}
