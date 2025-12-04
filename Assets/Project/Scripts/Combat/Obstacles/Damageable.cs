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
        public event Action<IDamageSource> Killed;

        private IDamageSource lastDamageSource;

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

        public void SetDamageSource(IDamageSource damageSource)
        {
            lastDamageSource = damageSource;
        }

        public void InflictDamage(DamageBundle attack)
        {
            if (Health <= 0) return;
            if (attack.Can(m_takesDamageType))
            {
                int amount = attack.Damage[m_takesDamageType];
                Health -= amount;
                OnTakeDamage?.Invoke(amount, Health);
            }
            if (Health <= 0)
            {
                OnDeath?.Invoke();
                Killed?.Invoke(lastDamageSource);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            bool hit = collision.gameObject.CompareTag("Projectile");
            if (!hit)
                return;

            var projectile = collision.gameObject.GetComponent<Projectile>();
            bool canDealDamage = projectile.Weapon.GetDamage().Can(m_takesDamageType);
            if (!canDealDamage)
                return;

            var contact = collision.GetContact(0);
            OnImpact?.Invoke(contact.point, contact.normal);
        }

    }
}