using System;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Attributes;
using VisualEffects;

namespace Puzzle.Entities
{
    [Serializable]
    public class PieceParticles
    {
        [SerializeField] private ParticleSystem m_impactPrefab;
        [SerializeField] private int m_maxImpacts = 2;
        [SerializeField] private ParticleSystem m_chargePrefab;
        [SerializeField] private ParticleSystem m_deathPrefab;

        private List<ParticleSystem> impacts;
        private int currentImpact;

        private ParticleSystem deathEffect;
        private ParticleSystem chargeEffect;

        private GameObject gameObject;
        private Transform transform;

        private ParticlesInjectionData particlesData;

        public void PlayDamage()
        {

        }

        public void PlayCharged()
        {

        }

        public void Install(GameObject gameObject)
        {
            this.gameObject = gameObject;
            transform = gameObject.transform;
        }

        [Inject]
        private void Inject(ParticlesInjectionData particlesData)
        {
            this.particlesData = particlesData;

            deathEffect = Create(deathEffect);
            chargeEffect = Create(chargeEffect);

            impacts = new();

            for (int i = 0; i < m_maxImpacts; i++)
                impacts.Add(Create(m_impactPrefab));
        }

        private ParticleSystem Create(ParticleSystem prefab)
        {
            var effect = particlesData.Get(m_deathPrefab);
            effect.transform.SetPositionAndRotation(transform.position, transform.rotation);
            effect.transform.SetParent(transform);
            return effect;
        }

        private bool GetImpact(List<ParticleSystem> particles, out ParticleSystem particle)
        {
            particle = null;

            if (particles.Count < 1)
                return false;

            if (currentImpact >= particles.Count)
                currentImpact = 0;
            particle = particles[currentImpact];
            currentImpact++;
            return true;
        }
    }
}