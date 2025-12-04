using System;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Attributes;
using VisualEffects;

namespace Combat
{
    [Serializable]
    public class DamageableParticles
    {
        [SerializeField] private DamageableParticlesConfig m_config;
        private List<ParticleSystem> impacts;
        private ParticleSystem deathEffect;
        private int currentImpact;

        private Transform transform;

        private Func<ParticleSystem, ParticleSystem> get;
        private Action<ParticleSystem> release;

        public void Install(Transform transform)
        {
            this.transform = transform;
            Populate();
        }

        public void PlayImpact(Vector2 point, Vector3 normal)
        {
            if (!GetCurrent(impacts, ref currentImpact, out var impact))
                return;

            var angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            impact.transform.SetPositionAndRotation(point, Quaternion.Euler(0, 0, angle + 90f));
            impact.Play();
        }

        public void PlayDeath()
        {
            deathEffect.Play();
            deathEffect.transform.position = transform.position;
        }

        private bool GetCurrent(List<ParticleSystem> particles, ref int index, out ParticleSystem particle)
        {
            particle = null;
            if (particles == null || particles.Count < 1)
                return false;

            if (index >= particles.Count)
                index = 0;

            particle = particles[index];
            index++;
            return true;
        }

        private void Populate()
        {
            deathEffect = get(m_config.DeathPrefab);
            deathEffect.transform.parent = transform;
            deathEffect.transform.SetPositionAndRotation(transform.position, transform.rotation);

            impacts = new();
            for (int i = 0; i < m_config.MaxImpacts; i++)
            {
                var impact = get(m_config.ImpactPrefab);
                impacts.Add(impact);
                deathEffect.transform.SetPositionAndRotation(transform.position, transform.rotation);
            }
        }

        [Inject]
        private void Inject(ParticlesInjectionData di)
        {
            get = di.Get;
            release = di.Release;
        }
    }
}
