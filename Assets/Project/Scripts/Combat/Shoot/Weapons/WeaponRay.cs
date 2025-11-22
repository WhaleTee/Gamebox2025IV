using UnityEngine;

namespace Combat.Weapon
{
    public class WeaponRay : Weapon<WeaponStatsRay>
    {
        [SerializeField] protected LayerMask layerMask;
        protected override void Shot(Vector2 origin)
        {
            bool success = CastRay(out var damageable, out var hit);
            if (!success)
                return;
            damageable?.InflictDamage(Config.Stats.Damage);
            Events.RayHit(origin, hit);
        }

        protected bool CastRay(out IDamageable damageable, out RaycastHit2D hitInfo)
        {
            damageable = null;
            hitInfo = default;
            if (FirePoint == null)
                return false;

            Ray ray = GetShotRay(FirePoint.position, FirePoint.forward, Config.Stats);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Config.Stats.Range, layerMask);
            bool success = hit.collider != null;

            if (success)
            {
                hitInfo = hit;
                damageable = hit.collider.GetComponent<IDamageable>();
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (hit.collider.attachedRigidbody != null)
                    hit.collider.attachedRigidbody.AddForceAtPosition(-hit.normal * 30f, hit.point, ForceMode2D.Impulse);
            }
            return success;
        }
    }
}