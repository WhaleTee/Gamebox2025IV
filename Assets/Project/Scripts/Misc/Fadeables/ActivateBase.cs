using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Misc
{
    [System.Serializable]
    public class ActivateBase : IActivate
    {
        public bool IsActive { get; private set; }
        public float Opacity { get; private set; } = 1;
        [SerializeField] private GameObject m_visual;
        [SerializeField] private List<SpriteRenderer> m_renderers;
        [SerializeField] private Collider2D m_collider;
        [SerializeField] private List<ParticleSystem> m_particles;
        [SerializeField] private List<Light2D> m_lights;
        [SerializeField] private List<ShadowCaster2D> m_shadows;

        public void SetActive(bool value)
        {
            if (m_visual != null)
                m_visual.SetActive(value);
            if (m_collider != null)
                m_collider.enabled = value;
            if (m_particles != null && m_particles.Count > 0)
                foreach (var particle in m_particles)
                {
                    if (value)
                        particle.Play(true);
                    else
                        particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
            if (m_lights != null)
                foreach (var light in m_lights)
                    light.enabled = value;
            if (m_shadows != null)
                foreach (var shadow in m_shadows)
                    shadow.enabled = value;

            IsActive = value;
        }

        public void SetActiveColliders(bool value)
        {
            if (m_collider != null)
                m_collider.enabled = value;
        }

        public void SetOpacity(float value)
        {
            foreach (var renderer in m_renderers)
            {
                var color = renderer.color;
                color.a = value;
                renderer.color = color;
            }
            foreach (var light in m_lights)
            {
                var color = light.color;
                color.a = value;
                light.color = color;
            }
        }
    }
}
