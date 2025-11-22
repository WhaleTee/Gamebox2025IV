using System;
using UnityEngine;

namespace Combat.Weapon
{
    public class Events
    {
        public event Action<Vector2> OnShot;
        public event Action<Vector2, RaycastHit2D> OnRayHit;
        public event Action<Vector2, Vector3> OnProjectileHit;
        public event Action OnStart;
        public event Action OnContinuous;
        public event Action OnFinish;
        public event Action OnExhausted;

        private Transform firePoint;
        public Events(Transform firePoint)
        {
            this.firePoint = firePoint;
        }

        public void Shoot() => Shoot(firePoint.position);
        public void Shoot(Vector2 origin) => OnShot?.Invoke(origin);
        public void RayHit(Vector2 origin, RaycastHit2D hit) => OnRayHit?.Invoke(origin, hit);
        public void ProjectileHit(Vector2 hitPoint, Vector3 normal) => OnProjectileHit?.Invoke(hitPoint, normal);
        public void Start() => OnStart?.Invoke();
        public void Continue() => OnContinuous?.Invoke();
        public void Finish() => OnFinish?.Invoke();
        public void Exhaust() => OnExhausted?.Invoke();
    }
}
