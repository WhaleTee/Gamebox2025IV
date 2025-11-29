using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Misc;
using Extensions;
using UnityEngine.Serialization;

namespace Combat
{
    [Serializable]
    public class DamageableEffects
    {
        [Header("Visual Effects")]
        [SerializeField] private ParticleSystem m_impactPrefab;
        [SerializeField] private int m_maxImpacts = 3;
        [SerializeField] [FormerlySerializedAs("m_death")] private ParticleSystem m_deathPrefab;
        [SerializeField][FormerlySerializedAs("PunchPositionStrength")]  private float m_punchPositionStrength = 0.1f;
        [SerializeField] [FormerlySerializedAs("PunchScaleStrength")] private float m_punchScaleStrength = -0.06f;
        [Header("Audio")]
        [SerializeField] private AudioSource m_audioPrefab;
        [SerializeField] private int polyphonyLimit = 4;
        [SerializeField] private List<AudioClip> impactSound;
        [SerializeField] private AudioClip deathSound;

        private List<AudioSource> impactAudios;
        private int currentImpactAudio;
        private AudioSource deathAudio;
        private List<ParticleSystem> impacts;
        private ParticleSystem deathEffect;
        private int currentImpact;
        private Damageable damageable;

        private Tween punchPosition;
        private Tween punchScale;
        private string tweenID;

        public void Install(Damageable damageable)
        {
            this.damageable = damageable;
            SetActive(true);

            InstallAudio();
            InstallVisualEffects();
            InstallTweens();
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
            var impact = GetCurrentImpact();
            var angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            impact.transform.SetPositionAndRotation(point, Quaternion.Euler(0, 0, angle + 90f));
            impact.Play();
            GetCurrentImpactAudio().Play();
            PlayPunch(punchPosition);
            PlayPunch(punchScale);
        }
        private void PlayDeath()
        {
            ActivateBase activate = damageable.Activate as ActivateBase;
            activate.SetActiveColliders(false);

            deathEffect.transform.position = damageable.transform.position;
            deathEffect.Play();
            deathAudio.Play();
            Death(activate);
        }

        private async void Death(ActivateBase activate)
        {
            await DOTween.To(() => activate.Opacity, activate.SetOpacity, 0f, 1f).AsyncWaitForCompletion();
            damageable.Activate.SetActive(false);
        }

        private void PlayPunch(Tween punch)
        {
            if (punch.IsPlaying())
                punch.Complete();
            punch.Rewind();
            punch.Play();
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

        private ParticleSystem GetCurrentImpact()
        {
            var impact = impacts[currentImpact];
            currentImpact++;
            if (currentImpact >= m_maxImpacts)
                currentImpact = 0;
            return impact;
        }

        private AudioSource GetCurrentImpactAudio()
        {
            var impact = impactAudios[currentImpactAudio];
            currentImpactAudio++;
            if (currentImpactAudio >= polyphonyLimit)
                currentImpactAudio = 0;

            int index = UnityEngine.Random.Range(0, impactSound.Count);
            if (index >= impactSound.Count)
                return impact;

            impact.clip = impactSound[index];

            return impact;
        }

        private void InstallAudio()
        {
            deathAudio = GameObject.Instantiate(m_audioPrefab);

            impactAudios = new();

            for (int i = 0; i < polyphonyLimit; i++)
            {
                var impactAudio = GameObject.Instantiate(m_audioPrefab);
                impactAudios.Add(impactAudio);
            }
        }

        private void InstallVisualEffects()
        {
            deathEffect = GameObject.Instantiate(m_deathPrefab, damageable.transform.position, damageable.transform.rotation, damageable.transform);

            impacts = new();

            for (int i = 0; i < m_maxImpacts; i++)
                impacts.Add(GameObject.Instantiate(m_impactPrefab));
        }

        private void InstallTweens()
        {
            tweenID = $"DamageableEffect_{damageable.gameObject.name}";
            punchPosition = damageable.transform.DOPunchPosition(damageable.transform.right * m_punchPositionStrength, 0.2f, 30);
            punchScale = damageable.transform.DOPunchScale(Vector3.one * m_punchScaleStrength, 0.2f, 30);
            punchPosition.Preserve(tweenID);
            punchScale.Preserve(tweenID);
        }
    }
}