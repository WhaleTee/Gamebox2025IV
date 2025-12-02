using System;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Attributes;
using VisualEffects;
using Cysharp.Threading.Tasks;

namespace Puzzle.Entities
{
    [Serializable]
    public class PieceParticles
    {
        [SerializeField] private PuzzlePieceParticlesConfig m_config;

        private List<ParticleSystem> impacts;
        private int currentImpact;

        private ParticleSystem deactivateEffect;
        private ParticleSystem dischargeEffect;
        private ParticleSystem chargeEffect;
        private ParticleSystem deathEffect;

        private SpriteRenderer spriteRenderer;
        private GameObject gameObject;
        private Transform transform;

        private Func<ParticleSystem, ParticleSystem> get;
        private Action<ParticleSystem> release;

        public void Install(GameObject gameObject)
        {
            this.gameObject = gameObject;
            transform = gameObject.transform;
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Populate();
            Debug.Log("Pre state of set all shapes");
            UniTask.Action(0.3f, SetAllShapes).Invoke();
        }

        public void SetActive(bool value)
        {
            if (value)
                return;

            var stop = ParticleSystemStopBehavior.StopEmitting;

            foreach (var i in impacts)
                i.Stop(true, stop);

            deathEffect.Stop(true, stop);
            chargeEffect.Stop(true, stop);
        }

        public void PlayDeactivate()
        {
            deactivateEffect.Play();
        }

        public void PlayCharge()
        {
            chargeEffect.Play();
        }

        public void PlayDischarge()
        {
            dischargeEffect.Play();
        }

        public void PlayImpact(Vector2 point, Vector3 normal, bool strong)
        {
            if (!GetImpact(impacts, out var impact))
                return;

            var angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            impact.transform.SetPositionAndRotation(point, Quaternion.Euler(0, 0, angle + 90f));
            impact.Play();
        }

        public void PlayDeath()
        {
            deathEffect.Play();
        }

        [Inject]
        private void Inject(ParticlesInjectionData di)
        {
            get = di.Get;
            release = di.Release;
        }

        private void Populate()
        {
            deathEffect = Create(m_config.DeathPrefab);
            chargeEffect = Create(m_config.ChargePrefab);

            impacts = new();

            for (int i = 0; i < m_config.MaxImpacts; i++)
                impacts.Add(Create(m_config.ImpactPrefab));
        }

        private ParticleSystem Create(ParticleSystem prefab)
        {
            var effect = get(prefab);
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

        private async UniTaskVoid SetAllShapes(float delay)
        {
            await UniTask.Delay(Mathf.RoundToInt(delay * 1000));

            Debug.Log("Invokes set all shapes");

            SetShapes(chargeEffect);
            SetShapes(deathEffect);

            foreach (var impact in impacts)
                SetShapes(impact);
        }

        private void SetShapes(ParticleSystem particle)
        {
            SetShape(particle);
            var subParticle = particle.GetComponentInChildren<ParticleSystem>();
            if (subParticle != null)
                SetShape(subParticle);
        }

        private void SetShape(ParticleSystem particle)
        {
            var shape = particle.shape;
            shape.spriteRenderer = spriteRenderer;
        }
    }
}