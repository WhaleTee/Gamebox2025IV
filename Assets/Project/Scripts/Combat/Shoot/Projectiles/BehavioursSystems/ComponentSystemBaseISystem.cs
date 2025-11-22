using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public partial class ComponentSystemBase<TComponent, TConfig> : IComponentSystem where TComponent : struct where TConfig : ScriptableObject
    {
        void IComponentSystem.Register(int id, ProjectileConfig config)
        {
            Register(id, config);
        }

        T IComponentSystem.GetConfig<T>(int id)
        {
            return GetConfig(id) as T;
        }

        bool IComponentSystem.TryGetComponent<TComp>(int id, out TComp result)
        {
            if (typeof(TComp) == typeof(TComponent))
            {
                result = (TComp)(object)components[id];
                return true;
            }

            result = default;
            return false;
        }

        bool IComponentSystem.TrySetComponent<TComp>(int id, in TComp component)
        {
            if (typeof(TComp) != typeof(TComponent))
                return false;

            SetComponent(id, (TComponent)(object)component);
            return true;
        }
    }
}