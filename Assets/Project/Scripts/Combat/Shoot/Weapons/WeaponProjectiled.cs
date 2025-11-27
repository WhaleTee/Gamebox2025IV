using System;
using UnityEngine;
using Reflex.Attributes;
using Characters;
using Characters.Equipment;
using Combat.Projectiles;
using Combat.Projectiles.Behaviours;
using Pooling;

namespace Combat.Weapon
{
    public class WeaponProjectiled : Weapon<WeaponStatsProjectile>, IEquipment
    {
        [Inject] private ObjectPoolManager pool;
        [Inject] private BehavioursSystem behaviour;
        [Inject] private CameraInjectionData cameraData;
        [SerializeField] private ProjectilesRepository repository;
        public Func<DamageBundle> GetDamage;

        public void SetDamage(Func<DamageBundle> getDamage)
        {
            GetDamage = getDamage;
        }

        protected override void Shot(Vector2 origin)
        {
            var ray = GetDirection();
            var projectile = GetProjectile(ray.origin);
            behaviour.Register(projectile);
            PrepareProjectileData(projectile, ray.direction);
            projectile.OnDisable += OnProjectileDisable;
            projectile.Enable(FirePoint.position, Events, this);
        }

        private Projectile GetProjectile(Vector2 origin) =>
            pool.SpawnObject(repository[Config.Stats.Variant].Prefab, origin, FirePoint.rotation);

        private void PrepareProjectileData(Projectile projectile, Vector2 direction)
        {
            DefaultBehaviour data = behaviour.Get(projectile.ID);
            behaviour.Reset(projectile.ID);
            data.OriginTransform = transform;
            data.Direction = direction;
            data.Speed = Config.Stats.ProjectileSpeed;
            data.IsFrozen = false;
            behaviour.Set(projectile.ID, data);
        }

        protected void OnProjectileDisable(Projectile projectile)
        {
            projectile.OnDisable -= OnProjectileDisable;
            ReturnToPool(projectile);
        }
        private void ReturnToPool(Projectile projectile)
        {
            Debug.Log($"Return to pool {projectile.gameObject.name}", gameObject);
            pool.ReturnObjectToPool(projectile);
        }

        //private Projectile Create()
        //{
        //    var projectile = GameObject.Instantiate(repository[Config.Stats.Variant].Prefab, FirePoint.position, FirePoint.rotation);
        //    projectile.Disable();
        //    return projectile;
        //}

        private Ray2D GetDirection()
        {
            Vector3 mouseWorldPos = cameraData.Camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            mouseWorldPos.z = 0;

            var direction = (mouseWorldPos - FirePoint.position).normalized;

            return GetShotRay((Vector2)FirePoint.position, (Vector2)direction, Config.Stats);
        }
    }
}
