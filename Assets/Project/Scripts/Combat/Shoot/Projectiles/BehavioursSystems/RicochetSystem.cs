using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Combat.Projectiles.Behaviours
{
    public class RicochetSystem : ComponentSystemBase<Ricochet, RicochetConfig>
    {
        public Action<int> OnEndOfLifeTime;
        private DefaultSystem defaultSystem;
        private Dictionary<int, Action<Collision2D>> listeners;

        public RicochetSystem Install(DefaultSystem defaultSystem)
        {
            this.defaultSystem = defaultSystem;
            Type = BehaviourType.Ricochet;
            listeners = new();
            return this;
        }

        public override void Register(int id, ProjectileConfig config, Ricochet component = default)
        {
            if (!config.Has(Type))
                return;
            base.Register(id, config, component);
            if (!CanGet(id))
                return;

            SubAll();
            AddEventListener(id);

            BeginTimer(id);
        }

        public override void Unregister(int id, ProjectileConfig config)
        {
            if (!config.Has(Type))
                return;

            UnsubAll();
            RemoveEventListener(id);

            base.Unregister(id, config);
        }

        public void OnCollide(Collision2D collision, int id)
        {
            Debug.Log("Ricochet triggered");
            var comp = components[id];
            var config = configs[id];

            if (comp.Count > config.MaxCount)
            {
                OnEndOfLifeTime?.Invoke(id);
                return;
            }

            comp.Count += 1;
            components[id] = comp;

            var baseComponent = defaultSystem.GetComponent(id);
            baseComponent.Direction = Vector2.Reflect(baseComponent.Direction, collision.contacts[0].normal).normalized;
            defaultSystem.SetComponent(id, baseComponent);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            var keys = components.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];

                if (Time.time > components[key].Deadline)
                    OnEndOfLifeTime?.Invoke(key);
            }
        }

        private void BeginTimer(int id)
        {
            var comp = components[id];
            var compConfig = configs[id];
            comp.TimeSinceStart = Time.time;
            comp.Deadline = Time.time + compConfig.MaxLiftime;
            components[id] = comp;
        }

        private void SubAll()
        {
            OnEndOfLifeTime += DisableProjectile;
            OnEndOfLifeTime += Unregister;
        }

        private void UnsubAll()
        {
            OnEndOfLifeTime -= DisableProjectile;
            OnEndOfLifeTime -= Unregister;
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
            if (!listeners.ContainsKey(id))
                return;

            bool success = defaultSystem.TryGet(id, out DefaultEvent ev);
            if (!success)
                return;

            var listener = listeners[id];

            ev.OnCollision -= listener;
            listeners.Remove(id);
        }

        private void DisableProjectile(int id)
        {
            defaultSystem.GetComponent(id).Self.Disable();
        }
    }
}
