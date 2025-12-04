using System;
using UnityEngine;
using UnityEngine.Audio;
using Reflex.Attributes;
using Reflex.Extensions;
using Cysharp.Threading.Tasks;
using Type = Pooling.PoolType;
using Pool = Pooling.ObjectPoolManager;
using System.Collections.Generic;

namespace Sound
{
    [Serializable]
    public class AudioInjectionData : IInjectable
    {
        [field: SerializeField] public AudioRandomizationConfig RandomizationConfig { get; private set; }
        public Dictionary<SoundType, AudioMixerGroup> Groups { get { if (groups == null) Init(); return groups; } private set { groups = value; } }
        [SerializeField] private AudioMixerGroup m_master;
        [SerializeField] private AudioMixerGroup m_music;
        [SerializeField] private AudioMixerGroup m_interface;
        [SerializeField] private AudioMixerGroup m_voice;
        [SerializeField] private AudioMixerGroup m_ambient;
        [SerializeField] private AudioMixerGroup m_sfx;
        [SerializeField] private AudioMixerGroup m_combat;
        [SerializeField] private AudioMixerGroup m_movement;
        [SerializeField] private AudioMixerGroup m_environment;
        [Inject] private Pool pool;
        private Dictionary<SoundType, AudioMixerGroup> groups;

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
                { SoundType.Master, m_master },
                { SoundType.Music, m_music },
                { SoundType.UI, m_interface },
                { SoundType.Voice, m_voice },
                { SoundType.Ambient, m_ambient },
                { SoundType.SFX, m_sfx },
                { SoundType.Combat, m_combat },
                { SoundType.Movement, m_movement },
                { SoundType.Environment, m_environment },
            };
        }
    }
}