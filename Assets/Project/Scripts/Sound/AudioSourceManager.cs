using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class FootstepAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] sounds;

        public void PlayOneShot()
        {
            
        }
    }
    
    [RequireComponent(typeof(AudioSource))]
    public abstract class AudioSourceManager<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] private AudioWithType<T>[] sounds;
        private AudioSource audioSource;
        private HashSet<AudioWithType<T>> playingAudio = new HashSet<AudioWithType<T>>();
        private CancellationTokenSource cts = new CancellationTokenSource();

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private async UniTaskVoid PlayOneShotAsync(AudioWithType<T> audio)
        {
            if (audio.audio == null || !playingAudio.Add(audio)) return;
            var clip = audio.audio[Random.Range(0, audio.audio.Length)];
            audioSource.PlayOneShot(clip, SoundSettings.Instance.GetVolume(audio.soundType));
            await UniTask.WaitForSeconds(clip.length, cancelImmediately: true, cancellationToken: cts.Token);
            playingAudio.Remove(audio);
        }
        
        public virtual void PlayOneShot(T type)
        {
            cts = new CancellationTokenSource();
            UniTask.Void(() => PlayOneShotAsync(sounds.FirstOrDefault(s => s.type.Equals(type))));
        }
        
        public void Stop()
        {
            audioSource.Stop();
            cts.Cancel();
        }
    }
}