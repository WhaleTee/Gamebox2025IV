using System;
using UnityEngine;

namespace Combat.Projectiles.Behaviours
{
    public struct DefaultBehaviour
    {
        public Projectile Self;
        public Rigidbody2D Rigidbody;
        public Vector2 Origin;
        public Transform OriginTransform;
        public Vector2 Direction;
        public float Speed;
        public bool IsFrozen;

        public DefaultBehaviour(Projectile self, Rigidbody2D rigidbody, float speed)
        {
            Self = self;
            Rigidbody = rigidbody;
            OriginTransform = null;
            Origin = Vector2.zero;
            Direction = Vector2.zero;
            Speed = speed;
            IsFrozen = false;
        }
    }
}