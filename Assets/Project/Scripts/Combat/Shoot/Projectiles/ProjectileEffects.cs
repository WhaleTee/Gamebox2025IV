using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;

namespace Combat.Projectiles
{
    [Serializable]
    public class ProjectileEffects
    {
        [SerializeField] private ParticleSystem[] m_particles;
        [SerializeField] private TrailRenderer[] m_trails;
        [SerializeField] private Light2D m_light;

        private Tween fade;
        private string tweenID;
        private float opacity;
        private float lightIntensityCache;

        private float[] simulationSpeed;
        private float[] startLiftimeMultiplier;
        private float[] trailsLifetime;

        public void Init(Projectile projectile)
        {
            tweenID = $"Projectile_{projectile.gameObject.name}";
            fade = DOTween.To(() => opacity, SetOpacity, 1, 0.2f).Preserve(tweenID);

            simulationSpeed = new float[m_particles.Length];
            startLiftimeMultiplier = new float[m_particles.Length];

            for (int i = 0; i < m_particles.Length; i++)
            {
                var particle = m_particles[i];
                var main = particle.main;
                simulationSpeed[i] = main.simulationSpeed;
                startLiftimeMultiplier[i] = main.startLifetimeMultiplier;
            }

            trailsLifetime = new float[m_trails.Length];

            for (int i = 0; i < m_trails.Length; i++)
                trailsLifetime[i] = m_trails[i].time;

            if (m_light == null)
                return;
            lightIntensityCache = m_light.intensity;
            m_light.intensity = 0;
        }

        public void OnEnable()
        {
            fade.PlayForward();
            ParticlesPlay();
            TrailsPlay();
        }

        public UniTask OnDisable()
        {
            ParticlesStop();
            TrailsStop();

            fade.PlayBackwards();
            float waitFade = fade.Duration();
            float waitTrails = 0;
            if (m_trails.Length > 0)
                waitTrails = m_trails.Max(t => t.time);
            float wait = Mathf.Max(waitFade, waitTrails);

            return UniTask.Delay(Mathf.RoundToInt(wait * 1000));
        }

        private void SetOpacity(float opacity)
        {
            if (m_light != null)
                m_light.intensity = Mathf.Lerp(0, lightIntensityCache, opacity);
            this.opacity = opacity;
        }


        private void ParticlesPlay()
        {
            int particlesCount = m_particles.Length;
            for (int i = 0; i < particlesCount; i++)
            {
                var particle = m_particles[i];
                var main = particle.main;
                main.simulationSpeed = simulationSpeed[i];
                main.startLifetimeMultiplier = startLiftimeMultiplier[i];
                particle.Play(true);
            }
        }

        private void ParticlesStop()
        {
            int particlesCount = m_particles.Length;
            for (int i = 0; i < particlesCount; i++)
            {
                var particle = m_particles[i];
                var main = particle.main;
                main.simulationSpeed = 2f;
                main.startLifetimeMultiplier = 0.1f;
                particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        private void TrailsPlay()
        {
            for (int i = 0; i < m_trails.Length; i++)
            {
                var trail = m_trails[i];
                trail.Clear();
                trail.time = trailsLifetime[i];
                trail.emitting = true;
            }
        }

        private void TrailsStop()
        {
            for (int i = 0; i < m_trails.Length; i++)
            {
                var trail = m_trails[i];
                trail.time = trailsLifetime[i] * 0.3f;
                trail.emitting = false;
            }
        }
    }
}