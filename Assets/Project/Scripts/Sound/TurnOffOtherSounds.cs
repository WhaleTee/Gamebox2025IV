using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class TurnOffOtherSounds: MonoBehaviour
    {
        [SerializeField] private SoundType type;
        [SerializeField] [Range(1, 5)] private float multiplier;
        private AudioSource audioSource;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            var volume = 1 - audioSource.volume;
            Debug.Log($"{audioSource.volume}, {volume}");
            SoundSettings.Instance.SetMixerVolume(SoundType.Music, volume);
        }
    }
}