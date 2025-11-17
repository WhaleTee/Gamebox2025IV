using System;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageableObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private DamageType takesDamageType;
        [SerializeField] private int health;

        public event Action OnDeath;

        private void OnValidate()
        {
            health = Mathf.Clamp(health, 0, int.MaxValue);
        }

        public void InflictDamage(Damage damage)
        {
            if (health <= 0) return;
            if (damage.type == takesDamageType) health -= damage.amount;
            if (health <= 0) OnDeath?.Invoke();
        }
    }
}