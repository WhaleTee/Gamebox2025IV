using System.Collections.Generic;
using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public partial class ComponentSystemBase<TComponent, TConfig> : IComponentSystem where TComponent : struct where TConfig : ScriptableObject
    {
        public BehaviourType Type { get; protected set; }
        protected Dictionary<int, TComponent> components = new();
        protected Dictionary<int, TConfig> configs = new();

        public virtual void Reset(int id)
        {
            if (!components.ContainsKey(id))
                return;
            components[id] = new();
        }

        public virtual void Register(int id, ProjectileConfig config, TComponent component = default)
        {
            Debug.Log($"Original behaviours [{config.BehavioursContainer.Behaviours.ToNiceString()}]");
            Debug.Log($"Expected [{Type.ToNiceString()}]. Actual is equal => [{config.Has(Type)}]");
            components.Add(id, component);
            var behaviourConfig = config.TryGet<TConfig>();
            configs.Add(id, behaviourConfig);
        }

        public virtual void Unregister(int id)
        {
            components.Remove(id);
            configs.Remove(id);
        }

        public virtual void Unregister(int id, ProjectileConfig config) => Unregister(id);

        public virtual void Tick(float deltaTime) { }

        public TConfig GetConfig(int id)
        {
            return configs[id];
        }

        public TComponent GetComponent(int id)
        {
            return components[id];
        }

        public void SetComponent(int id, TComponent component)
        {
            components[id] = component;
        }

        public bool CanGet(int id)
        {
            return components.ContainsKey(id) && configs.ContainsKey(id);
        }
    }
}
