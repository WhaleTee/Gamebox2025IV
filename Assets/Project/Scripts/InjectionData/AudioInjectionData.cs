using System;
using UnityEngine;
using UnityEngine.Audio;
using Reflex.Attributes;
using Reflex.Extensions;
using Cysharp.Threading.Tasks;
using Type = Pooling.PoolType;
using Pool = Pooling.ObjectPoolManager;
using System.Collections.Generic;

namespace Audio
{
    [Serializable]
    public class AudioInjectionData : IInjectable
    {
        [field: SerializeField] public AudioRandomizationConfig RandomizationConfig { get; private set; }
        public Dictionary<MixerGroup, AudioMixerGroup> Groups { get { if (groups == null) Init(); return groups; } private set { groups = value; } }
        [SerializeField] private AudioMixerGroup m_combat;
        [SerializeField] private AudioMixerGroup m_environment;
        [SerializeField] private AudioMixerGroup m_ambient;
        [SerializeField] private AudioMixerGroup m_music;
        [SerializeField] private AudioMixerGroup m_voices;
        [Inject] private Pool pool;
        private Dictionary<MixerGroup, AudioMixerGroup> groups;

        public AudioSource Get(AudioSource prefab)
        {
            if (pool == null) pool = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetSceneContainer().Resolve<Pool>();
            return pool.SpawnObject(prefab, Vector3.one * 99999, Quaternion.identity, Type.SoundFX);
        }

        public async UniTask<AudioSource> GetAsync(AudioSource prefab)
        {
            if (pool == null)
                await UniTask.WaitUntil(() => pool != null);
            return Get(prefab);
        }
        public void Release(AudioSource source) => pool.ReturnObjectToPool(source.gameObject, Type.SoundFX);

        private void Init()
        {
            Groups = new()
            {
                { MixerGroup.Combat, m_combat },
                { MixerGroup.Environment, m_environment },
                { MixerGroup.Ambient, m_ambient },
                { MixerGroup.Music, m_music },
                { MixerGroup.Voices, m_voices }
            };
        }
    }
}