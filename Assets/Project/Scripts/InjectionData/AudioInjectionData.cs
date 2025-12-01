using System;
using UnityEngine;
using Type = Pooling.PoolType;
using Pool = Pooling.ObjectPoolManager;
using Reflex.Attributes;

namespace Sound
{
    [Serializable]
    public class AudioInjectionData : IInjectable
    {
        [field: SerializeField] public AudioRandomizationConfig RandomizationConfig { get; private set; }
        [SerializeField] private AudioSource audioSourcePrefab;
        [Inject] private Pool pool;
        public Action<AudioSource> Release => IRelease;
        public Func<AudioSource> Get => IGet;

        private AudioSource IGet() => pool.SpawnObject(audioSourcePrefab, Vector3.one * 99999, Quaternion.identity, Type.SoundFX);
        private void IRelease(AudioSource source) => pool.ReturnObjectToPool(source.gameObject, Type.SoundFX);
    }
}