using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Reflex.Attributes;
using Reflex.Extensions;
using Extensions;
using Audio;

namespace Combat.Weapon.Audio
{
    [Serializable]
    public class WeaponAudio
    {
        public float Duration => channels.
            Where(c => c != null).
            Select(c => c.GetRemainingDuration()).
            DefaultIfEmpty(0f).
            Max();

        [SerializeField] private WeaponConfigAudio m_config;
        private AudioRandomizationConfig randomiztaionConfig;
        private Func<AudioSource> getSource;
        private Action<AudioSource> release;
        private AudioSource[] channels;
        private int currentSlot;
        private int limit;

        [Inject]
        public void Inject(AudioInjectionData data)
        {
            randomiztaionConfig = data.RandomizationConfig;
            getSource = data.Get;
            release = data.Release;
        }

        public void Install(WeaponConfigAudio config, int limit)
        {
            this.m_config = config;
            this.limit = limit;
            channels = new AudioSource[limit];
            Inject(SceneManager.GetActiveScene().GetSceneContainer().Resolve<AudioInjectionData>());
        }

        public void Init()
        {
            for (int i = 0; i < limit; i++)
                channels[i] = getSource();
        }

        public void Play(SoundType soundType, Vector3 at, Transform parent, bool interrupt = true)
        {
            if (currentSlot >= limit)
                currentSlot = 0;

            var source = channels[currentSlot];

            if (source == null)
                source = getSource?.Invoke();

            if (!interrupt && source.isPlaying)
                return;

            if (!m_config.Sounds.ContainsKey(soundType))
                return;

            var sound = m_config.Sounds[soundType];
            if (sound == null)
                return;

            source.clip = sound;
            Randomize(source, soundType);
            source.transform.SetParent(parent);
            source.transform.position = at;
            source.Play();
            //source.OnFinished(() => Release(source));

            currentSlot++;
        }

        private void Release(AudioSource source)
        {
            source.transform.SetParent(null);
            release?.Invoke(source);
        }

        private void Randomize(AudioSource source, SoundType soundType)
        {
            var cfg = randomiztaionConfig;

            source.ClearRand();

            switch (soundType)
            {
                case SoundType.Shot:
                    source.PanRand(cfg).VolumeRand(cfg).PitchRand(cfg);
                    break;
                case SoundType.Exhausted:
                    source.PitchRand(cfg);
                    break;
                case SoundType.Cancel:
                    break;
                case SoundType.Start:
                    source.PitchRand(cfg);
                    break;
                case SoundType.Finish:
                    source.PitchRand(cfg);
                    break;
                default:
                    source.ClearRand();
                    break;
            }
        }
    }
}