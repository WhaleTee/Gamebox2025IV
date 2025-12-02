using System;
using UnityEngine;
using Misc;
using Combat.Projectiles;

namespace Combat
{
    [RequireComponent(typeof(Collider2D))]
    public class Damageable : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [SerializeField] private DamageType m_takesDamageType;
        [SerializeField] private ActivateBase m_fadeable;
        [SerializeField] private DamageableEffects m_effects;
        public IActivate Activate;
        public int Health { get; private set; }

        public event Action<int, int> OnTakeDamage;
        public event Action<Vector2, Vector3> OnImpact;
        public event Action OnDeath;

        private void OnValidate()
        {
            MaxHealth = Mathf.Clamp(MaxHealth, 0, int.MaxValue);
        }

        private void Awake()
        {
            Activate = m_fadeable;
            Health = MaxHealth;
            m_effects.Install(this);
        }

        public void InflictDamage(Damage damage)
        {
            if (Health <= 0) return;
            if (damage.Type.Has(m_takesDamageType))
            {
                Health -= damage.amount;
                OnTakeDamage?.Invoke(damage.amount, Health);
            }
            if (Health <= 0)
                OnDeath?.Invoke();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            bool hit = collision.gameObject.CompareTag("Projectile");
            if (!hit)
                return;

            bool canDealDamage = collision.gameObject.GetComponent<Projectile>().WeaponProjectileStats.Damage.Type.Has(m_takesDamageType);
            if (!canDealDamage)
                return;

            var contact = collision.GetContact(0);
            OnImpact?.Invoke(contact.point, contact.normal);
        }
    }
}