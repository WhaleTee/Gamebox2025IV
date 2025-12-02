using System;
using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public class DefaultEvent
    {
        public Action<Collision2D> OnCollision;
        public Action<Collision2D> Defaults;

        private Projectile self;
        private DefaultSystem system;

        public DefaultEvent(Projectile self, DefaultSystem defaultSystem)
        {
            this.self = self;
            system = defaultSystem;

            Defaults = Freeze;
            Defaults += Disable;

            this.self.OnCollide += HandleCollision;
        }

        public void PreventDefault()
        {
            Defaults = null;
        }

        private void HandleCollision(Collision2D collision)
        {
            Defaults?.Invoke(collision);
            OnCollision?.Invoke(collision);
        }

        private void Freeze(Collision2D collision)
        {
            var comp = system.GetComponent(self.ID);
            comp.IsFrozen = true;
            system.SetComponent(self.ID, comp);
        }

        private void Disable(Collision2D collision)
        {
            self.Disable();
        }

        public void Dispose()
        {
            self.OnCollide -= HandleCollision;
            Defaults = null;
            OnCollision = null;
        }
    }
}
