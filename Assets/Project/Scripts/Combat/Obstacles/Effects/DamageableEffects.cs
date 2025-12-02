using System;
using UnityEngine;
using Extensions;
using Misc;

namespace Combat
{
    [Serializable]
    public class DamageableEffects
    {
        [SerializeField] private DamageableAnimation m_animation;
        [SerializeField] private DamageableAudio m_audio;
        [SerializeField] private DamageableParticles m_particles;

        private Damageable damageable;

        public void Install(Damageable damageable)
        {
            this.damageable = damageable;
            SetActive(true);

            m_audio.InjectAttributes();
            m_animation.InjectAttributes();
            m_particles.InjectAttributes();

            m_audio.Install(damageable.transform);
            m_particles.Install(damageable.transform);
            m_animation.Install(damageable.gameObject);
        }

        public void SetActive(bool value)
        {
            if (value)
                SubAll();
            else
                UnsubAll();
        }

        private void PlayImpact(Vector2 point, Vector3 normal)
        {
            m_particles.PlayImpact(point, normal);
            m_audio.PlayImpact(point, normal);
            m_animation.PlayImpact(point, normal);
        }
        private void PlayDeath()
        {
            ActivateBase activate = damageable.Activate as ActivateBase;
            activate.SetActiveColliders(false);

            m_particles.PlayDeath();
            m_audio.PlayDeath();
            m_animation.DeathFadeOut(activate);
        }
        private void SubAll()
        {
            damageable.OnDeath += PlayDeath;
            damageable.OnImpact += PlayImpact;
        }
        private void UnsubAll()
        {
            damageable.OnDeath -= PlayDeath;
            damageable.OnImpact -= PlayImpact;
        }
    }
}