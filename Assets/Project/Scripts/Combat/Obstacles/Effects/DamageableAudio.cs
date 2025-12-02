using System;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Attributes;
using Audio;

namespace Combat
{
    [Serializable]
    class DamageableAudio
    {
        [SerializeField] private DamageableAudioConfig m_config;
        private List<AudioSource> impactAudios;
        private int currentImpactAudio;
        private AudioSource deathAudio;

        private Func<AudioSource, AudioSource> get;
        private Action<AudioSource> release;

        private Transform transform;

        public void Install(Transform transform)
        {
            this.transform = transform;
            Populate();
        }

        public void PlayImpact(Vector2 point, Vector3 normal)
        {
            if (!GetCurrentSource(impactAudios, ref currentImpactAudio, out var impact))
                return;
            if (!GetSound(m_config.ImpactSound, out var clip))
                return;

            impact.resource = clip;
            var angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            impact.transform.SetPositionAndRotation(point, Quaternion.Euler(0, 0, angle + 90f));
            impact.Play();
        }

        public void PlayDeath()
        {
            deathAudio.Play();
        }

        private bool GetCurrentSource(List<AudioSource> sources, ref int index, out AudioSource source)
        {
            source = null;

            if (index >= m_config.PolyphonyLimit)
                index = 0;
            if (index >= sources.Count)
                return false;

            source = sources[index];
            index++;
            return true;
        }

        private bool GetSound(List<AudioClip> clips, out AudioClip clip)
        {
            clip = null;
            if (clips.Count < 1)
                return false;

            int index = UnityEngine.Random.Range(0, clips.Count);
            if (index >= clips.Count)
                return false;

            clip = clips[index];
            return true;
        }

        [Inject]
        private void Inject(AudioInjectionData di)
        {
            get = di.Get;
            release = di.Release;
        }

        private void Populate()
        {
            deathAudio = get(m_config.SourcePrefab);

            impactAudios = new();
            for (int i = 0; i < m_config.PolyphonyLimit; i++)
            {
                var impact = get(m_config.SourcePrefab);
                impact.transform.SetPositionAndRotation(transform.position, transform.rotation);
                impactAudios.Add(impact);
            }
        }
    }
}
