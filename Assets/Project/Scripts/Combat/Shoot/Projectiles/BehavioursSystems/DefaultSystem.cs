using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public class DefaultSystem : ComponentSystemBase<DefaultBehaviour, DefaultBehaviourConfig>
    {
        private Dictionary<int, DefaultEvent> events = new();

        public override void Register(int id, ProjectileConfig config, DefaultBehaviour defaultBehaviour)
        {
            components.Add(id, defaultBehaviour);
            configs.Add(id, config.DefaultConfig);
        }

        public override void Unregister(int id)
        {
            events[id].Dispose();
            base.Unregister(id);
            events.Remove(id);
        }

        public virtual void OnDefEvents(int id, Action<int> onCollision)
        {
            var self = components[id].Self;
            DefaultEvent defEvent = new(self, this);
            defEvent.Defaults += _ => onCollision?.Invoke(self.ID);
            events.Add(id, defEvent);
        }

        public override void Tick(float deltaTime)
        {
            var keys = components.Keys;
            foreach (var key in keys)
                Tick(key, deltaTime);
        }

        public void Tick(int id, float deltaTime)
        {
            var comp = components[id];
            var config = configs[id];

            if (comp.IsFrozen)
            {
                Debug.Log($"comp is frozen [{id}]");
                comp.Rigidbody.linearVelocity = Vector2.zero;
                return;
            }

            comp.Rigidbody.linearVelocity = comp.Direction * comp.Speed;
        }

        public bool TryGet(int id, out DefaultEvent defaultEvent)
        {
            defaultEvent = null;

            if (!events.ContainsKey(id))
                return false;
            
            defaultEvent = events[id];
            return true;
        }
    }
}
