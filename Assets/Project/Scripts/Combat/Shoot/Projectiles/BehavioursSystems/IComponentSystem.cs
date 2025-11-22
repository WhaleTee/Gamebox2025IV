using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public interface IComponentSystem
    {
        void Register(int id, ProjectileConfig config);
        void Unregister(int id, ProjectileConfig config);
        void Unregister(int id);
        void Reset(int id);
        void Tick(float deltaTime);

        public TConfig GetConfig<TConfig>(int id) where TConfig : ScriptableObject;
        public bool TryGetComponent<TComponent>(int id, out TComponent component) where TComponent : struct;
        public bool TrySetComponent<TComponent>(int id, in TComponent component) where TComponent : struct;
    }
}