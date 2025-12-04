using UnityEngine;
using Reflex.Attributes;
using Sound;

namespace Audio
{
    public abstract class AudioConfigBase : ScriptableObject
    {
        public AudioSource SourcePrefab { get { InitPrefab(); return sourcePrefab; } }
        protected SoundType mixerGroup;
        protected AudioSource sourcePrefab;

        protected abstract void InitPrefab();

        protected void InitPrefab(string prefabName)
        {
            if (sourcePrefab == null) sourcePrefab = CreatePrefab(prefabName);
        }

        private AudioSource CreatePrefab(string prefabName)
        {
            var prefab = new GameObject(prefabName);
            var audio = prefab.AddComponent<AudioSource>();
            audio.playOnAwake = false;
            audio.loop = false;
            prefab.SetActive(false);

            return audio;
        }

        [Inject]
        protected void Inject(AudioInjectionData di)
        {
            SourcePrefab.outputAudioMixerGroup = di.Groups[mixerGroup];
        }
    }
}