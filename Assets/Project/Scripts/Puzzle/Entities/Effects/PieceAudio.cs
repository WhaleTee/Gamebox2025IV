using System;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Attributes;
using Audio;

namespace Puzzle.Entities
{
    [Serializable]
    public class PieceAudio
    {
        [SerializeField] private PuzzlePieceAudioConfig m_config;

        private AudioSource deactivationAudio;
        private AudioSource chargeAudio;
        private AudioSource dischargeAudio;
        private AudioSource deathAudio;

        private List<AudioSource> impactAudios;
        private int currentImpact;

        private Transform transform;

        private Func<AudioSource, AudioSource> get;
        private Action<AudioSource> release;

        public void Install(Transform transform)
        {
            this.transform = transform;
            Populate();
        }
        public void PlayCharge() => chargeAudio.Play();
        public void PlayDischarge() => dischargeAudio.Play();
        public void PlayDeactivate() => deactivationAudio.Play();
        public void PlayDeath() => deathAudio.Play();

        public void PlayImpact(Vector2 point, Vector3 normal, bool strong)
        {
            var impact = GetCurrent(impactAudios, ref currentImpact);
            var clip = strong ? m_config.DamagedStrong : m_config.Damaged;
            impact.resource = clip;

            var angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            impact.transform.SetPositionAndRotation(point, Quaternion.Euler(0, 0, angle + 90f));
            impact.Play();
        }

        private AudioSource GetCurrent(List<AudioSource> audios, ref int current)
        {
            if (current >= audios.Count)
                current = 0;

            var curAudio = audios[current];
            current++;
            return curAudio;
        }

        private void Populate()
        {
            deactivationAudio = Get();
            deactivationAudio.resource = m_config.Deactivated;
            dischargeAudio = Get();
            dischargeAudio.resource = m_config.Discharged;
            deathAudio = Get();
            deathAudio.resource = m_config.Broken;
            chargeAudio = Get();
            chargeAudio.resource = m_config.Charged;

            impactAudios = new();
            for (int i = 0; i < m_config.PolyphonyLimit; i++)
            {
                var impact = Get();
                impact.resource = m_config.Damaged;
                impact.transform.SetPositionAndRotation(transform.position, transform.rotation);
                impactAudios.Add(impact);
            }
        }

        private AudioSource Get() => get(m_config.SourcePrefab);

        [Inject]
        private void Inject(AudioInjectionData di)
        {
            get = di.Get;
            release = di.Release;
        }
        public void SetActive(bool value) { }
    }
}