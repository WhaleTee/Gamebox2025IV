using System;
using UnityEngine;
using Combat.Weapon;
using Characters;

namespace Combat.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
        public event Action<Collision2D> OnCollide;
        public event Action<Collision2D> OnCollideExit;
        public event Action<Projectile> OnDisable;
        public ProjectileConfig Config { get; private set; }
        public WeaponProjectiled Weapon { get; private set; }
        [SerializeField] private Collider2D m_collider;
        [SerializeField] private Variant m_variant;
        [SerializeField] private ProjectilesRepository m_dataRepository;
        [SerializeField] private ProjectileEffects m_effects;

        private Events weaponEvents;
        private bool isEnabled;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isEnabled)
            {
                if (IgnoreTag(collision.gameObject)) return;
                OnCollide?.Invoke(collision); isEnabled = false;
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (isEnabled)
            {
                if (IgnoreTag(collision.gameObject)) return;
                OnCollideExit?.Invoke(collision);
            }    
        }

        private bool IgnoreTag(GameObject go) => go.CompareTag("Platform") || go.CompareTag("Projectile");

        public void Awake() => m_effects.Init(this);

        public void Enable(Vector3 spawnPoint, Events events, WeaponProjectiled weapon)
        {
            transform.position = spawnPoint;

            isEnabled = true;
            m_collider.enabled = true;

            this.Weapon = weapon;
            this.weaponEvents = events;
            OnCollide += _ => Debug.Log($"On Collide {gameObject.name}", gameObject);
            OnCollide += DealDamage;
            OnCollide += ProjectileHit;

            m_effects.OnEnable();
        }

        public void Disable()
        {
            isEnabled = false;
            m_collider.enabled = false;
            OnCollide -= DealDamage;
            OnCollide -= ProjectileHit;
            OnCollide = null;
            DelayedDisable();
        }

        public void SetID(int id)
        {
            this.ID = id;

            Config = m_dataRepository[m_variant].Config;
        }

        private void DealDamage(Collision2D target)
        {
            if (target.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                var damage = Weapon.GetDamage();
                damageable.SetDamageSource(Weapon);
                damageable.InflictDamage(Weapon.GetDamage());
            }
        }

        private void ProjectileHit(Collision2D target)
        {
            var contact = target.contacts[0];
            weaponEvents.ProjectileHit(contact.point, contact.normal);
        }

        private async void DelayedDisable()
        {
            await m_effects.OnDisable();

            OnDisable?.Invoke(this);
        }
    }
}