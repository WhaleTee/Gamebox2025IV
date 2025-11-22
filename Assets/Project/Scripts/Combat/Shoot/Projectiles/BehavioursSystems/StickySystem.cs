using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public class StickySystem : ComponentSystemBase<Sticky, StickyConfig>
    {
        public Action<int> OnEndOfLifeTime;
        private DefaultSystem defaultSystem;
        private Dictionary<int, Action<Collision2D>> listeners;

        public StickySystem Install(DefaultSystem defaultSystem)
        {
            Type = BehaviourType.Sticky;
            this.defaultSystem = defaultSystem;
            listeners = new();
            OnEndOfLifeTime += id => defaultSystem.GetComponent(id).Self.Disable();
            return this;
        }

        public override void Register(int id, ProjectileConfig config, Sticky component = default)
        {
            Debug.Log($"Trying to register [{id}]. Of type [{config.BehavioursContainer.Behaviours}] has Type [{Type}] = [{config.Has(Type)}]");
            if (!config.Has(Type))
                return;
            base.Register(id, config, component);
            AddEventListener(id);
        }

        public override void Unregister(int id, ProjectileConfig config)
        {
            if (!config.Has(Type))
                return;
            base.Unregister(id, config);
            RemoveEventListener(id);
        }

        public override void Reset(int id)
        {
            base.Reset(id);
        }

        public override void Tick(float deltaTime)
        {
            var keys = components.Keys;
            foreach (var key in keys)
            {
                Debug.Log($"[{key}] Contains Component [{components.ContainsKey(key)}]");
                Debug.Log($"[{key}] Contains Component [{configs.ContainsKey(key)}]");

                var comp = components[key];
                var config = configs[key];

                if (comp.Sticked && comp.TimeSinceStart > config.LifeTime)
                    OnEndOfLifeTime?.Invoke(key);
            }
        }

        public void OnCollide(Collision2D collision, int id)
        {
            var comp = components[id];
            var config = configs[id];

            comp.TimeSinceStart = Time.time;
            comp.Sticked = true;

            components[id] = comp;

            var baseComponent = defaultSystem.GetComponent(id);
            baseComponent.IsFrozen = true;
            defaultSystem.SetComponent(id, baseComponent);
        }

        private void AddEventListener(int id)
        {
            bool success = defaultSystem.TryGet(id, out DefaultEvent ev);
            if (!success)
                return;

            ev.PreventDefault();
            Action<Collision2D> listener = collision => OnCollide(collision, id);
            ev.OnCollision += listener;
            listeners.Add(id, listener);
        }

        private void RemoveEventListener(int id)
        {
            bool success = defaultSystem.TryGet(id, out DefaultEvent ev);
            if (!success)
                return;

            var listener = listeners[id];
            ev.OnCollision -= listener;
            listeners.Remove(id);
        }
    }
}
