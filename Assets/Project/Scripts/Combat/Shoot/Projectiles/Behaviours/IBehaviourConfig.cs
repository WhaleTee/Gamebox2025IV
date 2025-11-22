using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public interface IBehaviourConfig
    {
        bool HasMe(BehaviourType behaviours);
        T Create<T>() where T : ScriptableObject, IBehaviourConfig;
    }
}
